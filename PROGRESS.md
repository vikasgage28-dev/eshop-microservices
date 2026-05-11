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

## 📍 CURRENT STAGE — Stage 23: Azure Phase 9 (API Gateway)

### Where We Stopped
```
Ocelot API Gateway DONE! Deployed to production! Moving to APIM next!

Phase 9 — Ocelot API Gateway completed:
✅ EShop.Gateway project created (ASP.NET Core Empty, .NET 10)
✅ Ocelot NuGet package installed (v24.1.0)
✅ ocelot.json configured with routes:
   → /gateway/auth/{everything}    → EShop.API /api/auth/{everything}
   → /gateway/v1/products          → EShop.API /api/v1/products
   → /gateway/v1/products/{id}     → EShop.API /api/v1/products/{id}
   → /gateway/v2/products          → EShop.API /api/v2/products
   → /gateway/v1/products/{id}/reviews → EShop.API reviews
   → /gateway/health               → EShop.API /health
✅ Rate Limiting configured:
   → Auth routes   → 5 req/sec
   → Product routes → 10 req/sec
   → 429 Too Many Requests with custom message!
   → ClientId header based rate limiting!
✅ Program.cs configured with Ocelot middleware
✅ Tested locally:
   → GET /gateway/v1/products → 200 OK ✅
   → POST /gateway/auth/login → JWT token ✅
   → POST /gateway/v1/products (no token) → 401 ✅
   → POST /gateway/v1/products (with token) → 201 ✅
   → Rate limit exceeded → 429 ✅
✅ Deployed to Azure via GitHub Actions pipeline!

Key Lessons:
→ Upstream = what client sends to Gateway
→ Downstream = what Gateway forwards to API
→ Rate limiting needs ClientId header to identify client!
→ Period "1s" resets every second (use "1m" for manual testing!)
→ Gateway is a PROXY — auth still handled by downstream API!

Previously completed:
Service Bus + Welcome Email DONE! End-to-end tested and verified!

Phase 8 — Service Bus + Welcome Email completed:
✅ Azure Service Bus Namespace created (sb-eshop-prod, Basic tier, Central India)
✅ Queue created: welcome.email.queue
✅ Service Bus Connection String stored in Key Vault (ServiceBusConnection)
✅ Azure Communication Services created (acs-eshop-prod, Asia Pacific)
✅ Email Communication Services created (ecs-eshop-prod, Asia Pacific)
   → Lesson: Data location MUST match acs! (India failed, recreated Asia Pacific!)
✅ Azure Subdomain provisioned: ee9fee22-f283-43f8-8deb-32020e03c868.azurecomm.net
   → SPF + DKIM + DKIM2 all Verified automatically!
✅ Domain connected to acs-eshop-prod successfully!
✅ ACS Connection String stored in Key Vault (AcsConnection)

Code Changes:
✅ EShop.Shared/Messages/WelcomeEmailMessage.cs created
   → Email + UserName properties (shared between API and Functions!)
✅ AuthController.cs updated:
   → ServiceBusClient injected via DI
   → After registration → publishes WelcomeEmailMessage to Service Bus
   → try/catch → registration never fails because of email!
✅ Program.cs (EShop.API) updated:
   → ServiceBusClient registered as Singleton
   → Reads ServiceBusConnection from config
✅ WelcomeEmailFunction.cs created:
   → ServiceBusTrigger on "welcome.email.queue"
   → Deserializes JSON → WelcomeEmailMessage
   → Sends Welcome Email via ACS EmailClient (Plain text + HTML body)
✅ Program.cs (EShop.Functions) updated:
   → EmailClient registered as Singleton
   → Reads AcsConnection from config
✅ EShop.Shared reference added to EShop.Functions project
✅ NuGet packages:
   → EShop.API: Azure.Messaging.ServiceBus
   → EShop.Functions: Azure.Communication.Email
   → EShop.Functions: Microsoft.Azure.Functions.Worker.Extensions.ServiceBus

App Settings added:
✅ func-eshop-prod: ServiceBusConnection → Key Vault ✅
✅ func-eshop-prod: AcsConnection → Key Vault ✅
✅ func-eshop-prod: AcsSenderAddress → DoNotReply@ee9fee22-...azurecomm.net
✅ app-eshop-prod: ServiceBusConnection → Key Vault ✅

End-to-End Test:
✅ Registered user via Swagger → JWT returned immediately!
✅ WelcomeEmailFunction triggered automatically!
✅ Welcome Email received in Outlook! 📧
✅ Service Bus Explorer tested — sent JSON message directly from portal!

Key Lessons:
→ Azure subdomain = FREE, instant SPF/DKIM, goes to Junk (learning only!)
→ Custom domain = real company, goes to Inbox, needs DNS verification
→ Data location of ecs MUST match acs — different region = cannot connect!
→ Service Bus Explorer → can send test messages directly from portal!
→ try/catch around Service Bus → never block user registration!
→ Email sent async → user gets JWT immediately, no waiting!

Previously completed:
Azure Runbooks DONE! Night runbook scheduled and tested successfully!

Phase 7 — Azure Runbooks completed:
✅ Azure Automation Account created (automation-eshop-prod, rg-eshop-shared)
✅ System-assigned Managed Identity enabled on Automation Account
✅ Contributor role assigned to Managed Identity on rg-eshop-prod
✅ runbook-eshop-night (PowerShell 7.2) created and published:
   → Checks App Service Plan SKU (B1 or F1?)
   → Downgrades B1 → F1 if needed (cost saving!)
   → Stops App Service (app-eshop-prod)
   → Fixed bug: wrong plan name "plan-eshop-prod" → "asp-eshop-prod"
✅ schedule-eshop-night created:
   → Runs every day at 10:00 PM IST (India Standard Time)
   → Linked to runbook-eshop-night ✅
✅ Tested manually — runbook ran successfully:
   → Detected B1, downgraded to F1, stopped app service!
✅ Morning strategy: Manual start from portal (no morning runbook needed!)

Phase 7 — Azure Logic Apps completed:
✅ Azure Logic App created (Consumption plan)
✅ Recurrence trigger (every 1 hour)
✅ HTTP action calling /health endpoint (app-eshop-prod.azurewebsites.net/health)
✅ Condition checking status code = 200
✅ Email alert sent when API is DOWN
✅ True/False branches configured for failure scenarios
✅ End-to-end tested and verified!

Full Working Flow:
   Every 1 hour
   → HTTP GET app-eshop-prod.azurewebsites.net/health
   → Status = 200?
      TRUE  → API Healthy (do nothing)
      FALSE → API DOWN! → Send Email Alert!

Key Lessons:
   → Real world: production teams use this for 24/7 monitoring!
   → Instant alerts when service goes down
   → No manual checking needed
   → Logic Apps used by companies worldwide!

Azure Functions DONE! Two functions deployed and working in production!

Phase 7 — Azure Functions completed:
✅ EShop.Functions project created (dotnet-isolated, .NET 10, V4)
✅ HealthCheck Function (HTTP Trigger)
   → Route: GET /api/health
   → Returns: {"status":"healthy","service":"EShop.Functions","timestamp":"..."}
   → Fixed: WriteStringAsync (async I/O required in isolated worker!)
   → Fixed: Removed UseAzureMonitorExporter (not needed without App Insights)
   → Working in production: https://func-eshop-prod.azurewebsites.net/api/health ✅

✅ InventorySummary Function (Timer Trigger)
   → Schedule: 0 0 0 * * * (runs every midnight!)
   → Reads Products table from Azure SQL DB
   → Logs category summary (count + total stock per category)
   → Logs LOW STOCK ALERT for products with Stock < 5
   → Working in production! ✅

✅ Azure Function App (func-eshop-prod, Consumption plan, FREE!)
   → Managed Identity enabled
   → Key Vault reference for SQL connection string (ConnectionStrings__DefaultConnection)
   → CORS enabled for portal.azure.com (for Test/Run in portal)
   → SCM Basic Auth enabled (for publish profile deployment)

✅ CI/CD Pipeline updated (build-and-push.yml)
   → Added "Deploy Azure Functions" job
   → Uses Publish Profile (not Service Principal)
   → ZIP Deploy via Kudu SCM endpoint
   → All 3 jobs passing: Build+Push → Deploy App Service → Deploy Functions

✅ Key Lessons Learned:
   → Async I/O required in isolated worker (WriteStringAsync not WriteString!)
   → Timer Trigger 202 Accepted = success (async, no response body!)
   → Invocations tab needs Application Insights to show data
   → Use az webapp log tail for real-time function logs
   → Referencing EShop.Infrastructure = tight coupling (ok in monolith, fix in microservices!)
   → local.settings.json = User Secrets for Functions (never committed to Git!)
   → Connection strings tab in portal for DB connections (not App settings tab!)

Completed Azure Phase 1-6! App is LIVE with Cosmos DB product reviews!

Phase 6 FULLY DONE:
✅ Azure SQL Server + Database (EShopDb, Free tier, Central India)
✅ Key Vault SqlConnectionString updated → Azure SQL
✅ DB migrations + seeding run automatically on startup
✅ Azure Cosmos DB account (Free tier) + EShopDb database + reviews container
   Partition key: /productId | 400 RU/s
   CosmosDbConnection secret stored in Key Vault
✅ Product Reviews feature implemented end-to-end:
   → Review entity (Newtonsoft.Json [JsonProperty("id")])
   → IReviewRepository interface (Clean Architecture)
   → ReviewRepository (Microsoft.Azure.Cosmos SDK, fully qualified Container)
   → ReviewsController (GET/POST/DELETE — api/v1/products/{productId}/reviews)
   → CosmosClient registered as Singleton in Program.cs
   → Reads config as "CosmosDb:ConnectionString" ?? "CosmosDb__ConnectionString"
✅ CosmosDb__ConnectionString added to App Service via Portal (not CLI!)
   → Key Vault reference resolves to green ✅ in portal
   → PowerShell @ escaping issue: use Portal UI or backtick-escape @ in CLI
✅ Correct Managed Identity (2ecb0851) assigned Key Vault Secrets User role
✅ Reviews endpoint working in production!
   GET  https://app-eshop-prod.azurewebsites.net/api/v1/products/1/reviews → 200 ✅
   POST https://app-eshop-prod.azurewebsites.net/api/v1/products/1/reviews → 200 ✅

Completed so far in Stage 13 (Docker):
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
✅ Module 8 → Registry (Azure Container Registry) — done in Stage 18, Phase 5
✅ Module 9 → Best Practices
```

