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

## 📍 CURRENT STAGE — Stage 13: Docker + Containerization

### Where We Stopped
```
Completed docker-compose.yml with SQL Server + EShop API services

Completed so far in Stage 13:
  ✅ Module 1 — Docker Fundamentals
       → What is Docker, why it exists
       → VM vs Container
       → Docker Architecture (Client/Daemon/Registry)
       → docker run hello-world (first container!)
       → Containers are lightweight vs VMs

  ✅ Module 2 — Images & Containers
       → Image vs Container (cookie cutter analogy)
       → Docker Hub (pulled hello-world)
       → docker pull aspnet:10.0 and sdk:10.0
       → docker images, docker ps, docker ps -a
       → docker rm, docker rmi, docker container prune
       → Container lifecycle (Created→Running→Stopped→Removed)
       → Image layers explained
       → aspnet:10.0 = 340MB (runtime only)
       → sdk:10.0 = 1.26GB (full build tools)
       → Why multi-stage builds save ~1GB!

  ✅ Module 3 — Dockerfile
       → What is a Dockerfile
       → All instructions: FROM, WORKDIR, COPY, RUN,
         ENV, ARG, EXPOSE, ENTRYPOINT, CMD
       → Layer ordering and caching
       → .dockerignore file created
       → Single stage vs Multi-stage Dockerfile
       → Written multi-stage Dockerfile for EShop API
       → Stage 1: Build with SDK image
       → Stage 2: Runtime with aspnet image
       → Built image: eshop-api:v1

  ✅ Module 4 — Running Containers
       → docker run with environment variables (-e flag)
       → Port mapping (-p 8080:80)
       → host.docker.internal explained
       → docker logs, docker rm -f
       → SQL Server sa account setup (TCP/IP enabled)
       → Container connected to SQL Server successfully!
       → Fixed DB migration (db.Database.Migrate())
       → Fixed Swagger in Production mode


  ✅ Module 5 — Volumes and Data
       → What are Docker volumes
       → Named volumes vs bind mounts
       → sqlserver_data volume created
       → /var/opt/mssql = SQL Server data path inside container
       → Data persists even after container deleted!
       → Defined in volumes: section of compose

  ✅ Module 6 — Networking
       → What is Docker networking
       → Bridge network driver explained
       → eshop-network created
       → Containers on same network talk by service name!
       → sqlserver hostname resolves automatically!
       → Outside world accesses only via exposed ports!

  ✅ Module 7 — Docker Compose
       → What is docker-compose and why we need it
       → Written docker-compose.yml
       → SQL Server service with healthcheck + volume
       → EShop API service with depends_on
       → Docker networking (eshop-network)
       → Docker volumes (sqlserver_data - data persists!)
       → Server=sqlserver (service name as hostname!)
       → depends_on with service_healthy condition
       → version, services, volumes, networks explained

  ✅ Module 9 — Best Practices
       → Non-root user (groupadd + useradd + USER)
       → Security: COPY with --chown flag
       → .dockerignore optimized (IDE, OS, Docker files)
       → Health checks added (Dockerfile + docker-compose)
       → /health endpoint in Program.cs (AddDbContextCheck)
       → curl installed for health checks
       → .env file for secrets (not in Git!)
       → .env.example template for team
       → docker-compose uses env variables
       → Docker image labels (OCI standard)
       → Layer caching optimized (csproj copied first)
       → Multi-stage build verified (saves ~1GB)
```

### Docker Modules Plan
```
✅ Module 1 → Docker Fundamentals
✅ Module 2 → Images & Containers
✅ Module 3 → Dockerfile
✅ Module 4 → Running Containers
✅ Module 5 → Volumes & Data
✅ Module 6 → Networking
✅ Module 7 → Docker Compose
⏳ Module 8 → Registry (Azure Container Registry)
✅ Module 9 → Best Practices
```

