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

## 📍 CURRENT STAGE — Stage 27: Phase 12.2 (Ordering Service)

### Where We Stopped
```
Phase 12.1 — Catalog Service COMPLETE! ✅

Phase 12.1 — Catalog Service completed:
✅ Solution structure created:
   → EShopMicroservices/ (new solution folder)
   → Catalog.Core, Catalog.Infrastructure, Catalog.API projects
   → Clean Architecture with project references wired correctly

✅ Catalog.Core — Domain layer:
   → Entities: Product, Category, Review
   → Interfaces: IProductRepository, ICategoryRepository, IReviewRepository, IEventPublisher
   → CQRS Features (MediatR):
      Products:   GetAllProducts, GetProductById, CreateProduct, UpdateProduct, DeleteProduct
      Categories: GetAllCategories, GetCategoryById, CreateCategory, UpdateCategory, DeleteCategory
      Reviews:    GetReviewsByProduct, CreateReview, DeleteReview
   → Domain Events: ProductCreatedEvent, ProductUpdatedEvent, ProductDeletedEvent, ProductStockChangedEvent

✅ Catalog.Infrastructure — Data + Messaging layer:
   → CatalogDbContext (SQL Server via EF Core 10)
   → ProductRepository, CategoryRepository (SQL Server)
   → ReviewRepository (Azure Cosmos DB)
   → CatalogDataSeeder (idempotent — safe to run multiple times!)
   → EF Core Migration: InitialCatalogSchema (tables auto-created at startup!)
   → InMemoryEventPublisher (logs to console — no Azure needed for dev!)
   → ServiceBusEventPublisher (stub — wired in Phase 13 CI/CD!)
   → InfrastructureServiceExtensions (registers all DI services)

✅ Catalog.API — Presentation layer:
   → ProductsController, CategoriesController, ReviewsController (all via IMediator)
   → DTOs: ProductDto, CategoryDto, ReviewDto
   → Swagger UI configured (http://localhost:5067/swagger)
   → Program.cs: MigrateAsync() + SeedAsync() run at startup (Development only!)

✅ Event-Driven Architecture implemented:
   → IEventPublisher interface (Core — no infra dependency!)
   → Dev  → InMemoryEventPublisher (logs events to console, zero Azure cost!)
   → Prod → ServiceBusEventPublisher (stub for Phase 13 with Azure Service Bus)
   → Command Handlers publish events AFTER successful DB save!
      CreateProduct → publishes ProductCreatedEvent
      UpdateProduct → publishes ProductUpdatedEvent
      DeleteProduct → publishes ProductDeletedEvent

✅ Security — Connection strings secured via dotnet User Secrets:
   → appsettings.json + appsettings.Development.json have only placeholders!
   → Real values in User Secrets (never in git!)
   → ConnectionStrings:CatalogDb → LocalDB for local dev
   → ConnectionStrings:CosmosDb  → Cosmos Emulator key (well-known public key)

✅ Local Testing verified end-to-end:
   → API starts cleanly on http://localhost:5067
   → LocalDB auto-created by MigrateAsync()
   → 2 categories + 5 products seeded automatically!
   → Swagger UI returns live data from SQL Server ✅
   → Cosmos Emulator not running → graceful warning, app continues! ✅

✅ FluentValidation + MediatR Pipeline Behavior:
   → ValidationBehavior<TRequest,TResponse> runs BEFORE every command handler!
   → Validators live in Catalog.Core (next to their commands — Clean Architecture!)
   → CreateProductCommandValidator  (Name required, Price > 0, Stock >= 0, CategoryId)
   → UpdateProductCommandValidator  (Id + same rules as Create)
   → CreateCategoryCommandValidator (Name required, max lengths)
   → UpdateCategoryCommandValidator (Id + same rules as Create)
   → ExceptionMiddleware: ValidationException → 400 Bad Request with field errors JSON
   → Zero validation code in controllers — fully automatic via pipeline! ✅

✅ Unit Tests — 23 tests all passing:
   → CreateProductCommandHandlerTests (3), UpdateProduct (4), DeleteProduct (4)
   → GetAllProductsQueryHandlerTests (3), GetProductById (3)
   → GetAllCategoriesQueryHandlerTests (3), GetCategoryById (3)
   → Tools: xUnit + Moq + FluentAssertions

✅ Git workflow completed:
   → feature/phase12-catalog-events      → PR → merged to develop ✅
   → feature/phase12-catalog-tests       → PR → merged to develop ✅
   → feature/phase12-catalog-validation  → PR → merged to develop ✅

Key Architecture Decisions made in Phase 12.1:
→ Clean Architecture: Core has ZERO infrastructure dependencies!
→ CQRS: Controllers only know IMediator — decoupled from business logic!
→ Event pattern: IEventPublisher interface + two implementations (Dev/Prod)
→ ValidationBehavior: validation automatic in pipeline, NOT in controllers!
→ User Secrets: Credentials NEVER in git — stored per-developer locally!
→ Idempotent seeding: SeedAsync() safe to call multiple times!
→ Cosmos graceful fallback: try-catch → warning → app starts without Cosmos!

Phase 11 decisions (for reference):
→ Azure Front Door  → SKIPPED! Shifted to Phase 15 (AKS)!
→ App Configuration → SHIFTED to Phase 12 (Microservices)!
                      appconfig-eshop-prod created and ready! ✅
→ .NET Aspire       → SHIFTED to Phase 12 (after microservices split!)

Phase 10 — Observability DONE!

Phase 10 — Observability completed:
✅ Application Insights created (appi-eshop-prod)
✅ Connected to App Service via APPLICATIONINSIGHTS_CONNECTION_STRING
✅ AddApplicationInsightsTelemetry() added to EShop.API Program.cs
✅ Deployed to production via GitHub Actions!
✅ Live Metrics tested!
✅ Failed requests, Response time, Server requests visible!
⏳ Alerts → skipped for now
⏳ Log Analytics → skipped for now
⏳ Load Testing → later!

Phase 9 — API Gateway DONE!

Phase 9 — APIM completed:
✅ APIM created (Consumption tier - FREE!)
✅ EShop API imported from Swagger/OpenAPI automatically!
✅ All endpoints imported in one click! (no manual adding!)
✅ Rate limiting policy added (XML policy - 10 calls/60 seconds)
✅ Subscription keys configured:
   → No key    → 401 Access Denied!
   → With key  → 200 OK!
✅ Tested via Postman - working end to end!
✅ Developer Portal → Not available on Consumption tier (skip!)

Phase 9 — Ocelot Gateway completed:
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
| 30 | Azure Service Bus Topics (publish/subscribe pattern!) | 🟡 $10/mo | ⏭️ Skipped (paid, concept known!) |
| 31 | Azure Event Grid (event-driven) | 🟢 Free | ⏭️ Skipped (nothing to implement!) |
| 32 | Azure Queue Storage | 🟢 Free | ⏭️ Skipped (same concept as Service Bus Queue!) |
| 33 | Azure Redis Cache | 🔴 $16/mo | ⏭️ Skipped (paid, concept known!) |

---

### 🚪 Phase 9 — API Gateway (route all traffic through one door!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 33 | Ocelot API Gateway (.NET) | 🟢 Free | ✅ Done |
| 34 | Azure API Management (APIM) | 🟢 Free | ✅ Done |

---

### 📊 Phase 10 — Observability (app is live → now monitor it!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 35 | Log Analytics Workspace | 🟢 Free | ⏳ Later |
| 36 | Application Insights | 🟢 Free | ✅ Done |
| 37 | Azure Monitor + Alerts | 🟢 Free | ⏳ Later |
| 38 | Azure Load Testing | 🟢 Free | ⏳ |

---

### 🌍 Phase 11 — Architect Level
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 34 | Azure Front Door (global load balancing) | 🔴 Delete! | ⏭️ Shifted to Phase 15 (AKS)! |
| 35 | Azure App Configuration (centralized config) | 🟢 Free | ⏭️ Shifted to Phase 12 (Microservices)! appconfig-eshop-prod created! |
| 36 | .NET Aspire (cloud-native orchestration) | 🟢 Free | ⏭️ Shifted to Phase 12 (after microservices split!) |

---

### ⭐ Phase 12 — Microservices Split
> Logical learning sequence: Design → Split → Communicate → Configure → Orchestrate → Containerize!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 37 | Design microservices boundaries (Catalog, Order, Customer, Identity) | 🟢 Free | ✅ Done |
| 38 | Split monolith → Catalog Service (.NET) — Clean Arch + CQRS + Events + LocalDB + User Secrets | 🟢 Free | ✅ Done (Phase 12.1!) |
| 39 | Split monolith → Order Service (.NET) | 🟢 Free | ⏳ |
| 40 | Split monolith → Customer Service (.NET) | 🟢 Free | ⏳ |
| 41 | Split monolith → Identity Service (.NET) | 🟢 Free | ⏳ |
| 42 | Service-to-service communication (HTTP + Service Bus) | 🟢 Free | ⏳ |
| 43 | Azure App Configuration — central hub (settings + KV refs!) | 🟢 Free | ⏳ |
| 44 | Each microservice reads from App Config only (one source!) | 🟢 Free | ⏳ |
| 45 | Docker Compose for ALL microservices locally | 🟢 Free | ⏳ |
| 46 | .NET Aspire — orchestrate all services locally + dashboard! | 🟢 Free | ⏳ |
| 47 | Azure Container Registry (ACR) — private registry for all images! | 🟡 $5 | ⏳ |
| 48 | Update CI/CD pipelines → build + push each service image to ACR | 🟢 Free | ⏳ |
| 49 | Managed Identity → AKS pulls from ACR (no password needed!) | 🟢 Free | ⏳ |

---

### 🌎 Phase 13 — Multiple Environments (DEV / STAGING / PROD)
> Real world: Always 3 environments! Never deploy direct to PROD!
> ⚠️ Cost Decision: NOT creating separate Azure environments (too expensive!)
> Smart approach: Learn concepts + simulate using FREE tools!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 50 | Environment strategy concept (DEV → STAGING → PROD) | 🟢 Free | ⏳ |
| 51 | Local Docker Compose = DEV environment (already done!) | 🟢 Free | ✅ Done |
| 52 | Azure Deployment Slots = STAGING (same App Service, zero cost!) | 🟢 Free | ⏳ |
| 53 | Pipeline with approval gates (GitHub Actions environments) | 🟢 Free | ⏳ |
| 54 | Environment-specific appsettings.json (Dev/Staging/Production) | 🟢 Free | ⏳ |
| 55 | Promote build: DEV(local) → STAGING(slot) → PROD(swap!) | 🟢 Free | ⏳ |

---

### ☸️ Phase 14 — Kubernetes (AKS)
> Deploy ALL microservices to AKS — this is real world! ✅

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 56 | Azure Container Apps (stepping stone before AKS!) | 🟡 $1-3 | ⏳ |
| 57 | Kubernetes fundamentals (pods, deployments, services) | 🟢 Free | ⏳ |
| 58 | Azure Kubernetes Service (AKS) cluster setup | 🟡 ~$5 | ⏳ |
| 59 | Deploy ALL microservices to AKS | 🟡 ~$5 | ⏳ |
| 60 | Kubernetes ConfigMaps + Secrets (CSI + Key Vault) | 🟢 Free | ⏳ |
| 61 | Horizontal Pod Autoscaler (scale each service independently!) | 🟢 Free | ⏳ |
| 62 | AKS Ingress Controller (NGINX — one entry point for all!) | 🟢 Free | ⏳ |
| 63 | Azure Front Door (global entry point for AKS!) | 🔴 Delete! | ⏳ |
| 64 | Helm Charts (package each microservice deployment) | 🟢 Free | ⏳ |
| 65 | CI/CD per microservice → auto deploy to AKS | 🟢 Free | ⏳ |
| 66 | Delete AKS cluster after learning | 🔴 Delete! | ⏳ |

---

### ⚛️ Phase 15 — React Frontend (UI for EShop!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 67 | React project setup (Vite + TypeScript) | 🟢 Free | ⏳ |
| 68 | Azure Static Web Apps (host React — FREE!) | 🟢 Free | ⏳ |
| 69 | Connect React to EShop.API via APIM | 🟢 Free | ⏳ |
| 70 | Products listing page | 🟢 Free | ⏳ |
| 71 | Login/Register page (using our JWT) | 🟢 Free | ⏳ |
| 72 | Admin dashboard (create/update products) | 🟢 Free | ⏳ |

---

### 🔑 Phase 16 — Authentication Deep Dive (with React UI!)
> Logical sequence: Concepts → Auth0 (easy!) → Azure AD B2C → Enterprise → Advanced!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 73 | OAuth 2.0 concept (flows, tokens, scopes — theory first!) | 🟢 Free | ⏳ |
| 74 | OpenID Connect (OIDC) concept (built on OAuth 2.0!) | 🟢 Free | ⏳ |
| 75 | Auth0 setup (FREE! 7,500 users — easiest to start with!) | 🟢 Free | ⏳ |
| 76 | Auth0 + React — Authorization Code Flow (login/logout!) | 🟢 Free | ⏳ |
| 77 | Auth0 — Social Logins (Google + GitHub — free!) | 🟢 Free | ⏳ |
| 78 | Auth0 — Refresh Tokens (silent re-auth!) | 🟢 Free | ⏳ |
| 79 | Auth0 — Machine to Machine (service-to-service!) | 🟢 Free | ⏳ |
| 80 | Azure AD B2C (same concepts — Azure native!) | 🟢 Free | ⏳ |
| 81 | Azure AD B2C + React (replace Auth0 with AD B2C!) | 🟢 Free | ⏳ |
| 82 | Social Logins via Azure AD B2C (Google + Microsoft!) | 🟢 Free | ⏳ |
| 83 | Azure AD / Entra ID (enterprise employee login!) | 🟢 Free | ⏳ |
| 84 | Client Credentials Flow (microservice to microservice!) | 🟢 Free | ⏳ |
| 85 | SAML 2.0 (corporate SSO — enterprise apps!) | 🟢 Free | ⏳ |
| 86 | Mutual TLS - mTLS (bank-level security!) | 🟢 Free | ⏳ |

---

### 🤖 Phase 17 — Search & AI (full app ready!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 87 | Azure Cognitive Search (product search + fuzzy!) | 🟢 Free | ⏳ |
| 88 | Azure OpenAI — GPT-4 (product recommendations) | 🟡 $1-2 | ⏳ |
| 89 | AI Chatbot in React (customer support!) | 🟡 $1-2 | ⏳ |
| 90 | AI product description generator | 🟡 $1-2 | ⏳ |

---

### 🏗️ Phase 18 — Infrastructure as Code (Terraform)
> Do at the END! Know all resources first, then automate everything!
> Real world: Build manually → understand → then codify!
> Nobody clicks portal in production! Everything is code!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 91 | Terraform Fundamentals (providers, state, plan, apply) | 🟢 Free | ⏳ |
| 92 | terraform import → import all existing resources! | 🟢 Free | ⏳ |
| 93 | Terraform modules for each microservice (SQL, KV, ACR) | 🟢 Free | ⏳ |
| 94 | Terraform — AKS cluster + networking + RBAC | 🟢 Free | ⏳ |
| 95 | Terraform — Full EShop infra in one command! | 🟢 Free | ⏳ |
| 96 | Terraform remote state (Azure Blob Storage backend) | 🟢 Free | ⏳ |
| 97 | Terraform workspaces (DEV / STAGING / PROD configs) | 🟢 Free | ⏳ |

---

## 🤖 AI/ML/GenAI Learning Path (Phases 19–29)
> Learn AI the same way we learned Azure — Concept → Implement → Deploy into EShop!
> Real organization approach: Use AI as a tool, integrate into real app, not toy examples!
> Languages: Python (Phases 19-25 concepts + ML) → C#/.NET (Phases 26-28 production AI)

### 🧠 Phase 19 — AI/ML/GenAI Concepts (Theory Only! No Code!)
> Start here! Understand everything before touching code!
> Same as learning Azure concepts before creating any resources!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 98  | What is AI vs ML vs Deep Learning vs GenAI? (clear differences!) | 🟢 Free | ⏳ |
| 99  | How neural networks work (concept, no math!) | 🟢 Free | ⏳ |
| 100 | How LLMs work internally (GPT, Claude, Gemini!) | 🟢 Free | ⏳ |
| 101 | Tokens, Embeddings, Context Window, Temperature, Hallucination | 🟢 Free | ⏳ |
| 102 | Transformer architecture concept (foundation of GPT!) | 🟢 Free | ⏳ |
| 103 | What is RAG, Vector DB, Prompt Engineering, Fine-tuning? (when to use what!) | 🟢 Free | ⏳ |

---

### 🐍 Phase 20 — Python for AI
> You already know programming! Python = just different syntax — 2-3 weeks max!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 104 | Python syntax (variables, loops, functions, classes — compare to C#!) | 🟢 Free | ⏳ |
| 105 | pip + virtual environments (like NuGet + .NET SDK versions!) | 🟢 Free | ⏳ |
| 106 | Jupyter Notebooks (industry standard for all AI work!) | 🟢 Free | ⏳ |
| 107 | NumPy (fast arrays + math!) | 🟢 Free | ⏳ |
| 108 | Pandas (data tables — analyze EShop product + order data!) | 🟢 Free | ⏳ |
| 109 | Matplotlib (charts — visualize EShop sales patterns!) | 🟢 Free | ⏳ |

---

### 📊 Phase 21 — Classical ML (scikit-learn)
> How machines learn from data! Foundation for everything that follows!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 110 | Supervised vs Unsupervised learning (concept + when to use!) | 🟢 Free | ⏳ |
| 111 | Linear Regression → predict EShop product price! | 🟢 Free | ⏳ |
| 112 | Classification (Decision Tree, Random Forest) → will customer churn? | 🟢 Free | ⏳ |
| 113 | K-Means Clustering → group similar EShop products! | 🟢 Free | ⏳ |
| 114 | Model evaluation (accuracy, precision, recall, F1!) | 🟢 Free | ⏳ |
| 115 | Train/Test split, Cross validation, Overfitting vs Underfitting! | 🟢 Free | ⏳ |

---

### 🧬 Phase 22 — Deep Learning (PyTorch)
> Neural networks, CNNs, LSTMs, Transformers — the foundation of GPT!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 116 | Neural networks (layers, neurons, weights, activation functions!) | 🟢 Free | ⏳ |
| 117 | Backpropagation (how AI learns from mistakes!) | 🟢 Free | ⏳ |
| 118 | CNN → EShop product image classifier! (Electronics, Clothing, etc.) | 🟢 Free | ⏳ |
| 119 | RNN/LSTM → EShop review sentiment analysis! | 🟢 Free | ⏳ |
| 120 | Transformer + Attention mechanism (how GPT is built internally!) | 🟢 Free | ⏳ |
| 121 | Transfer learning → use pre-trained model (like using a NuGet package!) | 🟢 Free | ⏳ |

---

### 🤗 Phase 23 — HuggingFace Library
> GitHub for AI models! 500,000+ pre-trained models — download in 2 lines of Python!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 122 | HuggingFace Transformers library + Pipelines (load any model easily!) | 🟢 Free | ⏳ |
| 123 | Sentiment pipeline → analyze EShop reviews in 2 lines! | 🟢 Free | ⏳ |
| 124 | Text Embeddings → convert EShop products to vectors (needed for RAG!) | 🟢 Free | ⏳ |
| 125 | Fine-tuning pre-trained model on EShop product descriptions! | 🟢 Free | ⏳ |
| 126 | HuggingFace Hub → explore + share models! | 🟢 Free | ⏳ |

---

### 🌟 Phase 24 — GenAI Concepts (Theory! Understand Before Building!)
> What is Generative AI? How does it work? What types exist? When to use what?
> Concept first — no code yet! Same approach as Phase 19!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 127 | Generative AI vs Discriminative AI (ML classifies → GenAI CREATES!) | 🟢 Free | ⏳ |
| 128 | Types of GenAI models → LLMs (text), Diffusion (images), GANs, VAEs | 🟢 Free | ⏳ |
| 129 | How text generation works step by step (sampling, temperature, top-k, top-p!) | 🟢 Free | ⏳ |
| 130 | RLHF — Reinforcement Learning from Human Feedback (how ChatGPT was made helpful!) | 🟢 Free | ⏳ |
| 131 | Fine-tuning vs RAG vs Prompt Engineering — when to use which approach! | 🟢 Free | ⏳ |
| 132 | Open source vs Closed LLMs (GPT-4 vs LLaMA vs Mistral vs Claude!) | 🟢 Free | ⏳ |
| 133 | LLM evaluation — how to measure quality of AI output! | 🟢 Free | ⏳ |
| 134 | GenAI limitations — hallucination, bias, cost, latency, safety! | 🟢 Free | ⏳ |

---

### ⚡ Phase 25 — GenAI Implementation (Build Real GenAI Features in EShop!)
> Now implement! Use Azure OpenAI SDK in C# to build real GenAI features!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 135 | Build text generation app in C# (Azure OpenAI SDK!) | 🟡 $1-2 | ⏳ |
| 136 | Build image generation (DALL-E) → auto-generate product images in EShop! | 🟡 $1-2 | ⏳ |
| 137 | Function calling — AI automatically calls EShop API endpoints! | 🟡 $1-2 | ⏳ |
| 138 | Structured output from LLMs (force JSON response for C# deserialization!) | 🟡 $1-2 | ⏳ |
| 139 | LLM chaining — output of one LLM → input to next LLM! | 🟡 $1-2 | ⏳ |
| 140 | Multi-modal AI — text + image together (describe product from photo!) | 🟡 $1-2 | ⏳ |
| 141 | Complete EShop GenAI feature → AI writes description + generates image! | 🟡 $2-3 | ⏳ |

---

### 💬 Phase 26 — Prompt Engineering
> Art of talking to AI correctly — now you have GenAI context to understand WHY!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 142 | Zero-shot prompting (ask directly, no examples!) | 🟢 Free | ⏳ |
| 143 | Few-shot prompting (give 2-3 examples → AI follows pattern!) | 🟢 Free | ⏳ |
| 144 | Chain of Thought — tell AI to think step by step! | 🟢 Free | ⏳ |
| 145 | System prompts + prompt templates with variables | 🟢 Free | ⏳ |
| 146 | Output formatting (JSON responses!) + avoid hallucination techniques | 🟢 Free | ⏳ |

---

### 🔍 Phase 27 — RAG (Retrieval Augmented Generation)
> Most used AI pattern in industry! AI answers from YOUR real data, not hallucination!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 147 | What is RAG? Why plain GPT fails for real apps (no product knowledge!) | 🟢 Free | ⏳ |
| 148 | Embeddings deep dive → convert text to vectors (numbers!) | 🟢 Free | ⏳ |
| 149 | Vector databases → ChromaDB (local) + Azure AI Search (cloud!) | 🟡 $1-2 | ⏳ |
| 150 | Indexing pipeline — embed all EShop products + store in vector DB! | 🟡 $1-2 | ⏳ |
| 151 | Retrieval pipeline — find similar products by user query! | 🟡 $1-2 | ⏳ |
| 152 | Generation step — GPT answers using retrieved EShop products! | 🟡 $1-2 | ⏳ |
| 153 | Chunking strategies + hybrid search + reranking! | 🟡 $1-2 | ⏳ |

---

### ☁️ Phase 28 — Azure OpenAI Setup
> Set up Azure OpenAI FIRST — needed before Semantic Kernel (Phase 29)!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 154 | Azure OpenAI Service setup (GPT-4 private deployment in Azure!) | 🟡 $2-3 | ⏳ |
| 155 | Call Azure OpenAI from C# (Azure SDK — no Python needed!) | 🟡 $1-2 | ⏳ |
| 156 | Connect RAG pipeline to Azure OpenAI (C# end-to-end!) | 🟡 $1-2 | ⏳ |
| 157 | Azure AI Search — index EShop products as vectors! | 🟡 $1-2 | ⏳ |

---

### 🔷 Phase 29 — Semantic Kernel (.NET)
> Microsoft's AI framework for .NET! Build production AI apps in C# — not Python!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 158 | Semantic Kernel setup + connect to Azure OpenAI! | 🟡 $1-2 | ⏳ |
| 159 | Plugins — AI calls YOUR C# functions automatically! | 🟡 $1-2 | ⏳ |
| 160 | Memory — vector store in .NET (RAG fully in C#!) | 🟡 $1-2 | ⏳ |
| 161 | Planner — AI creates execution plan + runs steps automatically! | 🟡 $1-2 | ⏳ |
| 162 | Agents — autonomous AI in C#! | 🟡 $1-2 | ⏳ |
| 163 | EShop.AI microservice — ProductRecommendation + CustomerSupport plugins! | 🟡 $2-3 | ⏳ |

---

### 🌐 Phase 30 — Full Azure AI Services
> All remaining Azure AI tools integrated into EShop (all C#!)

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 164 | Azure Computer Vision → product image analysis + auto-tagging! | 🟡 $1-2 | ⏳ |
| 165 | Azure Language Service → sentiment on reviews + key phrase extraction! | 🟡 $1-2 | ⏳ |
| 166 | Azure Speech Service → voice search in EShop! | 🟡 $1-2 | ⏳ |
| 167 | Azure AI Studio → playground, test prompts, deploy + manage models! | 🟡 $1-2 | ⏳ |
| 168 | Azure Bot Service → deploy AI chatbot to React UI! | 🟡 $1-2 | ⏳ |
| 169 | Azure Document Intelligence → OCR on invoices + product documents! | 🟡 $1-2 | ⏳ |

---

### ⚙️ Phase 31 — MLOps (DevOps for Machine Learning!)
> CI/CD for ML models! Your existing DevOps knowledge shines here!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 170 | MLOps concept — ML lifecycle (train → deploy → monitor → retrain!) | 🟢 Free | ⏳ |
| 171 | MLflow — experiment tracking + model versioning! | 🟢 Free | ⏳ |
| 172 | Azure Machine Learning platform (train models in cloud!) | 🟡 $1-2 | ⏳ |
| 173 | Deploy ML model as REST API → EShop.API calls it from C#! | 🟡 $1-2 | ⏳ |
| 174 | GitHub Actions for ML (train on PR, deploy if accuracy improves!) | 🟢 Free | ⏳ |
| 175 | Model monitoring — detect accuracy drift, trigger alerts! | 🟡 $1-2 | ⏳ |
| 176 | Auto-retrain pipeline when new EShop orders arrive! | 🟡 $1-2 | ⏳ |

---

```
Total Topics   →  176
🟢 Free        →  124 topics
🟡 Cheap       →   48 topics (~$10-15/month)
🔴 Delete!     →    4 topics (create → learn → delete)
─────────────────────────────────────────────
Order          →  Microservices → Multi-env → AKS
               →  React → Auth (Auth0 first! → AD B2C!)
               →  AI Concepts → Python → Classical ML → Deep Learning
               →  HuggingFace → GenAI Concepts → GenAI Implementation
               →  Prompt Engineering → RAG → Azure OpenAI
               →  Semantic Kernel → Azure AI Services → MLOps → Terraform LAST!