### What We Built in Stage 13
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
✅ Azure Container Registry (ACR) — covered in Phase 5 Hosting
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

## 🗺️ Azure Master Plan — Complete Roadmap (Sequential)

> **Order matters!** Foundation → Security → Networking → Hosting → Data → Messaging → Observability → Advanced

---

### 📚 Phase 1 — Foundation
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 1 | Azure Account + Portal + CLI | 🟢 Free | ✅ Done |
| 2 | Resource Groups + Naming + Tagging + Cost Management | 🟢 Free | ✅ Done |

---

### 🔒 Phase 2 — Security (Identity, secrets and access — before anything else!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 3 | Azure AD / Entra ID + Service Principal | 🟢 Free | ✅ Done |
| 4 | Azure Key Vault (secrets storage) | 🟢 Free | ✅ Done |
| 5 | Azure RBAC (who can access what) | 🟢 Free | ✅ Done |
| 6 | Azure Defender for Cloud | 🟢 Free | ✅ Done |

---

### 🌐 Phase 3 — Networking (BEFORE hosting — app needs a network to live in!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 7 | Azure Virtual Network (VNet) + Subnets | 🟢 Free | ✅ Done |
| 8 | Network Security Groups (NSG) | 🟢 Free | ✅ Done |
| 9 | Azure Private Endpoints | 🔴 Delete! | ✅ Done (created → learned → deleted) |
| 10 | Azure Application Gateway (WAF + SSL) | 🔴 Delete! | ✅ Done (created → learned → deleted) |
| 11 | Azure DNS | 🟡 $0.50 | ✅ Moved to Phase 5 (needs real IP first!) |

