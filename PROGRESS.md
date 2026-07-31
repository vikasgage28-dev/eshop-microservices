# 🚀 EShop Learning Project — Session Progress Tracker

> **HOW TO USE THIS FILE:**
> At the start of every new AI session, paste this entire file content.
> The AI will instantly know everything and continue from exactly where we left off.
> At the end of every session, update this file and commit to GitHub.

---

## 👤 Project Owner
- **Developer:** Vikas Gage (beginner learning cloud development)
- **GitHub:** https://github.com/vikasgage28-dev/eshop-microservices
- **Local Path:** `C:\WORK\Learning\eshop-microservices`
- **Azure:** Personal FREE account (NOT company resources)
- **Goal:** Learn full stack cloud development like a real organization

---

## 🏗️ Project Overview
Building an **EShop API** from monolith to microservices — exactly like a real organization.
- Framework: **.NET 10 LTS**
- Architecture: **Clean Architecture**
- Pattern: **CQRS + MediatR**
- Auth: **JWT Authentication**
- DB: **SQL Server + EF Core 10**
- Testing: **xUnit + Moq + FluentAssertions**
- Logging: **Serilog**
- Validation: **FluentValidation**

---

## 📁 Solution Structure
```
eshop-microservices/
├── EShop.API/              ← Web API, Controllers, Middleware, Validators
├── EShop.Core/             ← Entities, Interfaces, CQRS Features
├── EShop.Infrastructure/   ← EF Core, Repositories, Services, Migrations
├── EShop.Shared/           ← DTOs, ApiResponse, PagedResult
├── EShop.Tests/            ← xUnit + Moq unit tests
├── .github/workflows/      ← CI/CD pipelines
├── PROGRESS.md             ← THIS FILE
```

---

## 🌿 Git Branching Strategy
- `main` → Production | `develop` → Development | `feature/xxx` → Features
- PR workflow: feature → develop → main
- Branch protection: PRs required, pipeline must pass

---

## ✅ Completed Stages (1-12)

| Stage | Topic | Status |
|-------|-------|--------|
| 1 | GitHub Setup + Branching Strategy | ✅ Done |
| 2 | Clean Architecture (API/Core/Infrastructure/Shared) | ✅ Done |
| 3 | EF Core + SQL Server + Migrations | ✅ Done |
| 4 | FluentValidation + Global Error Handling | ✅ Done |
| 5 | JWT Authentication (Register/Login/Authorize) | ✅ Done |
| 6 | Serilog Logging (Console + File sinks) | ✅ Done |
| 7 | CQRS + MediatR (Commands/Queries/Handlers) | ✅ Done |
| 8 | Unit Tests (xUnit + Moq + FluentAssertions) | ✅ Done |
| 9 | Pagination + Filtering (PagedResult) | ✅ Done |
| 10 | API Versioning (v1/v2) | ✅ Done |
| 11 | CORS (environment-based origins) | ✅ Done |
| 12 | User Secrets (local dev secrets) | ✅ Done |

---

## 📍 CURRENT STAGE — Phase 15: Cloud Deployment (AKS + Full Production Stack) — IN PROGRESS
> Phase 14 Authentication Deep Dive is COMPLETE. Starting Phase 15 — Stage 1: Dockerize.

### Key Credentials (never changes)
```
Admin:    admin@eshop.com         / Admin@12345
Customer: vikasgage28@gmail.com   / Customer@12345  (also used for Gmail SMTP + Google OAuth)
Currency: INR (₹) — en-IN locale
UI Theme: Lenovo Vantage Blue (#0067c0), 165px Sidebar
Frontend: http://localhost:5173 (Vite dev server)
Aspire:   https://localhost:17222 (dashboard)
Ports:    catalog=5010, customer=5011/5022(gRPC), ordering=5012, identity=5013
JWT:      RS256, 60-min expiry, private.pem signs, public.pem verifies
2FA:      Email OTP — 2-min expiry — vikasgage28@gmail.com receives codes
Gmail:    SMTP App Password in User Secrets (EmailSettings:AppPassword)
Auth0:    dev-p6qgjp2d5mvexwg7.us.auth0.com — Google social login enabled
```