### What We Will Build in Stage 13
```
✅ .dockerignore file
✅ Multi-stage Dockerfile for EShop.API
✅ docker-compose.yml (API + SQL Server + Volumes + Network)
✅ Health checks (Dockerfile + compose)
✅ Docker Volumes (data persistence)
✅ Docker Networking (bridge network)
✅ .env file for compose
✅ Non-root user (security)
✅ Docker image labels (OCI standard)
✅ Layer caching optimized
⏳ Azure Container Registry (ACR)
⏳ Push image to ACR
```

---

## ✅ Stage 14 — CI/CD Pipeline (GitHub Actions)

### Completed
```
✅ build-and-test.yml workflow created
✅ Triggers: push to develop + PR to develop/main
✅ Steps: checkout → setup .NET 10 → restore → build → test
✅ Branch protection rules on develop + main
✅ Pipeline verified GREEN ✅
✅ PR: feature/cicd-pipeline → develop → main merged
```

---

## 🗺️ Azure Master Plan — Complete Roadmap (39 Topics)

### 📚 Phase 1 — Foundation
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 1 | Azure Account + Portal + CLI | 🟢 Free | ⏳ |
| 2 | Resource Groups + Naming + Tagging + Cost Management | 🟢 Free | ⏳ |

### 🔒 Phase 2 — Security First
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 3 | Azure Key Vault | 🟢 Free | ⏳ |
| 4 | Managed Identity | 🟢 Free | ⏳ |
| 5 | Azure RBAC | 🟢 Free | ⏳ |
| 6 | Azure Defender for Cloud | 🟢 Free | ⏳ |

### 🌐 Phase 3 — Networking
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 7 | Azure Virtual Network (VNet) | 🟢 Free | ⏳ |
| 8 | Azure Private Endpoints | 🔴 Delete! | ⏳ |
| 9 | Azure Application Gateway (WAF + SSL) | 🔴 Delete! | ⏳ |
| 10 | Azure DNS | 🟡 $0.50 | ⏳ |

### 🗄️ Phase 4 — Storage
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 11 | Azure Blob Storage | 🟢 Free | ⏳ |
| 12 | Azure CDN | 🟢 Free | ⏳ |

### ⚡ Phase 5 — Serverless & Automation
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 13 | Azure Functions | 🟢 Free | ⏳ |
| 14 | Azure Logic Apps | 🟢 Free | ⏳ |
| 15 | Azure Runbooks | 🟢 Free | ⏳ |

### 📦 Phase 6 — Containers in Cloud
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 16 | Azure Container Registry (ACR) | 🟡 $5 | ⏳ |
| 17 | Azure App Service (learn first) | 🟢 Free | ⏳ |
| 18 | Azure Deployment Slots (staging → prod swap, zero downtime) | 🟢 Free | ⏳ |
| 19 | Azure Container Apps (migrate from App Service) | 🟡 $1-3 | ⏳ |
| 20 | CD Pipeline (deploy-to-azure.yml) | 🟢 Free | ⏳ |

### 🔀 Phase 7 — Microservices + Databases
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 20 | Product Service + Azure SQL DB | 🟢 Free | ⏳ |
| 21 | Order Service + Azure SQL DB | 🟢 Free | ⏳ |
| 22 | Customer Service + Azure SQL DB | 🟢 Free | ⏳ |
| 23 | Azure SQL Elastic Pool | 🔴 Delete! | ⏳ |
| 24 | Azure Cosmos DB | 🟢 Free | ⏳ |

### 🔍 Phase 8 — Search & AI
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 25 | Azure Cognitive Search | 🟢 Free | ⏳ |
| 26 | Azure OpenAI (product recommendations) | 🟡 $1-2 | ⏳ |

### 📡 Phase 9 — Messaging & Events
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 27 | Azure Service Bus | 🟢 Free | ⏳ |
| 28 | Azure Event Grid | 🟢 Free | ⏳ |
| 29 | Azure Queue Storage | 🟢 Free | ⏳ |
| 30 | Azure Redis Cache | 🔴 Delete! | ⏳ |