---

### 📦 Phase 4 — Storage (independent of hosting — learn early!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 12 | Azure Blob Storage | 🟢 Free | ✅ Done (steshopprod + product-images container) |
| 13 | Azure CDN | 🟢 Free | ✅ Replaced by Azure Front Door in Phase 13 (CDN Classic deprecated!) |

---

### 🚀 Phase 5 — Hosting (network exists → now host the app!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 14 | Azure Container Registry (ACR) — skipped, using Docker Hub instead | 🟡 $5 | ✅ Using Docker Hub (FREE!) |
| 15 | Docker Hub setup + build-and-push.yml pipeline | 🟢 Free | ✅ Done |
| 16 | Azure App Service Plan (asp-eshop-prod, B1, Linux) | 🟢 Free | ✅ Done |
| 17 | Azure App Service (app-eshop-prod) pulling from Docker Hub | 🟢 Free | ✅ Done |
| 18 | Managed Identity + Key Vault access (RBAC role assigned) | 🟢 Free | ✅ Done |
| 19 | App Settings + Key Vault References | 🟢 Free | ✅ Done |
| 20 | Azure Deployment Slots (Blue/Green, zero downtime swap) | 🟢 Free | ✅ Concept learned (S1 needed for impl, too costly!) |
| 21 | Azure Container Apps (migrate from App Service) | 🟡 $1-3 | ⏳ |
| 22 | CD Pipeline — auto deploy to Azure App Service | 🟢 Free | ✅ Done (deploy job added to build-and-push.yml!) |
| 23 | Azure DNS — point api.eshop.com to real Public IP | 🟡 $0.50 | ⏳ |