---

## 📚 Detailed History & Full Roadmap → see `Learnings.md`
> **Progress.md** = lean **active session context** (where we are + what's next).
> **Learnings.md** = **technical bookshelf** (deep per-phase logs, gRPC/Aspire details,
> Azure Master Plan tables, Phase 15 per-stage deep-dives, full AI/ML 175-skill roadmap,
> production issues, key decisions).
> At session start paste **Progress.md**. Open **Learnings.md** only when deep detail is needed.

---

## ✅ Completed Work — Compact Summary

**Monolith (Azure Phases 1-11)** ✅ — Clean Arch + CQRS + EF Core + JWT + Serilog + xUnit,
then Azure: App Service, Key Vault, Blob, Functions, Logic Apps, Runbooks, Service Bus +
Welcome Email, Ocelot + APIM gateway, App Insights. *(full logs → Learnings.md)*

**Phase 12 — Microservices Split** ✅
- 12.1 Catalog · 12.2 Ordering · 12.3 Customer · 12.4 Identity (Clean Arch + CQRS each)
- 12.5 .NET Aspire orchestration (ServiceDefaults + AppHost + dashboard)
- 12.6 Service-to-service: HTTP typed client + async messaging (3 swappable publishers)
- 12.7 gRPC Customer lookup (Protobuf/HTTP2, dedicated h2c port 5022)

**Phase 13 — React 19 Frontend** ✅
- Vite 8 + TS + Tailwind v4 + Shadcn/ui + Redux Toolkit + RTK Query + Axios JWT interceptor
- Pages: Login/Register, Products, Cart, Checkout, Orders, Customers, Dashboard, Profile
- Dark mode, custom hooks, address management, Lenovo Vantage UI

**Phase 14 — Auth Deep Dive** 🔄 (in progress)
- ✅ Silent token refresh · refresh-token rotation · JWT RS256 · 2FA email OTP
- ✅ OAuth 2.0 + PKCE (Auth0) + Google & GitHub social login
- ⏳ Remaining: OIDC, client-credentials, TOTP app, magic links, passkeys, SAML, mTLS, etc.

---

## 📍 CURRENT PHASE — 15: Cloud Deployment (AKS + Full Production Stack)

| Stage | Topic | Cost | Status |
|-------|-------|------|--------|
| 1 | Dockerize — multi-stage Dockerfiles + docker-compose | 🟢 | ✅ |
| 2 | Clean Azure slate — rg-eshop-microservices | 🟢 | ✅ |
| 3 | Data layer — SQL x4 + Cosmos + Storage (blob + queues) | 🟢 | ✅ |
| 4 | Secrets + central config — Key Vault + App Config | 🟢 | ✅ |
| 5 | Container Registry — ACR (acreshop2026) | 🟡 ~₹420/mo | ✅ |
| 6 | CI/CD pipelines + Trivy + CodeQL + versioning + path-based filters | 🟢 | ✅ |
| 7 | **Kubernetes Concepts** (pure learning, no cluster) | 🟢 | ✅ |
| 8 | AKS deployment — all 4 services + SQL Server pod in K8s | 🟡 ~₹748/mo | 🔄 **IN PROGRESS (~90% — Ingress live, JWT auth added; HPA + frontend remain)** |
| 8b | APIM — optional enterprise layer in front of NGINX | 🟢 Consumption=FREE | ⏳ |
| 9 | Azure Container Apps (same app, simpler platform) | 🟢 | ⏳ |
| 10 | Entra ID — "Login with Microsoft" for admins | 🟢 | ⏳ |
| 11 | Azure AD B2C — consumer identity | 🟢 | ⏳ |
| 12 | Istio service mesh — mTLS zero-trust | 🟢 | ⏳ |
| 13 | Workload Identity + KEDA | 🟢 | ⏳ |
| 14 | Observability — App Insights + Log Analytics | 🟢 | ⏳ |
| 15 | Helm charts | 🟢 | ⏳ |
| 16 | DNS + SSL + Azure Front Door | 🟡 ~₹40/mo | ⏳ |
| 17 | Azure Load Testing | 🟢 | ⏳ |
| 18 | GitOps — ArgoCD | 🟢 | ⏳ |
| 19 | Multi-environment DEV → STAGING → PROD | 🟢 | ⏳ |

> Cost while studying ~₹748/mo (no Azure SQL — SQL runs inside AKS pod!); ~₹500/mo when AKS node stopped (ACR + OS disk only).
> Full per-stage deep-dive notes + completed Stage 1-6 LEARNED logs → `Learnings.md`.

---

### ✅ Stage 7: Kubernetes Concepts — COMPLETE

| # | What | Status |
|---|------|--------|
| 15.7.1 | Pod, Node, Cluster, Control Plane | ✅ |
| 15.7.2 | Deployment + ReplicaSet + Service (ClusterIP vs LoadBalancer) | ✅ |
| 15.7.3 | ConfigMap + Secret + why raw K8s Secrets aren't secure alone | ✅ |
| 15.7.4 | Ingress — path routing, NGINX, TLS termination, APIM option | ✅ |
| 15.7.5 | Namespace, Init Container, Resource Requests + Limits | ✅ |
| 15.7.6 | Helm — Chart.yaml, values.yaml, templates/, install/upgrade | ✅ |
| 15.7.7 | Write all K8s YAML manually first (before Helm) | ✅ |

---

### 🔄 Stage 8: AKS Deployment — IN PROGRESS (~90% — all 4 services live + public Ingress + JWT auth; HPA/frontend remaining)

**Stage 8** — AKS cluster provisioning + SQL Server pod + deploy all 4 services live!

**Key architectural decision:** SQL Server runs as a pod inside AKS (not Azure SQL).
- Azure SQL bills per second from creation → unavoidable cost even with auto-pause
- SQL Server pod = stops with AKS node = ₹0 when not studying ✅
- Data persisted on Azure Disk via PVC (survives pod restarts and node stops) ✅
- Teaches bonus K8s concepts: PVC, StatefulSet pattern, pod-to-pod DNS ✅

**Files created in `k8s/sql-server/`:** `storageclass.yaml`, `pvc.yaml`, `secret.yaml`, `deployment.yaml`, `service.yaml` ✅ (all applied to cluster, untracked in git — pending commit)

**Stage 8 Full Sequence:**
```
Phase 1 — AKS Setup                                                         ✅ DONE
  15.8.1 → Create AKS cluster (1 node B2s, Standard HDD)                    ✅
  15.8.2 → Get credentials + connect kubectl                                ✅
  15.8.3 → Create eshop namespace                                          ✅

Phase 2 — SQL Server in AKS                                                 ✅ DONE
  15.8.4 → Create StorageClass + PVC (Azure Disk — 32GB HDD) → Bound        ✅
  15.8.5 → Create SQL Server Secret (SA password)                          ✅
  15.8.6 → Create SQL Server Deployment + Service                          ✅
           (fixed volume permission bug with fsGroup: 10001)
  15.8.7 → Verify SQL Server pod running + sqlcmd login confirmed          ✅

Phase 3 — Configuration Update                                              ✅ DONE
  15.8.8 → Update Key Vault connection strings (x4 DBs)
           → Server=sql-server,1433 instead of Azure SQL URL               ✅

Phase 3b — AKS Workload Identity (passwordless Azure auth for pods)         ✅ DONE
  15.8.8a → Enable OIDC issuer + Workload Identity on cluster              ✅
  15.8.8b → Create Managed Identity (id-eshop-workload)                    ✅
  15.8.8c → Grant RBAC: Key Vault Secrets User + App Config Data Reader    ✅
  15.8.8d → Federated credential (eshop-sa ↔ id-eshop-workload)            ✅
  15.8.8e → Create + annotate + label K8s ServiceAccount (eshop-sa)        ✅
  15.8.8f → Wire eshop-sa + label into all 4 deployment.yaml               ✅
  15.8.8g → Add AppConfig__Endpoint to all 4 configmap.yaml                ✅
  15.8.8h → Create identity-pem-keys Secret + mount into identity-api      ✅

Phase 4 — Deploy Microservices                                             ✅ DONE
  15.8.9  → kubectl apply all YAML files                                   ✅ Applied (all objects created)
  15.8.10 → Verify all pods running                                        ✅ All 4 pods Running, 0 restarts
            Original blocker: ACR had zero images (never built+pushed) →
            fixed by committing k8s/ changes + merging develop→main.
            Second blocker found after images existed: customer-api,
            identity-api, ordering-api stuck in CrashLoopBackOff — probes
            hit GET /health → 404 because those 3 services were still
            running STALE images (only catalog-api got rebuilt).
            Root cause: CI path-filter regex only matched each service's
            OWN folder, never accounted for shared ServiceDefaults/Contracts
            projects that ALL 4 services reference. So when the /health
            production fix landed in ServiceDefaults, only catalog-api
            (which also had its own folder touched) got rebuilt.
            Fix: updated `.github/workflows/build-and-push.yml` — each
            service's `paths` filter now also matches
            EShopMicroservices.ServiceDefaults/** (and EShop.Contracts/**
            where relevant). Committed on develop → merged develop→main →
            all 4 matrix jobs rebuilt → verified all images tagged with
            latest commit (61a89d3) → `kubectl rollout restart` → all pods
            Running, 0 restarts.
            AKS kubelet AcrPull role → confirmed working (images pulled
            successfully, no ImagePullBackOff).
  15.8.11 → Test via kubectl port-forward (free!)                          ✅ Verified
            SQL Server: port-forward svc/sql-server 14330:1433 → connected
            via SSMS (127.0.0.1,14330 + Trust Server Certificate) — kubectl
            exec + sqlcmd also confirmed as reliable fallback.
            Identity API: port-forward svc/identity-api 8082:80 → logs show
            clean startup (EF migrations applied, seeder ran, RSA/PEM keys
            loaded without error) → POST /api/auth/login with seeded
            admin@eshop.com/Admin@12345 → returned valid RS256 JWT +
            refresh token + Admin role → end-to-end auth chain confirmed.

Phase 5 — Ingress                                                           ✅ DONE
  15.8.12 → Install NGINX Ingress Controller (Helm)                         ✅
            winget install of Helm was broken → installed binary manually,
            PATH set at Machine scope.
  15.8.13 → Apply ingress.yaml + test public IP                             ✅
            Public IP: 4.187.191.129 — all 6 paths verified over the
            public internet. Three fixes were required:
            (a) catalog route prefixes corrected to /api/products,
                /api/categories, /api/reviews (was /api/catalog);
            (b) switched from the deprecated kubernetes.io/ingress.class
                annotation to spec.ingressClassName: nginx;
            (c) THE REAL BLOCKER — Azure LB health probe was HTTP on "/",
                which NGINX answers 404 (no rule for "/") → Azure removed
                the node from rotation → silent blackhole (TCP connect
                itself failed, not an HTTP error). Fixed by switching the
                probe to TCP via Service annotations:
                service.beta.kubernetes.io/port_80_health-probe_protocol=tcp
                (and the same for port_443). Full post-mortem = Incident 4
                in KUBERNETES_AKS_MASTER_LOG.md.
  15.8.13a → Security gap found + fixed: JWT auth on customer/ordering      ✅ DEPLOYED + VERIFIED
            Public testing revealed /api/customers and /api/orders returned
            200 with NO auth — customer PII and order data world-readable.
            Root cause: the documented "gateway owns auth" pattern assumed a
            validating gateway in front; NGINX Ingress does no JWT
            validation, so exposing services publicly left them wide open.
            Fix (mirrors Identity.API): RS256 JwtBearer validation using
            public.pem in both Customer.API and Ordering.API +
            UseAuthentication/UseAuthorization + [Authorize] on
            CustomersController and OrdersController + public.pem mounted
            from the identity-pem-keys Secret via subPath in both
            deployment.yaml files.
            VERIFIED in-cluster: both pods start clean (no RSA/PEM errors,
            EF migrations applied, Kestrel listening) → /api/customers and
            /api/orders return 401 with no token, and 200 with a valid
            bearer token from admin@eshop.com. Confirms the full RS256
            chain across service boundaries: Identity.API signs with
            private.pem, Customer/Ordering independently verify with
            public.pem — no shared secret, no callback to Identity.
            Also cleaned up stale Error/ContainerStatusUnknown pods left
            behind by the earlier az aks stop (node deallocated mid-flight;
            ReplicaSets had already scheduled healthy replacements).

Phase 6 — Final
  15.8.14 → Add HPA                                                        ✅ VERIFIED (with an important caveat)
            k8s/catalog-api/hpa.yaml — autoscaling/v2, min 1 / max 3,
            CPU target 70% (= 70m, since catalog-api requests 100m —
            HPA scales against the REQUEST, not the limit).
            metrics-server needed no install: AKS ships it as a managed
            add-on in kube-system.
            Load test (busybox pod, 5 parallel wget loops against
            http://catalog-api/api/products): 2% → 24% → 484% →
            SuccessfulRescale "New size: 3". Killing the load returned it
            to 1 after the 300s scale-down stabilization window, stepping
            3→2→1 at 1 pod/min.
            ⚠️ CAVEAT — the more valuable finding: only 1 of the 3 pods
            ever ran. The other two sat Pending with NODE <none>, because
            the single B2s node (2 vCPU, ~1.6 allocatable) was already
            full. HPA reported "3 current / 3 desired" — it counts what it
            asked for, not what the scheduler placed — so it believed it
            had succeeded while CPU stayed pegged at 484%.
            HPA scales PODS and assumes capacity exists; Cluster
            Autoscaler scales NODES. Production needs both. Not enabling
            CA here — a second node roughly doubles compute spend.
  15.8.15 → Deploy React frontend (Azure SWA)                              ⏳
  15.8.16 → End-to-end test                                                ⏳
  15.8.17 → Stop cluster (az aks stop)                                     ✅ Stopped again after verifying login end-to-end (this session)
```

---

## 🎯 Where We Stopped / Next Action
- Phase 15 Stages 1-7 ✅ fully complete (Docker → Azure data → secrets → ACR → CI/CD → K8s concepts + raw YAML).
- Stage 8 Phases 1-4 ✅ complete: AKS cluster, SQL-in-pod (with PVC), Key Vault updated, full Workload Identity chain wired into all 4 deployments, PEM keys mounted into identity-api, all 4 microservices deployed and verified Running with 0 restarts.
- **CI/CD bug found + fixed:** `build-and-push.yml` path filters only matched each service's own folder, so shared `ServiceDefaults`/`Contracts` changes silently skipped rebuilding `customer-api`, `identity-api`, `ordering-api`. This meant a production fix (health checks mapped outside `IsDevelopment()`) never reached those 3 images → `/health` probe returned 404 → `CrashLoopBackOff`. Fixed by adding shared-project paths to each service's matrix filter; merged `develop → main`; all 4 images rebuilt and pushed to ACR tagged with commit `61a89d3`.
- **All 4 pods confirmed Running, 0 restarts** after `kubectl rollout restart` pulled the fresh images.
- **SQL Server connectivity solved:** `kubectl port-forward svc/sql-server 14330:1433` → SSMS via `127.0.0.1,14330` with "Trust Server Certificate" enabled (`localhost` didn't work reliably over the tunnel, `127.0.0.1` did). `kubectl exec` + `sqlcmd` remains the reliable fallback if the TDS handshake over port-forward misbehaves.
- **Identity API fully verified end-to-end:** logs show clean startup, EF migrations applied, seeder ran, RSA/PEM keys loaded without error. `POST /api/auth/login` with seeded `admin@eshop.com` / `Admin@12345` returned a valid RS256-signed JWT + refresh token + `Admin` role — confirms `private.pem`/`public.pem` volume mounts and `JwtTokenService` are working correctly in AKS.
- **Seeded credentials (from `IdentityDataSeeder.cs`):** Admin → `admin@eshop.com` / `Admin@12345`; Customer → `alice@eshop.com` / `Customer@12345`.
- **AKS cluster STOPPED again** (`az aks stop`) after verification, to save cost during the break. Resume with `az aks start --name aks-eshop --resource-group rg-eshop-microservices`.
- **Phase 5 (Ingress) ✅ complete:** NGINX Ingress Controller live on public IP `4.187.191.129`, path-based routing to all 4 services, all 6 paths verified from the public internet. Blocker was the Azure LB HTTP health probe on `/` returning 404 → node dropped from rotation → traffic silently blackholed at TCP level. Fixed with TCP probe annotations on the controller Service (Incident 4 in `KUBERNETES_AKS_MASTER_LOG.md`).
- **Security gap found + FIXED + VERIFIED:** public testing showed `/api/customers` and `/api/orders` returned 200 with no token. Added RS256 JWT validation (`public.pem`) + `[Authorize]` to Customer.API and Ordering.API, mirroring Identity.API; `public.pem` mounted from the `identity-pem-keys` Secret via `subPath` in both deployments. Deployed and confirmed in-cluster: **401 without a token, 200 with a valid bearer token.** **This reverses the earlier "gateway owns auth, services stay open" decision** — see the amended note in `Learnings.md`.
- **SQL Strategy confirmed:** Azure SQL abandoned. SQL Server runs as pod inside AKS. Data persisted via PVC on Azure Disk (~₹8/mo). Stops with AKS node = ₹0 idle cost ✅
- **Connection strings** in Key Vault updated to: `Server=sql-server,1433;Database=<DbName>;...` (K8s Service DNS) ✅
- **HPA (15.8.14) ✅ verified:** `k8s/catalog-api/hpa.yaml` (autoscaling/v2, 1→3 pods, 70% CPU). Load test drove utilization to **484%** → scaled to 3 → returned to 1 after the 300s stabilization window. **Caveat that matters more than the success:** 2 of the 3 pods stayed `Pending` — the single B2s node had no spare CPU, so scaling changed nothing. HPA scales pods and assumes capacity; **Cluster Autoscaler** scales nodes. Details in `Learnings.md`.
- **Next action:** (a) remove `replicas: 1` from `k8s/catalog-api/deployment.yaml` — it now conflicts with the HPA (a later `kubectl apply` resets to 1, HPA scales back up, they fight); (b) persist the TCP health-probe annotations in the ingress-nginx Helm values (a plain `helm upgrade` will otherwise wipe them and re-blackhole the public IP); then 15.8.15 React frontend to Azure SWA and 15.8.16 end-to-end test. `az aks stop` when pausing.

---

## 📋 Instructions for AI Assistant
```
1. Read this file completely before responding
2. DO NOT re-explain completed stages
3. Continue from "Where We Stopped" section
4. Always explain concept BEFORE implementing
5. Developer has 11+ years .NET experience - no basic explanations!
6. Announce clearly when moving to next Module/Stage
7. Go step by step, wait for user confirmation
8. Update this file at end of every session
9. Commit updates to GitHub before ending session
10. Developer: 11+ years .NET, AZ-900, AZ-204 certified
    Azure: SQL, Key Vault, Storage, Blobs, Logic Apps
    Goal: Principal Cloud Architect
11. User creates feature branch BEFORE any coding starts
12. User does implementation, AI guides with explanation
13. User is NOT a beginner - treat as experienced developer!
```