### 🚪 Phase 10 — API Gateway
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 31 | Ocelot API Gateway (.NET) | 🟢 Free | ⏳ |
| 32 | Azure API Management (APIM) | 🟢 Free | ⏳ |

### 📊 Phase 11 — Observability
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 33 | Application Insights | 🟢 Free | ⏳ |
| 34 | Azure Monitor + Alerts | 🟢 Free | ⏳ |
| 35 | Log Analytics Workspace | 🟢 Free | ⏳ |
| 36 | Azure Load Testing | 🟢 Free | ⏳ |

### 🔑 Phase 12 — Identity
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 37 | Azure AD B2C | 🟢 Free | ⏳ |

### 🌍 Phase 13 — Architect Level
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 38 | Azure Front Door | 🔴 Delete! | ⏳ |
| 39 | Azure App Configuration | 🟢 Free | ⏳ |
| 40 | .NET Aspire | 🟢 Free | ⏳ |

```
Total Topics   →  40
🟢 Free        →  30 topics (75%)
🟡 Cheap       →   5 topics (~$11/month)
🔴 Delete!     →   5 topics (create → learn → delete same session)
─────────────────────────────────────────────
Monthly Cost   →  ~$11/month
$200 Credit    →  18+ months 🚀
```

---

## 🔜 Stage Progress Summary

| Stage | Topic | Status |
|-------|-------|--------|
| 13 | Docker + Containerization | ✅ Done |
| 14 | CI/CD Pipeline (GitHub Actions) | ✅ Done |
| 15 | Azure Foundation + Security + Networking | ⏳ Next |
| 16 | Storage + Serverless + Containers in Cloud | ⏳ |
| 17 | Microservices Split + Databases | ⏳ |
| 18 | Search + AI + Messaging + Events | ⏳ |
| 19 | API Gateway + Observability | ⏳ |
| 20 | Identity + Architect Level | ⏳ |

---

## 🏢 Real Organization Approach
```
Environments: DEV → STAGING → PROD
Workflow: feature branch → PR → CI checks → merge → auto deploy
Azure: Personal free account ($200 credit + free tier)
Tools: GitHub Actions, Azure CLI, Docker Compose
```

---

## 🛠️ Tools Installed & Verified
```
✅ .NET 10 SDK
✅ Visual Studio 2026 / VS Code
✅ SQL Server 2025 (local)
✅ Git + GitHub account (vikasgage28-dev)
✅ Docker Compose (via docker-compose.yml)
❌ Docker Desktop (uninstalled - office policy)
⏳ Azure CLI (needed for Stage 15)
⏳ Azure Account (needed for Stage 15)
```

---

## 💡 Key Decisions Made
```
→ Using .NET 10 LTS (not 9)
→ Docker images: aspnet:10.0 and sdk:10.0
→ Multi-stage Dockerfile (build with SDK, run with aspnet)
→ Personal Azure free account for ALL cloud resources
→ GitFlow branching (feature/xxx → develop → main)
→ One feature branch per stage
→ Learning style: Concept first → Analogy → Implement → Verify
→ Docker Desktop removed - office policy - using CI/CD for builds
→ SQL Server 2025 (image: mssql/server:2025-latest)
→ sa account enabled with Password123!
→ db.Database.Migrate() added in Program.cs for auto DB creation
→ Swagger enabled in all environments (not just Development)
→ Non-root user (appuser) runs container for security
→ /health endpoint added with AddDbContextCheck
→ .env file used for secrets (not committed to Git)
→ .env.example template shared with team
→ Docker image labels follow OCI standard
→ Module 8 (ACR) deferred to Stage 15 (Azure)
→ CI pipeline (GitHub Actions) - build-and-test.yml
→ Branch protection rules on develop + main
→ Pipeline must pass before PR can be merged
→ Environments: Local = DEV, Azure = PROD
→ Deployment Slots for staging concept (zero extra cost!)
→ No separate staging environment (cost saving for learning)
```

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
