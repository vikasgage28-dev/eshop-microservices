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
Currently in Module 3 — Dockerfile (about to start)

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

  🔜 Module 3 — Dockerfile (NEXT TO START)
       → What is a Dockerfile
       → All instructions: FROM, WORKDIR, COPY, RUN,
         ENV, ARG, EXPOSE, ENTRYPOINT, CMD
       → Layer ordering and caching
       → .dockerignore file
       → Single stage vs Multi-stage
       → Write EShop API Dockerfile!
```

### Docker Modules Plan
```
✅ Module 1 → Docker Fundamentals
✅ Module 2 → Images & Containers
🔜 Module 3 → Dockerfile (NEXT!)
⏳ Module 4 → Running Containers
⏳ Module 5 → Volumes & Data
⏳ Module 6 → Networking
⏳ Module 7 → Docker Compose
⏳ Module 8 → Registry (Azure Container Registry)
⏳ Module 9 → Best Practices
```

### What We Will Build in Stage 13
```
→ .dockerignore file
→ Multi-stage Dockerfile for EShop.API
→ docker-compose.yml (API + SQL Server + Volumes + Network)
→ .env file for compose
→ Health checks
→ Azure Container Registry (ACR)
→ Push image to ACR
→ Non-root user (security)
→ Optimized image size (~150MB target)
```

---

## 🔜 Upcoming Stages

| Stage | Topic |
|-------|-------|
| 13 | 🔄 Docker + Containerization (IN PROGRESS) |
| 14 | ⏳ CI/CD Pipeline (GitHub Actions) |
| 15 | ⏳ Azure Deployment + Key Vault + Managed Identity |
| 16 | ⏳ Split to Microservices (Product/Order/Customer) |
| 17 | ⏳ API Gateway (Ocelot + Rate Limiting) |
| 18 | ⏳ Service Communication (Azure Service Bus) |
| 19 | ⏳ .NET Aspire + Azure Container Apps |

---

## 🏢 Real Organization Approach
```
Environments: DEV → STAGING → PROD
Workflow: feature branch → PR → CI checks → merge → auto deploy
Azure: Personal free account ($200 credit + free tier)
Tools: GitHub Actions, Azure CLI, Docker Desktop
```

---

## 🛠️ Tools Installed & Verified
```
✅ .NET 10 SDK
✅ Visual Studio 2026 / VS Code
✅ SQL Server (local)
✅ Git + GitHub account (vikasgage28-dev)
✅ Docker Desktop v4.69.0 (WSL2 backend)
✅ Docker Engine v29.4.0
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
→ Following eshop-learning-plan.html as base roadmap
→ Extending features as needed beyond the plan
```

---

## 📋 Instructions for AI Assistant
```
1. Read this file completely before responding
2. DO NOT re-explain completed stages
3. Continue from "Where We Stopped" section
4. Always explain concept BEFORE implementing
5. Use simple analogies (developer is a beginner)
6. Announce clearly when moving to next Module/Stage
7. Go step by step, wait for user confirmation
8. Update this file at end of session
9. Commit updates to GitHub
10. User is learning Docker from SCRATCH
    Be patient and thorough with explanations
```