Key decisions  →  Auth0 FREE (easy start!) → AD B2C (Azure native!)
               →  Terraform at END (know all resources first!)
               →  Build manually → understand → automate! ✅
               →  AI: Concept first → Python → ML → GenAI (logical order!) ✅
               →  GenAI: Concepts phase → Implementation phase (dedicated!) ✅
               →  AI in C#/.NET for production (Semantic Kernel!) ✅
Monthly Cost   →  ~$10-15/month (AKS + AI services while learning)
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
| 22 | Azure Phase 8 — Messaging (Service Bus, Event Grid, Queue Storage, Redis) | ✅ Done (remaining skipped — paid/concept known!) |
| 23 | Azure Phase 9 — API Gateway (Ocelot, APIM) | ✅ Done |
| 24 | Azure Phase 10 — Observability (Log Analytics, App Insights, Monitor, Load Testing) | ✅ Done |
| 25 | Phase 11 — Architect Level (Front Door/App Config/Aspire → all shifted to Phase 12!) | ✅ Done |
| 26 | Phase 8 Remaining — Messaging (Topics, Event Grid, Queue, Redis) | ⏭️ Skipped (paid/concept known!) |
| 27 | Phase 12.1 — Catalog Service (Clean Arch + CQRS + Events + User Secrets) | ✅ Done |
| 27 | Phase 12 — Microservices Split (Ordering, Customer, Identity + App Config + Aspire + ACR) | 🔄 In Progress |
| 28 | Phase 13 — Multiple Environments (DEV/STAGING/PROD) | ⏳ |
| 29 | Phase 14 — Kubernetes / AKS (Container Apps → AKS → Front Door!) | ⏳ |
| 30 | Phase 15 — React Frontend (Static Web Apps, connect to API!) | ⏳ |
| 31 | Phase 16 — Auth Deep Dive (Auth0 → AD B2C → SAML → mTLS!) | ⏳ |
| 32 | Phase 17 — Search & AI (Cognitive Search, OpenAI, Chatbot!) | ⏳ |
| 33 | Phase 18 — Terraform / IaC (automate everything at the end!) | ⏳ |
| 34 | Phase 19 — AI/ML/GenAI Concepts (Theory Only! No Code!) | ⏳ |
| 35 | Phase 20 — Python for AI (NumPy, Pandas, Matplotlib!) | ⏳ |
| 36 | Phase 21 — Classical ML / scikit-learn (price prediction, churn!) | ⏳ |
| 37 | Phase 22 — Deep Learning / PyTorch (CNN, LSTM, Transformer!) | ⏳ |
| 38 | Phase 23 — HuggingFace Library (pre-trained models, embeddings!) | ⏳ |
| 39 | Phase 24 — GenAI Concepts (GenAI types, RLHF, LLM evaluation, limitations!) | ⏳ |
| 40 | Phase 25 — GenAI Implementation (text gen, image gen, function calling!) | ⏳ |
| 41 | Phase 26 — Prompt Engineering (zero-shot, few-shot, CoT!) | ⏳ |
| 42 | Phase 27 — RAG (vector DB, index EShop products, AI search!) | ⏳ |
| 43 | Phase 28 — Azure OpenAI Setup (GPT-4 private, C# SDK!) | ⏳ |
| 44 | Phase 29 — Semantic Kernel .NET (EShop.AI microservice!) | ⏳ |
| 45 | Phase 30 — Full Azure AI Services (Vision, Speech, Language, Bot!) | ⏳ |
| 46 | Phase 31 — MLOps (CI/CD for ML, auto-retrain, model monitoring!) | ⏳ |

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

## 🎯 Complete Technology Skills — Full Roadmap!

### 🏗️ Backend & Architecture
```
1.  Clean Architecture
2.  CQRS + MediatR
3.  Repository Pattern
4.  Dependency Injection
5.  Middleware (custom pipeline!)
6.  Global Error Handling
7.  API Versioning
8.  Pagination + Filtering
9.  FluentValidation
10. CORS
11. User Secrets
```

### 🗄️ Database & ORM
```
12. Entity Framework Core
13. SQL Server
14. EF Migrations
15. Cosmos DB (NoSQL)
16. Redis (caching!)
17. Azure SQL
18. Elastic Pool
```

### 🔐 Security & Authentication
```
19. JWT Authentication
20. ASP.NET Identity
21. OAuth 2.0
22. OpenID Connect (OIDC)
23. Auth0
24. Azure AD B2C
25. RBAC (Role Based Access Control)
26. Azure Key Vault
27. Managed Identity
28. Service Principal
29. SAML (concept!)
30. mTLS (concept!)
```

### 📝 Logging & Observability
```
31. Serilog
32. Azure Application Insights
33. Azure Log Analytics
34. Azure Monitor
35. Live Metrics
36. Distributed Tracing
37. Health Checks
```

### 🧪 Testing
```
38. Unit Testing (xUnit)
39. Moq (mocking!)
40. FluentAssertions
41. Integration Testing
```

### 🐳 Containers & Orchestration
```
42. Docker
43. Dockerfile (multi-stage!)
44. Docker Compose
45. Azure Container Registry (ACR)
46. Kubernetes (AKS)
47. Helm Charts
48. .NET Aspire
```

### ☁️ Azure Services
```
49. Azure CLI
50. Azure Resource Groups + Tagging
51. Azure App Service
52. Azure VNet + NSG
53. Azure Private Endpoints
54. Azure Application Gateway
55. Azure Blob Storage
56. Azure CDN
57. Azure Functions
58. Azure Logic Apps
59. Azure Automation Runbooks
60. Azure Service Bus
61. Azure Event Grid
62. Azure Queue Storage
63. Azure API Management (APIM)
64. Azure Front Door
65. Azure App Configuration
66. Azure Defender
67. Azure Static Web Apps
68. Azure Bot Service
69. Azure AI Search (Cognitive Search)
```

### 🚀 CI/CD & DevOps
```
70. GitHub Actions
71. Branch Protection Rules
72. PR Workflow
73. Deployment Slots
74. Blue/Green Deployment
75. Environment Approvals
76. GitFlow Strategy
```

### 🏛️ Microservices
```
77. Microservices Architecture
78. Bounded Context (DDD!)
79. Strangler Fig Pattern
80. Service-to-Service HTTP
81. gRPC (internal communication!)
82. API Gateway Pattern
83. Ocelot Gateway
84. Event Driven Architecture
85. Publish-Subscribe Pattern
86. Saga Pattern (distributed transactions!)
87. Service Discovery
```

### ⚛️ Frontend
```
88. React
89. TypeScript
90. Vite
91. React Router
92. State Management
93. Axios (API calls!)
94. Azure Static Web Apps
```

### 🏗️ Infrastructure as Code
```
95. Terraform
96. Terraform State
97. Terraform Modules
98. Terraform Workspaces
99. terraform import
```

### 🐍 Python for AI
```
100. Python syntax
101. Virtual environments
102. Jupyter Notebooks
103. NumPy
104. Pandas
105. Matplotlib
```

### 🤖 Machine Learning
```
106. Supervised Learning
107. Unsupervised Learning
108. Linear Regression
109. Classification
110. Decision Trees + Random Forest
111. K-Means Clustering
112. Model Evaluation (accuracy, F1, precision!)
113. Cross Validation
114. Overfitting vs Underfitting
115. scikit-learn
```

### 🧬 Deep Learning
```
116. Neural Networks
117. Backpropagation
118. CNN (image recognition!)
119. RNN/LSTM (sequences!)
120. Transformer Architecture
121. Attention Mechanism
122. Transfer Learning
123. PyTorch
```

### 🌟 Generative AI
```
124. LLMs (GPT, Claude, Gemini!)
125. Tokens + Embeddings
126. Context Window
127. Temperature + Top-k + Top-p
128. RLHF
129. Hallucination + how to avoid!
130. Fine-tuning vs RAG
131. Open source vs Closed LLMs
132. LLM Evaluation
133. Text Generation
134. Image Generation (DALL-E!)
135. Function Calling
136. LLM Chaining
137. Multi-modal AI
138. HuggingFace
```

### 💬 Prompt Engineering
```
139. Zero-shot prompting
140. Few-shot prompting
141. Chain of Thought (CoT)
142. System prompts
143. Prompt templates
144. Structured output (JSON!)
```

### 🔍 RAG
```
145. Vector Databases
146. ChromaDB
147. Embeddings deep dive
148. Indexing pipeline
149. Retrieval pipeline
150. Chunking strategies
151. Hybrid search
152. Reranking
```

### 🔷 .NET AI (Semantic Kernel)
```
153. Semantic Kernel
154. SK Plugins
155. SK Memory
156. SK Planner
157. SK Agents
158. Microsoft.Extensions.AI
159. Azure OpenAI SDK (C#!)
```

### ☁️ Azure AI Services
```
160. Azure OpenAI Service
161. Azure Computer Vision
162. Azure Language Service
163. Azure Speech Service
164. Azure AI Studio
165. Azure Document Intelligence
166. Azure Machine Learning
```

### ⚙️ MLOps
```
167. ML Lifecycle
168. MLflow
169. Experiment Tracking
170. Model Versioning
171. Model Deployment (REST API!)
172. GitHub Actions for ML
173. Model Monitoring
174. Drift Detection
175. Auto-retrain Pipelines
```

---

### 📊 Skills Summary
```
🏗️  Backend + Architecture     →  11 skills
🗄️  Database + ORM             →   7 skills
🔐  Security + Auth            →  12 skills
📝  Logging + Observability    →   7 skills
🧪  Testing                    →   4 skills
🐳  Containers + K8s           →   7 skills
☁️  Azure Services             →  21 skills
🚀  CI/CD + DevOps             →   7 skills
🏛️  Microservices              →  11 skills
⚛️  Frontend (React)           →   7 skills
🏗️  Terraform (IaC)            →   5 skills
🐍  Python for AI              →   6 skills
🤖  Machine Learning           →  10 skills
🧬  Deep Learning              →   8 skills
🌟  Generative AI              →  15 skills
💬  Prompt Engineering         →   6 skills
🔍  RAG                        →   8 skills
🔷  .NET AI (Semantic Kernel)  →   7 skills
☁️  Azure AI Services          →   7 skills
⚙️  MLOps                      →   9 skills
──────────────────────────────────────────────
TOTAL                          → 175 skills!

Duration  → ~2-3 years
Outcome   → Principal AI Cloud Architect! 💰
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