---

### 🗄️ Phase 6 — Databases
> App needs DB to start! Created Azure SQL BEFORE finishing Phase 5 hosting.
> ⚠️ Plan Decision: OrderDb + CustomerDb created AFTER microservices split (no code uses them yet!)
> Smart approach: Learn Elastic Pool concept + add Cosmos DB with real feature (product reviews)!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 24 | Azure SQL Server + Database (EShopDb — monolith) | 🟢 Free | ✅ Done |
| 25 | Update Key Vault SqlConnectionString → Azure SQL | 🟢 Free | ✅ Done |
| 26 | Verify app starts + health endpoint works | 🟢 Free | ✅ Done |
| 27 | Azure SQL Elastic Pool (learn concept → delete!) | 🔴 Delete! | ✅ Done (learned concept, skipped provisioning) |
| 28 | Azure Cosmos DB (free tier — product reviews feature!) | 🟢 Free | ✅ Done |
| 29 | Add /reviews endpoint to monolith using Cosmos DB | 🟢 Free | ✅ Done |
| 30 | Azure SQL Database (Order Service) | 🟢 Free | ⏳ After microservices split! |
| 31 | Azure SQL Database (Customer Service) | 🟢 Free | ⏳ After microservices split! |

---

### ⚡ Phase 7 — Serverless & Automation
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 26 | Azure Functions (HTTP + Timer Trigger, Managed Identity, Key Vault, CI/CD) | 🟢 Free | ✅ Done |
| 27 | Azure Logic Apps | 🟢 Free | ✅ Done |
| 28 | Azure Runbooks (automation) | 🟢 Free | ✅ Done |

---

### 📡 Phase 8 — Messaging & Events (microservices need to talk!)

> 🎯 Welcome Email scenario: User registers → Service Bus → Azure Function Queue Trigger → ACS email!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 29 | Azure Service Bus (async messaging) + Welcome Email (Queue Trigger) | 🟢 Free | ✅ Done |
| 30 | Azure Event Grid (event-driven) | 🟢 Free | ⏳ |
| 31 | Azure Queue Storage | 🟢 Free | ⏳ |
| 32 | Azure Redis Cache | 🔴 Delete! | ⏳ |

