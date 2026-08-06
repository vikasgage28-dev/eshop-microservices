# 🎓 Kubernetes & AKS Master Log — EShop Microservices

> **Purpose:** Single source of truth for everything Kubernetes/AKS in this project —
> the step-by-step build log, every error hit and how it was fixed, a command
> cheat-sheet, the reasoning behind architectural choices, and a skill checklist
> for interview prep.
> Companion to `PROGRESS.md` (session state) and `Learnings.md` (full project history).
> **This file is living — update it every time AKS/K8s work happens, until Stage 8 is 100% done.**

**Current status:** Stage 8 (AKS deployment) **100% COMPLETE** ✅
All phases done: cluster, SQL pod, Workload Identity, PEM key mounting, all 4 microservices
deployed + verified, NGINX Ingress live with HTTPS/TLS (cert-manager + Let's Encrypt),
React frontend deployed on Azure Static Web Apps, and end-to-end login verified.
Cluster stopped (`az aks stop`) — resume with `az aks start` before next AKS stage.

---

## 📑 Table of Contents
1. [Architecture Overview](#1-architecture-overview)
2. [Step-by-Step Implementation Log](#2-step-by-step-implementation-log)
3. [Troubleshooting Battle Log](#3-troubleshooting-battle-log)
4. [Command Reference](#4-command-reference)
5. [Architectural Decisions & Production Translation](#5-architectural-decisions--production-translation)
6. [Skill Mapping (Interview Checklist)](#6-skill-mapping-interview-checklist)

---

## 1. Architecture Overview

**Cluster:** `aks-eshop` (resource group `rg-eshop-microservices`) — 1 node, `Standard_B2s`, Standard HDD.
**Namespace:** `eshop`

**Resources (`k8s/` folder):**
```
k8s/
├── namespace.yaml
├── ingress.yaml                    (NGINX — TLS + path routing; cert-manager annotation;
│                                    hostname: eshop-api.centralindia.cloudapp.azure.com)
├── cert-manager/
│   └── cluster-issuer.yaml         (Let's Encrypt production ClusterIssuer, HTTP-01)
├── sql-server/
│   ├── storageclass.yaml           (disk.csi.azure.com, Standard_LRS, WaitForFirstConsumer)
│   ├── pvc.yaml                    (32Gi ReadWriteOnce)
│   ├── secret.yaml                 (SA_PASSWORD)
│   ├── deployment.yaml             (fsGroup 10001, TCP liveness/readiness probes)
│   └── service.yaml                (ClusterIP :1433)
├── catalog-api/   (deployment.yaml, service.yaml, configmap.yaml)
├── customer-api/  (deployment.yaml, service.yaml, configmap.yaml)
├── identity-api/  (deployment.yaml, service.yaml, configmap.yaml + PEM secret volume mount)
└── ordering-api/  (deployment.yaml, service.yaml, configmap.yaml)
```

**Traffic flow (live — HTTPS):**
```
Browser (SWA HTTPS) → https://eshop-api.centralindia.cloudapp.azure.com
        → Azure Public IP 4.187.191.129
        → Azure Load Balancer (TCP health probe)
        → AKS node NodePort (443→31403)
        → ingress-nginx-controller pod (TLS terminated — Let's Encrypt cert)
        → path match per k8s/ingress.yaml:
  /api/products   → catalog-api   (ClusterIP :80 → pod :8080)
  /api/categories → catalog-api   (ClusterIP :80 → pod :8080)
  /api/reviews    → catalog-api   (ClusterIP :80 → pod :8080)
  /api/customers  → customer-api  (ClusterIP :80 → pod :8080)
  /api/orders     → ordering-api  (ClusterIP :80 → pod :8080)
  /api/auth       → identity-api  (ClusterIP :80 → pod :8080)
                                       ↓
                          sql-server (ClusterIP :1433, PVC-backed)

cert-manager watches Certificate object → renews Let's Encrypt cert automatically
before expiry (every 60 days, renewed at 30 days left)
```
**Frontend (SWA):**
```
https://proud-plant-008c4b200.7.azurestaticapps.net  (Azure Static Web Apps — FREE tier)
  → React SPA (Vite bundle — env vars baked at build time)
  → VITE_API_*_URL = https://eshop-api.centralindia.cloudapp.azure.com (GitHub Actions Variables)
  → staticwebapp.config.json → navigationFallback → /index.html (SPA routing)
```
**Note:** catalog-api is reached via three separate path prefixes, not a single
`/api/catalog` — its controllers are `[Route("api/products")]`, `[Route("api/categories")]`,
and `[Route("api/reviews")]`. There is no rewrite annotation, so Ingress paths must match
the app's real routes exactly.

**Passwordless identity chain (Workload Identity):**
```
Pod (label azure.workload.identity/use=true, ServiceAccount=eshop-sa)
  → OIDC token auto-projected by AKS (mutating webhook)
  → Federated credential trust: id-eshop-workload ⟷ system:serviceaccount:eshop:eshop-sa
  → Azure AD token issued — zero stored secrets anywhere
  → RBAC: Key Vault Secrets User + App Configuration Data Reader
```


## 2. Step-by-Step Implementation Log

### Phase 1 — Cluster Provisioning ✅
```
az aks create --resource-group rg-eshop-microservices --name aks-eshop \
  --node-count 1 --node-vm-size Standard_B2s --node-osdisk-type Standard_LRS \
  --generate-ssh-keys
az aks get-credentials --resource-group rg-eshop-microservices --name aks-eshop
kubectl apply -f k8s/namespace.yaml
```
**Learned:** managed control plane (free) vs. paid worker nodes; `get-credentials` merges
cluster context into local kubeconfig.

### Phase 2 — SQL Server in AKS ✅
```
kubectl apply -f k8s/sql-server/storageclass.yaml
kubectl apply -f k8s/sql-server/pvc.yaml
kubectl apply -f k8s/sql-server/secret.yaml
kubectl apply -f k8s/sql-server/deployment.yaml
kubectl apply -f k8s/sql-server/service.yaml
kubectl get pods -n eshop -w
```
**Hit + fixed:** volume permission denied on `/var/opt/mssql` → SQL Server's container runs
as non-root UID 10001 but the Azure Disk PVC mounted root-owned by default. Fixed by adding
`securityContext.fsGroup: 10001` at the **pod** level in `deployment.yaml`.
**Learned:** PVC lifecycle, StorageClass provisioner (`disk.csi.azure.com`),
`WaitForFirstConsumer` binding mode, `fsGroup` security context.

### Phase 3 — Central Config Update ✅
Updated Key Vault connection strings for all 4 databases:
`Server=sql-server,1433;Database=<DbName>;User Id=sa;Password=...`
**Learned:** Kubernetes Service DNS — `sql-server` resolves automatically to the pod's
ClusterIP inside the `eshop` namespace; no hardcoded IPs needed.

### Phase 3b — Workload Identity (OIDC Federation) ✅
```
az aks update -g rg-eshop-microservices -n aks-eshop \
  --enable-oidc-issuer --enable-workload-identity
az identity create -g rg-eshop-microservices -n id-eshop-workload
az role assignment create --assignee <clientId> --role "Key Vault Secrets User" --scope <kv-id>
az role assignment create --assignee <clientId> --role "App Configuration Data Reader" --scope <appconfig-id>
az identity federated-credential create \
  --name eshop-fed-cred --identity-name id-eshop-workload \
  --resource-group rg-eshop-microservices \
  --issuer <oidc-issuer-url> \
  --subject system:serviceaccount:eshop:eshop-sa
kubectl create serviceaccount eshop-sa -n eshop
kubectl annotate sa eshop-sa -n eshop azure.workload.identity/client-id=<clientId>
kubectl label sa eshop-sa -n eshop azure.workload.identity/use=true
```
Every `deployment.yaml` → `serviceAccountName: eshop-sa` + pod label
`azure.workload.identity/use: "true"`. App code unchanged — `DefaultAzureCredential()`
auto-discovers the projected token at runtime.
**Learned:** OIDC federation, zero-secret Azure auth pattern, mutating webhook token
projection.

### Phase 3b (extra) — RS256 JWT PEM Key Mounting ✅
```
kubectl create secret generic identity-pem-keys -n eshop \
  --from-file=private.pem=./private.pem --from-file=public.pem=./public.pem
```
`identity-api/deployment.yaml`:
```yaml
volumeMounts:
  - name: pem-keys
    mountPath: /app/private.pem
    subPath: private.pem
  - name: pem-keys
    mountPath: /app/public.pem
    subPath: public.pem
volumes:
  - name: pem-keys
    secret:
      secretName: identity-pem-keys
```
**Learned:** mounting individual files out of a Secret via `subPath` (vs. mounting the
whole Secret as a directory) — required because the app expects exact file paths.

### Phase 4 — Deploy All 4 Microservices ✅
```
kubectl apply -f k8s/
kubectl get pods -n eshop
kubectl rollout restart deployment -n eshop
```
Verified: all 4 pods `Running`, `0` restarts. Full root-cause writeups for the two
blockers hit here are in [Section 3](#3-troubleshooting-battle-log).

### Phase 5 — Ingress ✅
```
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install ingress-nginx ingress-nginx/ingress-nginx -n ingress-nginx --create-namespace
kubectl get svc -n ingress-nginx ingress-nginx-controller -w      # wait for EXTERNAL-IP
kubectl apply -f k8s/ingress.yaml
kubectl get ingress -n eshop
```
**Public IP assigned:** `4.187.191.129` (Service `ingress-nginx-controller`,
`80:30382/TCP,443:31403/TCP`).

**Three fixes were required before traffic actually flowed:**

1. **Route prefixes corrected.** `ingress.yaml` originally routed `/api/catalog` →
   catalog-api, but no controller maps to that prefix (real routes are `/api/products`,
   `/api/categories`, `/api/reviews`). Would have 404'd every catalog request. Replaced the
   single prefix with the three real ones.
2. **`ingressClassName` instead of the deprecated annotation.** `kubectl get ingress` showed
   `CLASS: <none>` and an empty `ADDRESS` while using
   `annotations: kubernetes.io/ingress.class: nginx`. Modern controllers read
   `spec.ingressClassName`:
   ```yaml
   spec:
     ingressClassName: nginx    # replaces kubernetes.io/ingress.class annotation
   ```
   After this, `kubectl get ingress -n eshop` → `CLASS: nginx`, `ADDRESS: 4.187.191.129`.
3. **Azure LB health probe switched HTTP → TCP.** This was the real blocker — full
   writeup in [Incident 4](#-incident-4--ingress-public-ip-unreachable-azure-lb-health-probe-failing--resolved).

**Helm install (Windows) — winget was broken on this machine**
`winget install Helm.Helm` failed with an msstore/winget source error (also after
`winget source reset --force`). Installed manually instead:
```powershell
$helmVersion = "v3.16.3"; Invoke-WebRequest -Uri "https://get.helm.sh/helm-$helmVersion-windows-amd64.zip" -OutFile "$env:TEMP\helm.zip"; Expand-Archive -Path "$env:TEMP\helm.zip" -DestinationPath "C:\Tools\helm" -Force
```
PATH had to be set at **Machine** scope (not User) so both Windows accounts on the laptop
see it — read the existing Machine PATH rather than `$env:Path`, so user-scoped entries
don't get baked into the machine variable:
```powershell
[Environment]::SetEnvironmentVariable("Path", [Environment]::GetEnvironmentVariable("Path", "Machine") + ";C:\Tools\helm\windows-amd64", "Machine")
```
**Learned:** `helm repo add` is machine-global (stored in
`%APPDATA%\helm\repositories.yaml`), unrelated to the current working directory or project
folder. Also: kubeconfig is per-Windows-user (`C:\Users\<user>\.kube\config`) — running
kubectl/helm as a different account gives `connection refused to localhost:8080` until
`az aks get-credentials` is run for that account too.

**Verified publicly over the internet** (not port-forward):
```powershell
@("/api/products","/api/categories","/api/reviews","/api/customers","/api/orders","/api/auth/me") | ForEach-Object { $p = $_; try { $r = Invoke-WebRequest -Uri "http://4.187.191.129$p" -UseBasicParsing -TimeoutSec 15; "$p -> $($r.StatusCode)" } catch { "$p -> $($_.Exception.Response.StatusCode.value__)" } }
```
| Path | Result |
|---|---|
| `/api/products` | `200` (real JSON product data) |
| `/api/categories` | `200` |
| `/api/reviews` | `200` |
| `/api/customers` | `200` |
| `/api/orders` | `200` |
| `/api/auth/me` | `401` (correctly rejects tokenless request) |

**⚠️ Security finding surfaced by this phase:** `/api/customers` and `/api/orders` return
`200` with **no authentication** — customer and order data is now publicly readable by
anyone with the IP. This is pre-existing (missing `[Authorize]`), not caused by Ingress;
Ingress simply exposed it. Needs fixing before this stays public.

### Phase 6 — Final 🔄

**15.8.14 — HPA ✅ DONE**

Manifest: `k8s/catalog-api/hpa.yaml` (autoscaling/v2, min 1 / max 3, CPU target 70%).
metrics-server required no install — AKS ships it as a managed add-on in `kube-system`.

Load test (busybox pod, 5 parallel `wget` loops → `http://catalog-api/api/products`):
- Baseline: `2%/70%`, 1 replica
- Under load: `2% → 24% → 484%` → `SuccessfulRescale "New size: 3"`
- After load removed: held 3 replicas for the 300s stabilization window, then stepped `3→2→1` at 1 pod/min (rate policy)

⚠️ **Key finding — HPA scaled, but it did NOT help:**
Two of the three pods stayed `Pending` with `NODE <none>`. The single B2s node (2 vCPU, ~1.6 allocatable) was already full carrying SQL Server + 4 services + ingress-nginx. HPA counts pods it *asked* for, not pods the scheduler placed — it reported `3 current / 3 desired` and believed it had succeeded while the single running pod stayed at 484% CPU.

**HPA scales PODS. Cluster Autoscaler scales NODES. Production needs both.**

Also: `spec.replicas` removed from `catalog-api/deployment.yaml` — once an HPA owns a Deployment, the Deployment must not declare replica count or they fight on every `kubectl apply`.

**15.8.15 — React frontend → Azure Static Web Apps ✅ DONE**

Created SWA resource `swa-eshop` via Azure CLI. GitHub Actions workflow auto-generated:
`.github/workflows/azure-static-web-apps-proud-plant-008c4b200.yml`

Key challenges and solutions (details in Battle Log Incidents 5-7):
- Auth0 crash (React #527): conditional `Auth0Provider` + hook extraction → fixed
- React version mismatch: aligned `react` + `react-dom` to `19.2.7` → fixed
- Mixed Content (HTTP from HTTPS page): solved by adding HTTPS/TLS to Ingress (see 15.8.16)
- Build-time env vars: SWA "Application Settings" are runtime-only; Vite needs vars at
  build time → inject in GitHub Actions `env:` from GitHub Actions repository Variables
- CORS: added SWA origin to Azure App Configuration → rollout restart all pods
- App version in footer: `VITE_APP_VERSION` read from `package.json` via a Node step;
  `package.json` version bumped to `1.0.0`

**15.8.16 — HTTPS/TLS on AKS Ingress ✅ DONE**

Assigned DNS label to AKS public IP: `eshop-api.centralindia.cloudapp.azure.com`
```powershell
az network public-ip update \
  --name kubernetes-a931540c82fb94d31af3596519705b4a \
  --resource-group mc_rg-eshop-microservices_aks-eshop_centralindia \
  --dns-name eshop-api
```
Installed cert-manager:
```powershell
helm install cert-manager jetstack/cert-manager \
  --namespace cert-manager --create-namespace --set crds.enabled=true
```
Applied `k8s/cert-manager/cluster-issuer.yaml` (Let's Encrypt production, HTTP-01 solver).
Updated `k8s/ingress.yaml` with TLS block + `cert-manager.io/cluster-issuer: letsencrypt-prod`.
Certificate `READY: True` in ~27 seconds. HTTPS verified.

GitHub Variables updated:
- `VITE_API_CATALOG_URL` → `https://eshop-api.centralindia.cloudapp.azure.com`
- `VITE_API_CUSTOMER_URL` → `https://eshop-api.centralindia.cloudapp.azure.com`
- `VITE_API_ORDERING_URL` → `https://eshop-api.centralindia.cloudapp.azure.com`
- `VITE_API_IDENTITY_URL` → `https://eshop-api.centralindia.cloudapp.azure.com`

**15.8.17 — End-to-end test ✅ VERIFIED**

Login from SWA over HTTPS to AKS Identity API: ✅ SUCCESS
Dashboard loaded with real data from AKS catalog-api. JWT auth chain confirmed.

**15.8.18 — `az aks stop` ✅ Done**

Cluster stopped after E2E verified. Resume: `az aks start --name aks-eshop --resource-group rg-eshop-microservices`

---

## 3. Troubleshooting Battle Log

### 🔥 Incident 1 — ErrImagePull / ImagePullBackOff (all 4 pods) — ✅ Resolved
**Symptom:** `kubectl get pods -n eshop` → all 4 pods stuck in `ErrImagePull`.
**Root cause:** `az acr repository list --name acreshop2026` returned empty — Dockerfiles
existed but images had never actually been built/pushed for these services.
**Fix:** Committed `k8s/` changes on `develop` → PR → merged to `main` →
`build-and-push.yml` (GitHub Actions) built and pushed all 4 images to ACR tagged `:latest`.
**Verified:** `az acr repository list` showed all 4 repos; pods moved
`ContainerCreating` → `Running`.
**Also confirmed:** AKS kubelet identity already had the `AcrPull` role on `acreshop2026`
— no extra RBAC step was needed once images existed.

### 🔥 Incident 2 — CrashLoopBackOff on 3/4 services (stale images + CI path-filter bug) — ✅ Resolved
**Symptom:** After images existed, `catalog-api` came up fine but `customer-api`,
`identity-api`, and `ordering-api` went into `CrashLoopBackOff`.
**Root cause analysis:**
- `kubectl describe pod <pod>` showed liveness/readiness probes failing:
  `GET /health` → `404 Not Found`.
- A production fix had mapped health-check endpoints outside `IsDevelopment()` (needed so
  they respond in AKS's `Production` environment) — but that fix lived in the shared
  `ServiceDefaults` project, not in any single service's own folder.
- `build-and-push.yml`'s per-service `paths` filter only matched each service's own folder
  (e.g. `EShopMicroservices/Catalog.**`) and never accounted for the shared
  `ServiceDefaults`/`Contracts` projects that all 4 services reference.
- Net effect: only `catalog-api` (whose own folder was *also* touched in the same commit)
  got rebuilt. The other 3 silently kept running a stale image that still 404'd on `/health`.
**Fix:** Updated `.github/workflows/build-and-push.yml` so every service's `paths` regex
also matches the shared projects, e.g.:
```yaml
paths: 'EShopMicroservices/(Customer\.|EShopMicroservices\.ServiceDefaults|EShop\.Contracts)'
```
Merged `develop → main` → all 4 matrix jobs rebuilt → verified all 4 images tagged with the
same fresh commit SHA → `kubectl rollout restart deployment -n eshop`.
**Verified:** `kubectl get pods -n eshop` → all 4 `Running`, `0` restarts.
**Lesson:** any CI path-filter / monorepo build trigger MUST explicitly account for
shared/common projects — otherwise a shared-code fix can pass code review and CI green,
yet never actually reach the services that depend on it.

### 🔥 Incident 3 — SQL Server unreachable via `kubectl port-forward` + SSMS — ✅ Resolved
**Symptom 1:** SSMS error `1225` (connection actively refused).
**Root cause 1:** The `kubectl port-forward` terminal had been closed/interrupted — it's a
blocking foreground process; closing it silently kills the tunnel.
**Symptom 2 (after restarting the tunnel):** SSMS error `258` ("wait operation timed out")
— a TCP-provider-level timeout, not a login failure.
**Root cause 2:** Raw TCP connect succeeded (`Test-NetConnection` → `TcpTestSucceeded: True`),
but SQL Server's TDS/TLS handshake didn't reliably survive the proxy — a known
`kubectl port-forward` limitation with SQL Server's login/encryption negotiation.
**Fix:**
- Use `127.0.0.1,14330` instead of `localhost,14330` (IPv4 loopback resolved more reliably
  than the hostname over the tunnel).
- Reset "Database Name" back to `<default>` (a stale autocompleted value from an unrelated
  project was also contributing to failures).
- Enable "Trust Server Certificate".
**Verified:** SSMS connected successfully; ran `SELECT name FROM sys.databases` and queries
against `CatalogDb`.
**Reliable fallback** if this ever regresses (bypasses the tunnel entirely):
```
kubectl exec -it <sql-pod> -n eshop -- /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<password>" -C
```

### 🔥 Incident 4 — Ingress public IP unreachable (Azure LB health probe failing) — ✅ Resolved
**Symptom:** `ingress-nginx` installed cleanly and got public IP `4.187.191.129`, but every
request to it failed — not an HTTP error, no response at all:
```
Invoke-WebRequest http://4.187.191.129/api/products  → "Unable to connect to the remote server"
Test-NetConnection -ComputerName 4.187.191.129 -Port 80   → TcpTestSucceeded: False
Test-NetConnection -ComputerName 4.187.191.129 -Port 443  → TcpTestSucceeded: False
```

**Isolation steps (in order):**
| Check | Result | Conclusion |
|---|---|---|
| `Test-NetConnection google.com -Port 80` | `True` | Outbound port 80 works — not a local firewall |
| Same test over **phone mobile hotspot** | still `False` | Not the home network / not a corporate agent → **Azure-side** |
| `kubectl get pods -n ingress-nginx` | `1/1 Running` | Controller pod healthy |
| `az network public-ip list` | `Succeeded` | IP provisioned |
| `az network nsg rule list` | `Allow Tcp Inbound Internet → 4.187.191.129 : 443,80` | Firewall permits it |
| `az network lb rule list` | `80→80`, `443→443`, `EnableFloatingIP: True` | Forwarding rules exist |
| `az network lb address-pool show ... --query backendIPConfigurations` | node NIC present | Backend pool populated |
| `az network lb probe list` | `Protocol: Http`, `RequestPath: /`, ports `30382`/`31403` | ⬅ **the smoking gun** |

**Root cause:** The `ingress-nginx` Helm chart sets `appProtocol: http` on the controller
Service's port 80. Azure's cloud-controller-manager reads that and creates an **HTTP** health
probe with default `requestPath: /`. But NGINX only returns `200` for paths matching an
actual Ingress rule — `ingress.yaml` has no rule for `/`, so NGINX correctly answers **404**.
Azure's probe accepts only `2xx`; a `404` marks the node **unhealthy**, and a Standard Load
Balancer with zero healthy backends **silently discards inbound packets** rather than
rejecting them. Hence "unreachable" instead of a `502`/`503`.

**Why it was hard to diagnose:** every component reported healthy because every component
genuinely *was* healthy. The failure lived in the runtime conversation between two correctly
configured components (Azure asking `GET /`, NGINX answering `404`) — nothing in any
`ProvisioningState`/`Running` field can surface that. Config inspection alone will never
find this class of bug.

**Failed first attempt (worth recording):** pointed the HTTP probe at `/healthz` —
```
kubectl annotate service ingress-nginx-controller -n ingress-nginx service.beta.kubernetes.io/azure-load-balancer-health-probe-request-path=/healthz --overwrite
```
Azure reconciled the path, but traffic still failed. Reason: `ingress-nginx` serves
`/healthz` on its **dedicated healthz port 10254**, which is *not* exposed through the
Service — so `/healthz` on port 80 is also a `404`. Proven with an in-cluster curl:
```
kubectl run healthz-test --image=curlimages/curl:latest --rm -it --restart=Never -n ingress-nginx -- curl -s -o /dev/null -w "%{http_code}\n" http://ingress-nginx-controller/healthz
→ 404
```

**Fix — switch the probe from HTTP to TCP** (per-port Azure annotations):
```
kubectl annotate service ingress-nginx-controller -n ingress-nginx service.beta.kubernetes.io/port_80_health-probe_protocol=tcp service.beta.kubernetes.io/port_443_health-probe_protocol=tcp --overwrite
```
A TCP probe only asks "is the port open and accepting connections?" — no status code to trip
over. This is the correct probe type for an Ingress Controller precisely *because* its
response to any given URL depends entirely on user-defined routing rules, so no fixed URL is
a dependable health signal.

**Verified:**
```
az network lb probe list -g MC_rg-eshop-microservices_aks-eshop_centralindia --lb-name kubernetes --query "[].{name:name, port:port, protocol:protocol, path:requestPath}" -o table
→ Protocol: Tcp (both), path empty
Test-NetConnection -ComputerName 4.187.191.129 -Port 80 → TcpTestSucceeded: True
Invoke-WebRequest http://4.187.191.129/api/products → 200 + JSON product data
```

**⚠️ Persistence caveat:** this was applied with `kubectl annotate`, which edits the live
object. A future `helm upgrade ingress-nginx` will overwrite the Service and **wipe these
annotations**, silently breaking public access again. Make it permanent in Helm values:
```
helm upgrade ingress-nginx ingress-nginx/ingress-nginx -n ingress-nginx --reuse-values --set controller.service.annotations."service\.beta\.kubernetes\.io/port_80_health-probe_protocol"=tcp --set controller.service.annotations."service\.beta\.kubernetes\.io/port_443_health-probe_protocol"=tcp
```

**Lessons:**
- `PingSucceeded: False` against any Azure Load Balancer IP is **normal and meaningless** —
  Azure LBs don't answer ICMP. Only `TcpTestSucceeded` matters.
- A Standard Azure LB with no healthy backend **blackholes** traffic (looks like the host
  doesn't exist), which is easy to misread as a network/firewall/routing problem.
- Testing from a **second, independent network** (mobile hotspot) is the fastest way to split
  "my machine/network" from "the cloud side" — it eliminated an entire branch of theories in
  one step.
- For any LoadBalancer Service fronting a reverse proxy, prefer a **TCP** health probe unless
  you deliberately expose a path guaranteed to return `2xx`.

### 🔥 Incident 5 — React Auth0 crash on SWA (Error #527 / hook outside provider) — ✅ Resolved
**Symptom:** SWA deployment showed a white screen with React error #527:
`useAuth0 called outside Auth0Provider context`.
**Root cause:** `Auth0Provider` was always rendered unconditionally. When deployed to SWA,
the `VITE_AUTH0_*` environment variables were not present in the build (SWA Application
Settings are runtime-only; Vite needs them at build time). With `domain=""` and `clientId=""`
the provider initialization threw. Additionally, components like `LoginPage`, `ProfilePage`,
and `Auth0CallbackPage` called `useAuth0()` at the module level — React's hook rules
require that hooks only run inside their provider's render tree.

**Fix (two-part):**
1. Conditional provider rendering in `main.tsx`:
```tsx
const auth0Enabled = !!(domain && clientId && callbackUrl)
// render Auth0Provider only when all 3 vars are present
{auth0Enabled ? <Auth0Provider ...>{app}</Auth0Provider> : app}
```
2. Extracted `useAuth0()` calls into guarded inner components rendered only when
`auth0Enabled` is true — prevents the hook violation when the provider is absent.

**Lesson:** React hooks throw if called outside their context provider. For optional
providers (e.g., Auth0 only in certain environments), always guard both the provider
wrapper AND every hook call site. `Auth0Provider` with empty/missing config does not
fail silently.

---

### 🔥 Incident 6 — Mixed Content block (HTTP calls from HTTPS SWA) — ✅ Resolved
**Symptom:** After SWA deployed, browser console showed:
```
Mixed Content: The page at 'https://proud-plant-008c4b200.7.azurestaticapps.net'
was loaded over HTTPS, but requested an insecure resource 'http://4.187.191.129/api/auth/login'.
This request has been blocked; the content must be served over HTTPS.
```
No API calls reached the backend. Dashboard blank.

**Root cause:** Azure SWA forces HTTPS (cannot opt out). Browser security policy blocks
any HTTP (`http://`) fetch from an HTTPS page. Our AKS Ingress was HTTP-only on a raw IP.

**Fix:** Enable HTTPS on the AKS Ingress:
1. Assign Azure DNS label to public IP → `eshop-api.centralindia.cloudapp.azure.com`
2. Install cert-manager → Let's Encrypt ClusterIssuer (HTTP-01 challenge)
3. Update `ingress.yaml` with TLS block + `cert-manager.io/cluster-issuer` annotation
4. Update all `VITE_API_*_URL` GitHub Variables to the new `https://` URL

**Lesson:** A SPA hosted on HTTPS (SWA, Netlify, Vercel, GitHub Pages) CANNOT call any
`http://` endpoint — browsers block it without exception. No workaround exists for this
rule. The only fix is HTTPS on the backend. Treat TLS termination at the Ingress as
mandatory when combining a hosted SPA with a cloud-hosted API.

**Architecture note:** cert-manager + Let's Encrypt is free, automated, and renews
certificates every ~60 days (at 30 days remaining). There is no manual renewal process.
The HTTP-01 ACME challenge is solved by NGINX Ingress serving the challenge path
automatically — no extra configuration needed once the ClusterIssuer is applied.

---

### 🔥 Incident 7 — CORS 405/block (SWA origin not in allowed origins list) — ✅ Resolved
**Symptom:** After HTTPS was fixed, browser console showed:
```
Access to XMLHttpRequest at 'https://eshop-api.centralindia.cloudapp.azure.com/api/auth/login'
from origin 'https://proud-plant-008c4b200.7.azurestaticapps.net' has been blocked by CORS policy:
Response to preflight request doesn't pass access control check:
No 'Access-Control-Allow-Origin' header is present on the requested resource.
```

**Root cause:** ASP.NET Core CORS middleware reads `Cors:AllowedOrigins` from Azure App
Configuration at pod startup. The SWA origin had never been added, so all 4 pods had:
```
AllowedOrigins = ["http://localhost:5173", "http://localhost:5174"]
```
The SWA preflight (OPTIONS) request was rejected — no `Access-Control-Allow-Origin` header
returned → browser blocked the actual request.

**Fix:**
1. Added `Cors:AllowedOrigins:2 = https://proud-plant-008c4b200.7.azurestaticapps.net`
   to Azure App Configuration (`appconfig-eshop-dev`).
2. `kubectl rollout restart deployment -n eshop` — all 4 pods restarted and re-read
   App Config at startup, picking up the new origin.

**Critical detail — why restart was required:**
ASP.NET Core reads App Configuration ONCE at pod startup. It does NOT hot-reload CORS
config at runtime. Changing App Config without restarting pods has zero effect on running
pods. The pod must restart to re-read the config.

**CORS flow in this project:**
```
Browser sends OPTIONS preflight → NGINX Ingress → service pod
ASP.NET Core CORS middleware checks AllowedOrigins list (loaded at startup from App Config)
If origin found → responds with Access-Control-Allow-Origin header → browser sends real request
If origin missing → no header → browser blocks entire request
```

**Lesson:** When adding a new frontend origin (new environment, new SWA, localhost port),
always update `Cors:AllowedOrigins` in App Configuration AND restart the affected pods.
Changing App Config without a pod restart is the most common "CORS still broken after fix"
mistake in this stack.

---

**RS256 JWT signing chain (identity-api only):**
```
identity-pem-keys Secret (private.pem + public.pem)
  → volumeMount with subPath → /app/private.pem, /app/public.pem
  → JwtTokenService signs with private key, other services validate with public key
```

---

## 4. Command Reference

### Cluster lifecycle / cost management
```
az aks list --query "[].{name:name, resourceGroup:resourceGroup}" -o table
az aks stop  --name aks-eshop --resource-group rg-eshop-microservices
az aks start --name aks-eshop --resource-group rg-eshop-microservices
az aks get-credentials --resource-group rg-eshop-microservices --name aks-eshop
```

### Pods / Deployments
```
kubectl get pods -n eshop -o wide
kubectl get pods -n eshop -w                        # watch status live
kubectl describe pod <pod-name> -n eshop            # events, probe failures
kubectl logs -n eshop -l app=identity-api --tail=100
kubectl logs -n eshop <pod-name> --previous          # logs from before last crash
kubectl rollout restart deployment -n eshop          # pulls fresh :latest image
kubectl rollout status deployment/<name> -n eshop
kubectl exec -it <pod-name> -n eshop -- /bin/bash
```

### Services / networking
```
kubectl get svc -n eshop
kubectl port-forward -n eshop svc/sql-server 14330:1433
kubectl port-forward -n eshop svc/identity-api 8082:80
kubectl get ingress -n eshop
kubectl get ingressclass
```

### Ingress controller (Helm)
```
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm repo list
helm install ingress-nginx ingress-nginx/ingress-nginx -n ingress-nginx --create-namespace
helm list -n ingress-nginx
kubectl get pods -n ingress-nginx
kubectl get svc -n ingress-nginx ingress-nginx-controller -w        # wait for EXTERNAL-IP
```

### Azure LB / networking diagnostics (for Ingress issues)
```
# the AKS-managed "node" resource group holds the LB, NSG and public IP
az aks show --name aks-eshop --resource-group rg-eshop-microservices --query nodeResourceGroup -o tsv

$MC = "MC_rg-eshop-microservices_aks-eshop_centralindia"
az network public-ip list -g $MC -o table
az network nsg list -g $MC -o table
az network nsg rule list -g $MC --nsg-name <nsg-name> -o table
az network lb list -g $MC -o table
az network lb rule list -g $MC --lb-name kubernetes -o table
az network lb probe list -g $MC --lb-name kubernetes --query "[].{name:name, port:port, protocol:protocol, path:requestPath}" -o table
az network lb address-pool list -g $MC --lb-name kubernetes -o table
az network lb address-pool show -g $MC --lb-name kubernetes --name kubernetes --query "backendIPConfigurations" -o json
```

### Connectivity testing
```powershell
Test-NetConnection -ComputerName 4.187.191.129 -Port 80     # only TcpTestSucceeded matters
Invoke-WebRequest -Uri "http://4.187.191.129/api/products" -UseBasicParsing | Select-Object StatusCode, Content

# throwaway in-cluster curl pod (status code only) — proves what a Service actually returns
kubectl run healthz-test --image=curlimages/curl:latest --rm -it --restart=Never -n ingress-nginx -- curl -s -o /dev/null -w "%{http_code}\n" http://ingress-nginx-controller/healthz
```

### Config / Secrets
```
kubectl get configmap -n eshop
kubectl get secret -n eshop
kubectl create secret generic identity-pem-keys -n eshop --from-file=private.pem --from-file=public.pem
```

### ACR / images
```
az acr repository list --name acreshop2026
az acr repository show-tags --name acreshop2026 --repository eshop/identity-api
```

### Apply everything
```
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/
kubectl apply -f k8s/ingress.yaml
kubectl apply -f k8s/cert-manager/cluster-issuer.yaml
```

### DNS label for AKS public IP (free Azure hostname)
```powershell
# Find the public IP resource (in the MC_ node resource group)
az network public-ip list --query "[?ipAddress=='4.187.191.129'].{Name:name, ResourceGroup:resourceGroup}" -o table

# Assign DNS label → results in <label>.centralindia.cloudapp.azure.com
az network public-ip update `
  --name <ip-resource-name> `
  --resource-group mc_rg-eshop-microservices_aks-eshop_centralindia `
  --dns-name eshop-api

# Verify
az network public-ip show --name <ip-resource-name> `
  --resource-group mc_rg-eshop-microservices_aks-eshop_centralindia `
  --query "dnsSettings.fqdn" -o tsv
```

### cert-manager (TLS certificate automation)
```powershell
# Add Jetstack Helm repo
helm repo add jetstack https://charts.jetstack.io
helm repo update

# Install cert-manager with CRDs
helm install cert-manager jetstack/cert-manager `
  --namespace cert-manager --create-namespace --set crds.enabled=true

# Verify pods
kubectl get pods -n cert-manager

# Apply ClusterIssuer
kubectl apply -f k8s/cert-manager/cluster-issuer.yaml

# Check ClusterIssuer ready
kubectl get clusterissuer letsencrypt-prod

# Watch certificate issuance (after ingress TLS block applied)
kubectl get certificate -n eshop -w
kubectl describe certificate -n eshop   # shows ACME challenge events
```

### HTTPS / TLS verification
```powershell
# Verify certificate is serving
Invoke-WebRequest "https://eshop-api.centralindia.cloudapp.azure.com/api/products" -UseBasicParsing | Select-Object StatusCode
```

---

## 5. Architectural Decisions & Production Translation

### SQL Server as a pod (PVC) vs. Azure SQL
**Why:** Azure SQL bills per-second from creation — even with auto-pause, cost is
unavoidable. Running SQL Server as a pod means it stops billing entirely when
`az aks stop` deallocates the node; only the PVC's underlying Azure Disk (~₹8/mo) persists.
**Production reality:** Real organizations almost never self-host a primary OLTP database
in Kubernetes — they use a managed service (Azure SQL, Azure DB for PostgreSQL/MySQL) so
backups, HA, patching, and failover are handled by the platform. This is a deliberate
cost/learning trade-off here, not a production recommendation. The PVC/StorageClass/fsGroup
knowledge still transfers directly to any *stateful* workload an org does run in K8s
(Kafka, Elasticsearch, Redis, internal tools).

### Workload Identity (OIDC) vs. stored secrets/connection strings
**Why:** No ClientSecret or key is ever stored anywhere — the only "identity" is
Kubernetes' own ServiceAccount token, federated to an Azure AD Managed Identity.
**Production reality:** This is the current Microsoft-recommended pattern and matches what
real orgs running AKS + Key Vault do today (replacing older approaches like the deprecated
AAD Pod Identity, or storing SPN secrets in K8s Secrets directly). Directly transferable.

### Single-node cluster, single replica per service
**Why:** Cost — a second node or replica means paying for compute not needed for a
learning project.
**Production reality:** Real prod AKS clusters run multiple nodes (often across
availability zones) and multiple replicas per Deployment for HA; a single node/replica has
no fault tolerance. This is the clearest gap vs. a "real" cluster — understood
conceptually, not yet practiced hands-on here.

### Raw `kubectl apply` vs. Helm/GitOps
**Why:** The learning path deliberately does raw YAML first so the underlying objects are
fully understood before any templating/automation layer hides them.
**Production reality:** Most orgs wrap this in Helm charts (or Kustomize) and drive deploys
via GitOps (ArgoCD/Flux) instead of a human running `kubectl apply`. Both are on the
pending roadmap (Stages 15 and 18) — the YAML fluency gained here is a genuine advantage
when learning those tools, since they ultimately generate/apply the same underlying objects.

### ConfigMap + Secret + Workload Identity layering (not raw K8s Secrets alone)
**Why:** Plain K8s Secrets are only base64-encoded (not encrypted at rest by default) and
visible to anyone with pod/exec access — acceptable for the PEM-key volume mount (still
namespace-scoped and RBAC-controlled) but not for cloud credentials, which instead flow
through Workload Identity → Key Vault/App Config.
**Production reality:** Same layered pattern real orgs use — Secrets for cluster-local
material (certs, keys), Workload Identity/External Secrets Operator for pulling live
values from a cloud secret store.

### NGINX Ingress (single path-based entry point) vs. one LoadBalancer per service
**Why:** Each service getting its own `type: LoadBalancer` Service means paying for a
public IP per service. A single Ingress Controller = one public IP, routes by path prefix
(`/api/products`, `/api/auth`, etc.).
**Production reality:** Standard pattern; larger orgs sometimes use AGIC (Azure's
Application Gateway Ingress Controller) instead of community NGINX for tighter Azure
integration (WAF, Front Door), but the routing model is identical.
**Trade-off accepted here:** Initially HTTP-only on a bare IP. Full HTTPS+DNS added in
Stage 8 Phase 6 using cert-manager + Let's Encrypt. Production-grade: a DNS name, a
TLS cert managed by cert-manager, and automatic renewal.

### HTTPS/TLS via cert-manager + Let's Encrypt (vs. Azure Front Door / Azure App Gateway)
**Why:** cert-manager is free, open-source, and integrates natively with Kubernetes.
Let's Encrypt issues free, browser-trusted certificates. Automatic renewal via the ACME
protocol (HTTP-01 challenge handled by NGINX Ingress). Zero manual certificate work.
**Production reality:** cert-manager is the de facto standard for TLS in Kubernetes.
Used by thousands of production clusters. Renewal is fully automated. For enterprise
requirements (WAF, CDN, multi-region failover), Azure Front Door sits in front and
provides its own managed certificate — cert-manager still handles the AKS side.
**Why not Azure-managed certs here:** Azure App Service Managed Certificates only work
with App Service, not AKS. Azure Front Door managed certs need custom domain ownership.
The Azure-provided `*.cloudapp.azure.com` subdomain works perfectly with Let's Encrypt
HTTP-01 because Azure allows the DNS label assignment.

### Azure Static Web Apps (SWA) for frontend vs. Docker-in-AKS
**Why:** SWA is FREE (F1 tier), provides global CDN, GitHub Actions CI/CD out of the box,
and HTTPS by default. Running the React app as a container in AKS wastes ~128Mi memory on
the node that's already full.
**Production reality:** Hosting a React SPA on SWA, Vercel, Netlify, or an Azure Storage
static website + CDN is the universal industry pattern. SPAs have no server-side process —
they're just static files. A container brings no value for static content.
**Key constraint:** Vite env vars (`VITE_*`) are baked into the JS bundle at build time,
not at runtime. SWA "Application Settings" are runtime-only env vars and cannot be read by
Vite during `npm run build`. Industry solution: inject `VITE_*` in the GitHub Actions `env:`
block from repository Variables — change the URL in GitHub UI, trigger a redeploy. No code
PR needed.

### Frontend versioning (package.json → VITE_APP_VERSION → footer)
**Why:** The footer shows the deployed frontend version for user-facing traceability.
Each service (catalog-api, identity-api, etc.) deploys independently with its own version.
Showing backend versions in the user-facing footer is confusing and meaningless to users.
**Pattern:** `package.json` `"version"` field is the single source of truth for the frontend
version. A Node step in the GitHub Actions workflow reads it at build time and injects it as
`VITE_APP_VERSION`. Bump `package.json` → redeploy → footer updates. No workflow changes.
**Backend versions:** exposed via `/health` endpoints read by Kubernetes, Azure Monitor, and
Prometheus — never in the user-facing UI.

---

## 6. Skill Mapping (Interview Checklist)

| Skill | Status | Where |
|---|---|---|
| Pod / Node / Cluster / Control Plane concepts | ✅ | Stage 7 |
| Deployment / ReplicaSet / Service (ClusterIP) | ✅ | Stage 7 + all `deployment.yaml` |
| ConfigMap vs. Secret | ✅ | `catalog-api/configmap.yaml`, `sql-server/secret.yaml` |
| Namespace isolation | ✅ | `k8s/namespace.yaml` |
| Liveness / Readiness Probes (HTTP + TCP) | ✅ | All 4 service `deployment.yaml`, sql-server (`tcpSocket`) |
| Resource Requests + Limits | ✅ | All `deployment.yaml` |
| PersistentVolumeClaim + StorageClass (Azure Disk CSI) | ✅ | `k8s/sql-server/pvc.yaml`, `storageclass.yaml` |
| `fsGroup` / non-root container security context | ✅ | `sql-server/deployment.yaml` |
| ServiceAccount + Workload Identity (OIDC federation) | ✅ | `eshop-sa`, all 4 `deployment.yaml` |
| Secret volume mount via `subPath` (PEM keys) | ✅ | `identity-api/deployment.yaml` |
| kubectl debugging (describe, logs, exec, rollout) | ✅ | Battle Log incidents 1-3 |
| CI/CD → ACR → AKS image rollout pipeline | ✅ | `build-and-push.yml` + rollout restart |
| Ingress (path-based routing, NGINX) | ✅ | `k8s/ingress.yaml` live on `4.187.191.129`, all 6 paths verified publicly |
| `ingressClassName` vs. deprecated ingress.class annotation | ✅ | `k8s/ingress.yaml` |
| Azure LB health probes (HTTP vs. TCP) + LB/NSG diagnostics | ✅ | Battle Log Incident 4 |
| HPA (Horizontal Pod Autoscaler) | ✅ | `k8s/catalog-api/hpa.yaml` — verified scale 1→3 under load; key lesson: HPA + Cluster Autoscaler are different things |
| cert-manager + Let's Encrypt (TLS automation) | ✅ | Installed via Helm; ClusterIssuer (HTTP-01); certificate issued in ~27s; auto-renewal wired |
| TLS termination at Ingress | ✅ | `k8s/ingress.yaml` TLS block + `cert-manager.io/cluster-issuer` annotation; HTTPS verified end-to-end |
| Azure DNS label for AKS public IP | ✅ | Free Azure-provided hostname (`eshop-api.centralindia.cloudapp.azure.com`) via `az network public-ip update --dns-name` |
| Azure Static Web Apps (SWA) deployment | ✅ | GitHub Actions CI/CD, VITE_* vars injected at build time from GitHub Actions Variables, `staticwebapp.config.json` SPA fallback |
| Vite build-time env var injection (SWA pattern) | ✅ | GitHub Actions `env:` block → `${{ vars.VITE_API_URL }}` → baked into JS bundle; SWA App Settings are runtime-only and do NOT work for Vite |
| CORS in Kubernetes (App Config + pod restart) | ✅ | AllowedOrigins in Azure App Config; ASP.NET Core reads at startup only — config change requires pod restart to take effect |
| React Auth0 conditional provider pattern | ✅ | Guard `Auth0Provider` + all `useAuth0()` call sites behind env var check — hook violation if provider absent |
| Mixed Content (HTTPS → HTTP browser block) | ✅ | Diagnosed and resolved by adding HTTPS/TLS to Ingress; SPA on HTTPS cannot call HTTP APIs |
| Helm charts | 🟡 | Consumed public charts (`ingress-nginx`, `cert-manager`) + annotation/value overrides; not yet authored one (Stage 8f) |
| Service mesh (Istio, mTLS) | ⏳ | Pending (Stage 8c — moved up, AKS batch) |
| KEDA (event-driven autoscaling) | ⏳ | Pending (Stage 8d — moved up, AKS batch) |
| GitOps (ArgoCD) | ⏳ | Pending (Stage 8i — moved up, AKS batch) |
| Multi-node / multi-AZ HA | ⏳ | Not attempted (cost) |
| Multi-environment namespaces/clusters | ⏳ | Pending (Stage 19) |

**Interview talking points already earned from this project:**
- Diagnosed and fixed a real CI/CD monorepo path-filter bug causing silent stale deployments.
- Implemented passwordless Azure auth from AKS pods end-to-end (OIDC federation).
- Debugged a `CrashLoopBackOff` from probe failure back to root cause across two layers
  (Kubernetes → CI pipeline).
- Made a deliberate, justified cost-vs-architecture trade-off (SQL-in-pod) and can explain
  why it wouldn't hold in a production setting.
- Root-caused a silent Azure Load Balancer blackhole down to an HTTP health probe expecting
  `2xx` on `/` while NGINX Ingress correctly returned `404` — after every component
  (`ProvisioningState`, pod status, NSG rules, LB rules, backend pool) reported healthy.
  Can explain why config inspection alone can never surface that class of bug, and why a TCP
  probe is the right choice in front of a reverse proxy.
- Used a second independent network path (mobile hotspot) as a deliberate isolation technique
  to eliminate client/network causes before investigating cloud infrastructure.
- Set up end-to-end TLS: assigned an Azure DNS label to a public AKS IP, installed cert-manager
  via Helm, configured a Let's Encrypt ClusterIssuer (HTTP-01), and updated the Ingress TLS
  block — certificate issued automatically in ~27 seconds, auto-renews without manual work.
- Diagnosed and resolved Mixed Content blocking (HTTPS SPA → HTTP API): can explain the
  browser security rule, why no client-side workaround exists, and the correct solution
  (TLS termination at the Ingress).
- Deployed a React SPA to Azure Static Web Apps with build-time environment variable
  injection: can explain why SWA Application Settings are runtime-only and therefore cannot
  be used for Vite builds, and the correct industry pattern (GitHub Actions Variables →
  `env:` block → `VITE_*` baked into bundle).
- Debugged a React hook violation (#527) in a conditional provider scenario: diagnosed that
  `useAuth0()` throws when called outside `Auth0Provider`, and fixed it with conditional
  rendering + hook extraction into guarded inner components.

---

## 📌 Update Log
| Date | Update |
|---|---|
| Initial | File created — documents Stage 8 Phases 1-4 (cluster, SQL pod, Workload Identity, PEM mounting, all 4 services deployed + verified) at ~80% complete. Ingress, HPA, and frontend deploy still pending. |
| Phase 5 | **Ingress complete.** NGINX Ingress Controller installed via Helm (manual Helm install — winget broken; PATH set at Machine scope). Public IP `4.187.191.129`. Three fixes needed: corrected catalog route prefixes, switched to `spec.ingressClassName: nginx`, Azure LB health probe HTTP→TCP (Incident 4 — the actual blocker). All 6 paths verified over the public internet. Security gap found: `/api/customers` and `/api/orders` publicly readable with no auth. |
| Phase 5 security fix | **JWT auth added to customer-api + ordering-api.** RS256 `[Authorize]` + `public.pem` subPath mount in both deployments. Verified: `401` without token, `200` with valid bearer token. Closes the public-read gap. |
| Phase 6 (partial) | **HPA done.** `k8s/catalog-api/hpa.yaml` — autoscaling/v2, 1→3 pods, 70% CPU. Verified scale-up to 3 under load (484% utilization). Key finding: 2 of 3 pods stayed `Pending` — single B2s node has no spare capacity. HPA scales pods, Cluster Autoscaler scales nodes. `spec.replicas` removed from deployment. React SWA + E2E test still pending. |
| Phase 6 (complete) — Stage 8 DONE ✅ | **React SWA deployed + HTTPS/TLS + E2E verified.** SWA: `https://proud-plant-008c4b200.7.azurestaticapps.net`. Incidents 5 (Auth0 #527), 6 (Mixed Content), 7 (CORS) all diagnosed and resolved. cert-manager + Let's Encrypt TLS on Ingress. Hostname: `eshop-api.centralindia.cloudapp.azure.com`. GitHub Variables pattern for build-time VITE_* injection established. App version from package.json. Login end-to-end over HTTPS: ✅. AKS cluster stopped. Stage 8 = **100% complete**. |

> **Stage 8 is fully closed. Next: Stage 8b (APIM) when cluster is restarted.**
>
> **Open follow-ups carried into future stages:**
> 1. Persist the TCP health-probe annotations in ingress-nginx Helm values so `helm upgrade`
>    can't wipe them (see Incident 4 persistence caveat) — ideally done in Stage 8f (Helm).
> 2. **Revised stage order:** all AKS stages (8b→8j: APIM, Istio, KEDA, Observability, Helm,
>    DNS/SSL, Load Testing, GitOps, Multi-env) grouped before Container Apps (Stage 9).
>    Saves ~₹15,000+ vs original interleaved order.
> 3. Full purchase flow E2E (Cart → Checkout → Order History) — deferred; login confirmed ✅.
