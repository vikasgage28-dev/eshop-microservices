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
| 8 | AKS deployment — all 4 services in K8s | 🟡 ~₹2,500/mo | 🔄 **NEXT** |
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

> Cost while studying ~₹3,000/mo; ~₹440/mo when AKS node stopped.
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

### 🔄 IMMEDIATE NEXT — Stage 8: AKS Deployment

**Stage 8** — AKS cluster provisioning + deploy all 4 services live!

---

## 🎯 Where We Stopped / Next Action
- Phase 15 Stages 1-7 ✅ fully complete (Docker → Azure data → secrets → ACR → CI/CD → K8s concepts + raw YAML).
- CI/CD enhanced with path-based filtering — only changed services rebuild on push ✅
- 15 raw K8s YAML files written in `k8s/` folder (namespace + 4×configmap/service/deployment + ingress) ✅
- **Next:** Stage 8 — Create AKS cluster + `kubectl apply` our YAML files + first live deployment!
- Azure SQL DBs deleted to stop vCore charges; recreate at Stage 8 with cost-safe flags:
  `--min-capacity 0.5 --auto-pause-delay 60 --max-size 1GB --compute-model Serverless`.

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