---

### 🚪 Phase 9 — API Gateway (route all traffic through one door!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 33 | Ocelot API Gateway (.NET) | 🟢 Free | ✅ Done |
| 34 | Azure API Management (APIM) | 🟢 Free | ⏳ |

---

### 📊 Phase 10 — Observability (app is live → now monitor it!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 35 | Log Analytics Workspace | 🟢 Free | ⏳ |
| 36 | Application Insights | 🟢 Free | ⏳ |
| 37 | Azure Monitor + Alerts | 🟢 Free | ⏳ |
| 38 | Azure Load Testing | 🟢 Free | ⏳ |

---

### 🔍 Phase 11 — Search & AI
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 39 | Azure Cognitive Search | 🟢 Free | ⏳ |
| 40 | Azure OpenAI (product recommendations) | 🟡 $1-2 | ⏳ |

---

### 🔑 Phase 12 — Identity
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 41 | Azure AD B2C | 🟢 Free | ⏳ |

---

### 🌍 Phase 13 — Architect Level
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 42 | Azure Front Door (global load balancing) | 🔴 Delete! | ⏳ |
| 43 | Azure App Configuration (centralized config) | 🟢 Free | ⏳ |
| 44 | .NET Aspire (cloud-native orchestration) | 🟢 Free | ⏳ |

---

### ⭐ Microservices Split — BEFORE Phases 14, 15, 16!
> Learn Terraform + AKS ONCE on real microservices! Not twice!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 45 | Design microservices boundaries (Catalog, Order, Customer, Identity) | 🟢 Free | ⏳ |
| 46 | Split monolith → Catalog Service (.NET) | 🟢 Free | ⏳ |
| 47 | Split monolith → Order Service (.NET) | 🟢 Free | ⏳ |
| 48 | Split monolith → Customer Service (.NET) | 🟢 Free | ⏳ |
| 49 | Split monolith → Identity Service (.NET) | 🟢 Free | ⏳ |
| 50 | Docker Compose for ALL microservices locally | 🟢 Free | ⏳ |
| 51 | Service-to-service communication (HTTP + Service Bus) | 🟢 Free | ⏳ |

---

### 🏗️ Phase 14 — Infrastructure as Code (Terraform)
> Learn ONCE on real microservices infrastructure! Not on monolith!
> Real world: Nobody clicks portal! Everything is code!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 52 | Terraform Fundamentals (providers, state, plan, apply) | 🟢 Free | ⏳ |
| 53 | Terraform modules for each microservice (SQL, KV, ACR) | 🟢 Free | ⏳ |
| 54 | Terraform — AKS cluster + networking + RBAC | 🟢 Free | ⏳ |
| 55 | Terraform — Full EShop microservices infra in one command! | 🟢 Free | ⏳ |
| 56 | Terraform remote state (Azure Blob Storage backend) | 🟢 Free | ⏳ |
| 57 | Terraform workspaces (DEV / STAGING / PROD configs) | 🟢 Free | ⏳ |

---

### 🌎 Phase 15 — Multiple Environments (DEV / STAGING / PROD)
> Real world: Always 3 environments! Never deploy direct to PROD!
> ⚠️ Cost Decision: NOT creating separate Azure environments (too expensive!)
> Smart approach: Learn concepts + simulate using FREE tools!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 58 | Environment strategy concept (DEV → STAGING → PROD) | 🟢 Free | ⏳ |
| 59 | Local Docker Compose = DEV environment (already done!) | 🟢 Free | ✅ Done |
| 60 | Azure Deployment Slots = STAGING (same App Service, zero cost!) | 🟢 Free | ⏳ |
| 61 | Pipeline with approval gates (GitHub Actions environments) | 🟢 Free | ⏳ |
| 62 | Environment-specific appsettings.json (Dev/Staging/Production) | 🟢 Free | ⏳ |
| 63 | Terraform workspaces for multi-env (concept + implement) | 🟢 Free | ⏳ |
| 64 | Promote build: DEV(local) → STAGING(slot) → PROD(swap!) | 🟢 Free | ⏳ |

---

### ☸️ Phase 16 — Kubernetes (AKS)
> Deploy ALL microservices to AKS — this is real world! ✅

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 65 | Kubernetes fundamentals (pods, deployments, services) | 🟢 Free | ⏳ |
| 66 | Azure Kubernetes Service (AKS) cluster setup | 🟡 ~$5 | ⏳ |
| 67 | Deploy ALL microservices to AKS | 🟡 ~$5 | ⏳ |
| 68 | Kubernetes ConfigMaps + Secrets (CSI + Key Vault) | 🟢 Free | ⏳ |
| 69 | Horizontal Pod Autoscaler (scale each service independently!) | 🟢 Free | ⏳ |
| 70 | AKS Ingress Controller (NGINX — one entry point for all!) | 🟢 Free | ⏳ |
| 71 | Helm Charts (package each microservice deployment) | 🟢 Free | ⏳ |
| 72 | CI/CD per microservice → auto deploy to AKS | 🟢 Free | ⏳ |
| 73 | Delete AKS cluster after learning | 🔴 Delete! | ⏳ |

---

```
Total Topics   →  73 (was 44, added 29!)
🟢 Free        →  62 topics
🟡 Cheap       →   5 topics (~$10/month)
🔴 Delete!     →   6 topics (create → learn → delete)
─────────────────────────────────────────────
Order          →  Azure Phases 6-13 → Microservices Split
               →  Terraform → Multi-env → AKS
               →  Learn Terraform ONCE on real microservices!
Multi-env      →  FREE! (slots + local Docker, no extra Azure cost!)
Monthly Cost   →  ~$10/month (only while learning AKS)
$200 Credit    →  20+ months 🚀
```

---

## 🔜 Stage Progress Summary

| Stage | Topic | Status |
|-------|-------|--------|
| 13 | Docker + Containerization | ✅ Done |
| 14 | CI/CD Pipeline (GitHub Actions) | ✅ Done |
| 15 | Azure Phase 1 — Foundation (Account, CLI, Resource Groups, Tagging) | ✅ Done |
| 16 | Azure Phase 2 — Security (AD, Service Principal, Key Vault, RBAC, Defender) | ✅ Done |
| 17 | Azure Phase 3 — Networking (VNet, NSG, Private Endpoints, App Gateway) | ✅ Done |
| 18 | Azure Phase 4 — Storage (Blob Storage, CDN → replaced by Front Door) | ✅ Done |
| 19 | Azure Phase 5 — Hosting (Docker Hub, App Service, Managed Identity, Key Vault refs, CD Pipeline) | ✅ Done |
| 20 | Azure Phase 6 — Databases (Azure SQL, Cosmos DB, Elastic Pool) | ✅ Done |
| 21 | Azure Phase 7 — Serverless (Functions, Logic Apps, Runbooks) | ✅ Done |
| 22 | Azure Phase 8 — Messaging (Service Bus, Event Grid, Queue Storage, Redis) | 🔄 In Progress |
| 23 | Azure Phase 9 — API Gateway (Ocelot, APIM) | 🔄 In Progress |
| 24 | Azure Phase 10 — Observability (Log Analytics, App Insights, Monitor, Load Testing) | ⏳ |
| 25 | Azure Phase 11 — Search & AI (Cognitive Search, OpenAI) | ⏳ |
| 26 | Azure Phase 12 — Identity (Azure AD B2C) | ⏳ |
| 27 | Azure Phase 13 — Architect Level (Front Door, App Config, .NET Aspire) | ⏳ |
| 28 | Microservices Split (Catalog, Order, Customer, Identity) | ⏳ |
| 29 | Azure Phase 14 — Terraform / IaC (on real microservices!) | ⏳ |
| 30 | Azure Phase 15 — Multiple Environments (DEV/STAGING/PROD) | ⏳ |
| 31 | Azure Phase 16 — Kubernetes / AKS (all microservices on K8s!) | ⏳ |

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
✅ Azure CLI (installed + logged in)
✅ Azure Account (active — vikasgage28@outlook.com)
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
→ Module 8 (ACR) skipped — using Docker Hub (FREE) instead of ACR ($5/month)
→ CI pipeline (GitHub Actions) - build-and-test.yml (runs on PR)
→ CD pipeline (GitHub Actions) - build-and-push.yml (runs on merge to main)
→ Branch protection rules on develop + main
→ Pipeline must pass before PR can be merged
→ Environments: Local = DEV, Azure = PROD
→ Deployment Slots for staging concept (zero extra cost!)
→ No separate staging environment (cost saving for learning)
→ Azure CDN Classic deprecated → replaced by Front Door in Phase 13
→ App Service F1 free tier has 60min/day limit → upgraded to B1 (free 12 months)
→ Phase order changed: Azure SQL BEFORE finishing Phase 5 (app needs DB to start!)
→ App container exits code 0 in <300ms without DB = db.Database.Migrate() fails!
→ Key Vault references resolve correctly (Pull reference values = green ✅)
→ ASPNETCORE_HTTP_PORTS=80 + WEBSITES_PORT=80 set in App Service env vars
→ Always create Database BEFORE deploying app! (app needs DB to start!)
→ Always enable App Service logging before diagnosing container issues!
→ Key Vault URI must be versionless (no GUID) → always reads latest secret!
→ Non-root Docker user needs home directory created explicitly in Dockerfile!
→ Exit code 0 quickly = startup exception caught, NOT success for web apps!
→ Microsoft.Sql provider needs manual registration in Azure subscription!
→ Azure SQL Free tier auto-pauses when idle → $0 cost when not used!
→ db.Database.Migrate() runs automatically on startup → creates tables in Azure SQL!
```

---

## 🔍 Production Issues Diagnosed & Resolved

### Issue 1 — F1 Quota Exhausted
```
Symptom  → Browser: "Error 403 - This web app is stopped"
Cause    → F1 free tier = 60 min/day compute limit
           Container startup itself uses quota (no requests needed!)
Detected → az webapp show --query state → "QuotaExceeded"
Fix      → Upgraded to B1 (free for 12 months!)
           az appservice plan update --sku B1
```

### Issue 2 — No Azure SQL Server Existed!
```
Symptom  → Container exits with code 0 in < 300ms
Cause    → db.Database.Migrate() fails → exception caught → app exits cleanly!
           Phase order was wrong: Hosting before Database!
Detected → Clues: exit code 0 + dies in 88ms + az sql server list = empty!
Fix      → Created Azure SQL Server + Database (FREE tier)
           Registered Microsoft.Sql provider first!
           Added firewall rule: AllowAzureServices (0.0.0.0 - 0.0.0.0)
```

### Issue 3 — Key Vault Secret Version Pinned!
```
Symptom  → Error 18456: SQL Login Failed
Cause    → App Settings had specific version GUID in Key Vault URI:
           .../SqlConnectionString/6c9808d1029b4b09add2c3f5cea3c004
           Updated secret → new version → app still read OLD version!
Detected → Error 18456 in default_docker.log after enabling app logging
Fix      → Changed to versionless URI:
           .../SqlConnectionString/  (no GUID = always latest!) ✅
```

### Issue 4 — Non-root User Permission Denied!
```
Symptom  → System.UnauthorizedAccessException: /home/appuser is denied
Cause    → Non-root appuser had no write access to home directory
           ASP.NET Data Protection needs to write keys to /home/appuser/
           Dockerfile never created home directory for appuser!
Detected → Visible in default_docker.log (after enabling app logging)
Fix      → Updated Dockerfile:
           useradd -m appuser (create with home dir)
           mkdir -p /home/appuser/ASP.NET/DataProtection-Keys
           chown -R appuser:appgroup /home/appuser
           Rebuilt + pushed image via GitHub Actions → merged to main!
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
