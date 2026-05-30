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

## 📍 CURRENT STAGE — Phase 14: Authentication Deep Dive — STARTING NOW

### Key Credentials (never changes)
```
Admin:    admin@eshop.com  / Admin@12345
Customer: alice@eshop.com  / Customer@12345
Currency: INR (₹) — en-IN locale
UI Theme: Lenovo Vantage Blue (#0067c0), 165px Sidebar
Frontend: http://localhost:5173 (Vite dev server)
Aspire:   https://localhost:17222 (dashboard)
Ports:    catalog=5010, customer=5011/5022(gRPC), ordering=5012, identity=5013
```

---

### Phase 13 — React Frontend COMPLETE ✅ (Fully Polished)
```
Phase 13 — React 19 Frontend — ALL DONE ✅

Stack: React 19 + TypeScript + Vite 8 + Tailwind CSS v4 + Shadcn/ui
State: Redux Toolkit + RTK Query + Axios JWT interceptor
Router: React Router DOM v7 — nested layouts, ProtectedRoute, role-based redirect

Pages Built:
→ Login / Register  — wider 700px cards, show/hide password, inline errors
→ Products          — RTK Query, search debounce, category filter, pagination, Picsum images
→ Product Detail    — full info, reviews, qty selector, Add to Cart
→ Admin (Products)  — CRUD table with Create/Edit/Delete dialog (Admin only)
→ Cart              — Redux slice, qty controls, remove, subtotal, live badge in Sidebar
→ Checkout          — saved address selector + new address form + save-to-profile checkbox
→ Orders            — table with status badges, click row → Order Detail
→ Order Detail      — full order info, items list, Cancel Order for Pending
→ Customers         — admin-only, search, Register (from site users), Unregister, address count badge
→ Dashboard         — stat cards (Products/Orders/Customers/Revenue) + Recent Orders table
→ Profile           — avatar, account info, role badges, order stats, addresses, actions

Address Management (Customer DB ↔ Checkout):
→ CustomerDB stores structured Address records (street, city, state, country, postalCode, isDefault)
→ OrderingDB stores flat shipping address string snapshot at order time
→ Profile page — full CRUD for saved addresses (Add / Delete)
→ Checkout — saved address cards selector + "Use different address" + "Save to profile" checkbox
→ AddAddress / DeleteAddress commands + handlers + ICustomerRepository + controller endpoints
→ RTK Query tag invalidation keeps Profile and Checkout in sync

Customer Registration Flow (Admin):
→ GET /api/auth/users — fetches all Identity users
→ Filters out users who already have a Customer profile (by email)
→ Admin picks from dropdown → creates Customer profile → list refreshes

Bug Fixes Applied:
→ ProfilePage orders: was using Identity userId (wrong) → now uses customer.id (correct)
→ OrdersPage: same fix applied earlier (customer.id lookup by email first)
→ CheckoutPage: alert() replaced with amber inline validation error banner
→ Dashboard status colors: extracted to shared orderStatusColor in lib/utils.ts

Polish Applied:
→ Shared orderStatusColor constant in lib/utils.ts — OrdersPage + DashboardPage both use it
→ Orders page: added <h1> heading (consistent with all other pages)
→ Checkout success: "Continue Shopping" → "Browse Products"
→ Profile page: "Browse Products" button added for customers (admins excluded)
→ Customers table: Addresses column with blue pill badge showing count per customer

Custom Hooks:
→ useAuth — reads Redux auth slice, exposes email/userId/roles/isAdmin/fullName
→ useCart — cart Redux slice CRUD + computed totals
→ useDebounce — debounced search inputs (300ms)
→ useDarkMode — localStorage persistence, Moon/Sun toggle in TopBar

UI Design:
→ Lenovo Vantage style — #0067c0 blue, 165px sidebar, solid active state, #f4f4f4 bg
→ Base font 20px in index.css → rem-based Tailwind (text-sm/xs) scales proportionally
→ Dark mode — full dark theme via Tailwind dark: variants, toggled in TopBar

Deferred (do later — not blocking):
→ Azure Static Web Apps deploy — shifting to Phase 15 (Cloud Deploy) with all services
→ Zod + React Hook Form — optional upgrade (forms work fine with useState)
→ React 19 new hooks (useTransition, useOptimistic) — optional deep dive
→ Categories admin page — optional (products work without it)
```

---

### Phase 14 — Authentication Deep Dive — NEXT ⬅️
```
Philosophy:
→ API Gateway owns auth validation — services trust the gateway (correct microservice pattern)
→ Individual services do NOT add [Authorize] — gateway handles it (like Netflix, Uber)
→ Each auth mode implemented in Identity.API + React frontend — end-to-end working demo
→ No Azure needed until items 20-22 — months of learning first!

Complete Authentication Sequence:
─────────────────────────────────────────────────────────────────
🟢 No Azure Needed — Implement In Order
─────────────────────────────────────────────────────────────────
  1.  Silent Token Refresh        ⏳  Auto-renew JWT before expiry — Axios interceptor (80% done!)
  2.  Refresh Token Rotation      ⏳  Each refresh → new token, old one invalidated immediately
  3.  JWT RS256 (Asymmetric)      ⏳  Upgrade HS256 shared secret → RS256 public/private key pair
  4.  2FA — Email OTP             ⏳  MailKit + Gmail SMTP — 6-digit code sent to email
  5.  2FA — TOTP (Authenticator)  ⏳  Google Authenticator / Authy — 30s rotating code (QRCoder NuGet)
  6.  SMS OTP                     ⏳  Twilio / MSG91 — OTP on mobile number
  7.  Magic Links                 ⏳  Passwordless — HMAC-signed link emailed to user (Slack/Notion style)
  8.  Step-up Auth                ⏳  Re-verify for sensitive actions (e.g. cancel order > ₹10,000)
  9.  OAuth 2.0 + PKCE            ⏳  Authorization Code flow — Auth0 free tier, industry standard
  10. OIDC (OpenID Connect)       ⏳  id_token + userinfo endpoint + discovery doc (on top of OAuth 2.0)
  11. Social Logins               ⏳  Google + GitHub via Auth0 — uses OIDC internally
  12. Client Credentials Flow     ⏳  Machine-to-machine OAuth — no user involved (B2B APIs)
  13. Device Authorization Grant  ⏳  GitHub CLI / Netflix TV / IoT — code shown on device
  14. PAT (Personal Access Token) ⏳  GitHub-style long-lived scoped developer tokens
  15. API Key Auth                ⏳  Stripe-style — for service accounts and external integrations
  16. Risk-based / Adaptive Auth  ⏳  New device/location detected → triggers extra challenge
  17. QR Code Login               ⏳  WhatsApp Web style — scan QR with phone to log in on desktop
  18. Passkeys / WebAuthn         ⏳  Fingerprint / Face ID — Fido2.NET NuGet (future of auth!)
  19. mTLS                        ⏳  Certificate-based service-to-service — local self-signed certs

─────────────────────────────────────────────────────────────────
🔵 Needs Azure — Do Later (Phase 15 onwards)
─────────────────────────────────────────────────────────────────
  20. Azure AD B2C                ⏳  Consumer identity — OIDC with custom policies + branding
  21. Entra ID (Azure AD)         ⏳  Enterprise — "Login with Microsoft" for employees
  22. Workload Identity           ⏳  AKS pod gets Azure token automatically — no passwords in K8s

─────────────────────────────────────────────────────────────────
🟣 Enterprise / Advanced — After Azure Phase
─────────────────────────────────────────────────────────────────
  23. SAML 2.0 + SSO             ⏳  Corporate SSO — Keycloak as local IdP (Salesforce/Workday style)
  24. DPoP                        ⏳  Banking-grade — binds access token to client key pair (FAPI)
  25. SCIM                        ⏳  Auto-provision/deprovision users from company directory
  26. Zero Trust Architecture     ⏳  Never trust the network — verify every request every time

Next immediate step: Item 1 — Silent Token Refresh
→ Backend RefreshTokenCommandHandler already complete
→ Axios interceptor partially set up in authClient.ts
→ Wire up: intercept 401 → call POST /api/auth/refresh → retry original request
```

### Previous: Phase 12.7 — gRPC Service-to-Service Communication COMPLETE! ✅
```
Phase 12.7 — gRPC Service-to-Service Communication COMPLETE! ✅

EShop.Contracts/Protos/customer.proto:
   → Defines CustomerGrpc service (Unary call — request/response, not streaming)
   → GetCustomerRequest { id: string }
   → CustomerResponse { id, email, fullName, found: bool }
   → Protobuf binary serialisation — 5-10x smaller payload than JSON

Customer.API — gRPC Server:
   → CustomerGrpcService.cs — inherits CustomerGrpc.CustomerGrpcBase (generated stub)
   → Calls existing GetCustomerByIdQuery via MediatR — ZERO new business logic
   → Clean Architecture preserved: gRPC is just another transport, Core unchanged!
   → Kestrel configured with TWO dedicated listeners:
        5011 → HttpProtocols.Http1   (REST + Swagger — HTTP/1.1 only)
        5022 → HttpProtocols.Http2   (gRPC h2c — HTTP/2 only)
   → UseUrls() clears Aspire-injected ASPNETCORE_URLS; Listen* calls win
   → ListenLocalhost (not ListenAnyIP) — no Windows Firewall admin prompt!

Ordering.Infrastructure — gRPC Client:
   → CustomerGrpcClient.cs — implements ICustomerServiceClient (same interface!)
   → PlaceOrderCommandHandler has ZERO changes — Core is fully transport-agnostic
   → AddGrpcClient<CustomerGrpc.CustomerGrpcClient> registered via DI
   → Address: "http://_grpc.customer-api" — Aspire named-endpoint SD syntax

AppHost.cs:
   → Customer.API now has TWO endpoints registered:
        "http"  port 5011, IsProxied=false — REST
        "grpc"  port 5022, IsProxied=false, UriScheme="http" — gRPC
   → IsProxied=false bypasses Aspire's HTTP/1.1-only YARP reverse proxy
   → Aspire injects services__customer-api__grpc__0=http://localhost:5022
     which the "_grpc.customer-api" service discovery pattern resolves to

Root Cause Diagnosed (HTTP_1_1_REQUIRED):
   → Http1AndHttp2 on ONE port over plain http:// = impossible for gRPC
   → Without TLS there is no ALPN negotiation — Kestrel defaults to HTTP/1.1
   → gRPC client sends HTTP/2 prior-knowledge frames → server responds HTTP_1_1_REQUIRED
   → Fix: dedicated HTTP/2-only port per Microsoft docs recommendation
   → In production (HTTPS): single port with Http1AndHttp2 works via ALPN automatically

End-to-End Test PASSED ✅ (201 Created confirmed):
   POST /api/orders (Ordering.API:5012) →
   CustomerGrpcClient resolves http://_grpc.customer-api → http://localhost:5022 →
   HTTP/2 gRPC call → CustomerGrpcService → GetCustomerByIdQuery via MediatR →
   CustomerResponse { email="john@test.com", found=true } returned as Protobuf →
   Order saved to SQL Server with customerEmail populated →
   201 Created ✅
   [GRPC CLIENT] Customer found: john@test.com (ordering-api console) ✅
   [GRPC SERVER] Customer found: john@test.com (customer-api console) ✅

Key Architecture Decisions — Phase 12.7:
→ gRPC requires HTTP/2 — always! No HTTP/1.1 fallback exists in the protocol
→ h2c (HTTP/2 cleartext) needs Http2-ONLY port — Http1AndHttp2 without TLS = broken
→ Production: HTTPS + ALPN allows single port Http1AndHttp2 (no code change needed!)
→ IsProxied=false is MANDATORY for gRPC in Aspire dev mode — YARP proxy is HTTP/1.1 only
→ "_endpointName.serviceName" = Aspire SD syntax to select a named endpoint by name
→ ICustomerServiceClient interface untouched — swap from HTTP to gRPC = one DI line!
→ UseUrls() + ListenLocalhost() = full Kestrel control, no Aspire port injection conflicts
→ Internal service auth: NONE intentionally — API Gateway (JWT) + Istio mTLS in Phase 14

Why unauthenticated ordering is intentional (NOT a bug):
→ Current state: services are exposed directly — learning phase, no gateway yet
→ External JWT layer = API Gateway validates once, services trust gateway
→ Internal mTLS = Istio proves service identity cryptographically (Phase 14/AKS)
→ Adding [Authorize] to every service = duplication, coupling, key rotation nightmare
→ Real pattern: gateway owns auth, services own business logic

### Next: Phase 13 — React Frontend (UI for EShop!)
→ Build React + TypeScript + Vite frontend against running local APIs
→ Azure Static Web Apps for free hosting with CI/CD built in
→ Products listing, Login/Register, Admin dashboard
→ Unblocks Phase 14 (Auth Deep Dive needs a UI to test against!)
→ App Config + Key Vault deferred to Phase 15 (Cloud Deploy) where multi-pod value is real
→ Branch: feature/phase13-react-frontend
```

Phase 12.6b — Async Messaging + Aspire Orchestration Fixes COMPLETE! ✅

EShop.Contracts/Events/OrderPlacedEvent.cs:
   → Shared event: OrderId, CustomerId, CustomerEmail, TotalAmount, PlacedAt
   → List<OrderPlacedItem> (ProductId, ProductName, Quantity, UnitPrice)

Ordering.Infrastructure/Messaging — 3 swappable IEventPublisher implementations:
   → InMemoryEventPublisher      — logs event to console (dev, zero cost)
   → ServiceBusEventPublisher    — Azure Service Bus Topic (learning only, ₹83/2 days)
   → StorageQueueEventPublisher  — manual fan-out to multiple queues (prod, ₹0.03/month)
   → Switch via appsettings.json "Messaging:Provider": "InMemory|ServiceBus|StorageQueue"

Catalog.Core/Interfaces:
   → IOrderPlacedConsumer — consumer contract (Clean Architecture)
   → IProductRepository   — added ReduceStockAsync(productId, quantity) → bool

Catalog.Infrastructure/Messaging — 3 swappable IOrderPlacedConsumer implementations:
   → InMemoryOrderPlacedConsumer      — logs "dev mode", sleeps
   → ServiceBusOrderPlacedConsumer    — Service Bus Topic Subscription processor
   → StorageQueueOrderPlacedConsumer  — polls queue every 5 seconds
   → OrderPlacedBackgroundService     — IHostedService keeping consumer alive

Catalog.Infrastructure/Repositories/ProductRepository:
   → ReduceStockAsync — prevents negative stock, returns false if insufficient

PlaceOrderCommand + Validator:
   → Removed CustomerEmail (fetched from Customer.API, not caller's responsibility!)
   → Added GUID format validation on CustomerId

AppHost.cs — critical orchestration fixes:
   → WithEndpoint("http", e => e.Port=N) — modifies existing endpoint (no duplicate conflict!)
   → WithReference(customerApi)          — injects services__customer-api__http__0 env var
   → WaitFor(customerApi)                — waits for /health before starting Ordering.API
   → Fixed ports: catalog=5010, customer=5011, ordering=5012, identity=5013
   → Dependency map documented in comments — only sync HTTP callers need WithReference+WaitFor

Security fixes — removed UseAuthorization() from Catalog/Customer/Ordering APIs:
   → Internal service calls don't use JWT
   → Production: Istio mTLS in Phase 14 handles service identity

Bugs diagnosed and fixed:
   → 403 Forbidden (error code 1003) — corporate proxy intercepting "customer-api" hostname
     Root cause: missing WithReference() → service discovery not injecting actual port
   → Connection refused (localhost:5011) — startup race condition
     Root cause: missing WaitFor() → Ordering.API started before Customer.API was ready
   → Endpoint conflict — WithHttpEndpoint() creates duplicate; fixed with WithEndpoint()

End-to-End Test PASSED ✅ (201 Created confirmed):
   POST /api/orders →
   CustomerServiceClient GET http://customer-api/api/customers/{id} → 200 OK →
   customerEmail "john@test.com" fetched from Customer.API →
   EF Core INSERT INTO [Orders] confirmed in logs →
   [EVENT PUBLISHED] OrderPlacedEvent { TotalAmount:199.98, Items:[Laptop x2] } →
   201 Created with full order returned ✅

Key Architecture Decisions — Phase 12.6:
→ WithReference only for sync HTTP callers — async messaging needs neither!
→ WaitFor reflects true startup dependency — only add when B must be ready before A calls it!
→ WithEndpoint() modifies existing; WithHttpEndpoint() creates new (causes conflict!)
→ Fixed ports prevent corporate DNS from intercepting non-standard hostnames!
→ UseAuthorization() removed from internal services — infra-level auth in Phase 14 (Istio)!
→ CustomerEmail removed from command — Customer.API is single source of truth!
→ IOrderPlacedConsumer + BackgroundService — Catalog listens without blocking HTTP!

### Next: Phase 12.7 — gRPC (FREE, highest job-market demand in 2025)
→ Add gRPC endpoint to Customer.API (alongside existing REST — side-by-side learning)
→ Ordering.API calls Customer.API via auto-generated gRPC stub (.proto contract)
→ Compare REST vs gRPC: JSON/HTTP1.1 vs Protobuf/HTTP2, performance, contract safety
→ Cost: ₹0 — NuGet packages only, no infrastructure
→ Branch: feature/phase12-grpc
```

Phase 12.6a — HTTP Service-to-Service Communication COMPLETE! ✅

Ordering.Core:
   → Added ICustomerServiceClient interface (Clean Architecture — no HttpClient in Core!)
   → Added CustomerDto (lightweight DTO for customer validation)

Ordering.Infrastructure:
   → Added HttpClients/CustomerServiceClient.cs (Typed HttpClient with detailed logging)
   → Calls GET http://customer-api/api/customers/{id} via Aspire Service Discovery

Ordering.Tests:
   → Mock ICustomerServiceClient added to all tests
   → New test: Handle_ShouldThrow_WhenCustomerNotFound ✅
   → All tests passing ✅

Key Architecture Decisions — Phase 12.6a:
→ Typed HttpClient per calling service — loose coupling, no shared HttpClient lib!
→ ICustomerServiceClient in Core — Infrastructure implements, Core stays clean!
→ URL in appsettings.json — works locally (Aspire) and in Azure (ACA/AKS)!
→ Customer.API owns customer data — Ordering trusts Customer.API as source of truth!

Phase 12.5 — .NET Aspire Orchestration COMPLETE! ✅

EShopMicroservices.ServiceDefaults:
   → AddServiceDefaults() — OpenTelemetry (Tracing, Metrics), Health Checks, Service Discovery
   → MapDefaultEndpoints() — /health and /alive endpoints on every service

EShopMicroservices.AppHost:
   → Orchestrates all 4 microservices as OS processes (no Docker needed!)
   → builder.AddProject<Projects.Catalog_API>("catalog-api")
   → builder.AddProject<Projects.Ordering_API>("ordering-api")
   → builder.AddProject<Projects.Customer_API>("customer-api")
   → builder.AddProject<Projects.Identity_API>("identity-api")

All 4 APIs wired:
   → builder.AddServiceDefaults() added to all 4 Program.cs files
   → app.MapDefaultEndpoints() added to all 4 Program.cs files

Aspire Dashboard:
   → All 4 services Running ✅ with ONE F5 press
   → Health checks, distributed tracing, metrics active
   → Process Mode (no Docker/Podman required)
   → Aspire Dashboard at https://localhost:17222

Key Architecture Decisions made in Phase 12.5:
→ Process Mode: No Docker required — services run as native OS processes!
→ ServiceDefaults: shared config written once, applied to all 4 services!
→ Service Discovery: services resolve by name (http://catalog-api) not hardcoded ports!
→ ASPNETCORE_URLS added to launchSettings.json to fix dashboard startup!

Phase 12.4 — Identity Service COMPLETE! ✅

Phase 12.4 — Identity Service completed:
✅ Identity.Core — Domain layer (clean POCO — NO ASP.NET Identity dependency):
   → ApplicationUser entity (pure POCO: Id, UserName, Email, FirstName, LastName, FullName, CreatedAt, RefreshToken)
   → Interfaces: ITokenService, IAuthRepository
   → CQRS Features (MediatR):
      Commands: RegisterCommand, LoginCommand, RefreshTokenCommand
      Queries:  GetUserByIdQuery, GetAllUsersQuery
   → Domain Events: UserRegisteredEvent
   → FluentValidation: RegisterCommandValidator, LoginCommandValidator
   → ValidationBehavior<TRequest,TResponse>: pipeline validation
   → AssemblyMarker: MediatR + FluentValidation scanning

✅ Identity.Infrastructure — Data layer:
   → AppIdentityUser : IdentityUser (Infrastructure-only, maps to ApplicationUser)
   → AppIdentityDbContext (IdentityDbContext<AppIdentityUser>, EF Core 10)
   → AuthRepository: wraps UserManager + SignInManager + maps to ApplicationUser POCOs
   → JwtTokenService: generates signed JWTs (HS256) + refresh tokens (RandomNumberGenerator)
   → IdentityDataSeeder: seeds Admin + Customer roles + 2 seed users (idempotent)
   → InfrastructureServiceExtensions: registers Identity, EF Core, services
   → EF Core Migration: InitialIdentitySchema
   → FrameworkReference: Microsoft.AspNetCore.App (for SignInManager in class library)

✅ Identity.API — Presentation layer:
   → AuthController: POST /register, POST /login, POST /refresh, GET /me, GET /users (Admin)
   → DTOs: RegisterRequest, LoginRequest, RefreshTokenRequest, AuthResponse, UserDto
   → ExceptionMiddleware: handles ValidationException (400) + unhandled exceptions (500)
   → Program.cs: JWT Bearer auth, Swagger JWT lock icon, MediatR pipeline, FluentValidation
   → Auto-migrate + seed on startup (development only)
   → User Secrets: JwtSettings + ConnectionStrings (local dev, no secrets in git!)

✅ Identity.Tests — 39 unit tests ALL PASSING:
   → RegisterCommandHandlerTests (4 tests)
   → LoginCommandHandlerTests (4 tests)
   → RefreshTokenCommandHandlerTests (4 tests)
   → GetUserByIdQueryHandlerTests (4 tests)
   → GetAllUsersQueryHandlerTests (3 tests)
   → RegisterCommandValidatorTests (9 tests: inline + theory)
   → LoginCommandValidatorTests (5 tests: inline + theory)

Key Architecture Decisions made in Phase 12.4:
→ ApplicationUser = pure POCO in Core — no ASP.NET Identity dependency in Core!
→ AppIdentityUser : IdentityUser lives ONLY in Infrastructure — proper Clean Architecture!
→ AuthRepository maps AppIdentityUser ↔ ApplicationUser — Infrastructure translates!
→ FrameworkReference Microsoft.AspNetCore.App needed for class libraries using SignInManager!
→ JWT + Refresh Token pattern: short-lived access (60 min) + long-lived refresh (7 days)!

Phase 12.3 — Customer Service COMPLETE! ✅

Phase 12.3 — Customer Service completed:
✅ Customer.Core — Domain layer:
   → Entities: Customer, Address (one-to-many relationship)
   → Interfaces: ICustomerRepository, IEventPublisher
   → CQRS Features (MediatR):
      Commands: CreateCustomer, UpdateCustomer, DeleteCustomer
      Queries:  GetCustomerById, GetAllCustomers, GetCustomerByEmail
   → Domain Events: CustomerCreatedEvent, CustomerUpdatedEvent, CustomerDeletedEvent
   → FluentValidation: CreateCustomerCommandValidator, UpdateCustomerCommandValidator
   → ValidationBehavior<TRequest,TResponse>: automatic pipeline validation!
   → AssemblyMarker: used for MediatR + FluentValidation scanning

✅ Customer.Infrastructure — Data layer:
   → CustomerDbContext (SQL Server via EF Core 10)
   → CustomerRepository (CRUD + eager loading of Addresses)
   → CustomerDataSeeder (idempotent — 2 sample customers with addresses)
   → EF Core Migration: InitialCustomerSchema (tables auto-created at startup!)
   → InMemoryEventPublisher (logs to console — no Azure needed for dev!)
   → InfrastructureServiceExtensions

✅ Customer.API — Presentation layer:
   → CustomersController: GET /api/customers, GET /api/customers/{id},
     GET /api/customers/email/{email}, POST /api/customers,
     PUT /api/customers/{id}, DELETE /api/customers/{id}
   → DTOs: CustomerDto, AddressDto
   → ExceptionMiddleware: ValidationException → 400 Bad Request with field errors JSON
   → Program.cs: MigrateAsync() + SeedAsync() run at startup (Development only!)
   → User Secrets: ConnectionStrings:CustomerDb (never in git!)
   → Swagger UI configured

✅ Unit Tests — 20 tests all passing:
   → CreateCustomerCommandHandlerTests (3 tests)
   → UpdateCustomerCommandHandlerTests (4 tests)
   → DeleteCustomerCommandHandlerTests (4 tests)
   → GetCustomerByIdQueryHandlerTests  (3 tests)
   → GetCustomerByEmailQueryHandlerTests (3 tests)
   → GetAllCustomersQueryHandlerTests  (3 tests)
   → Tools: xUnit + Moq + FluentAssertions

✅ Git workflow completed:
   → feature/phase12-customer-service → PR → merged to develop ✅

Key Architecture Decisions made in Phase 12.3:
→ Same Clean Architecture pattern (Core / Infrastructure / API) — fully consistent!
→ Address is a child entity — EF Core one-to-many, eager-loaded always!
→ UpdateCustomer: returns null when not found (no exceptions for 404) — same as Cancel pattern!
→ InMemoryEventPublisher: zero Azure cost in dev — swap to ServiceBus in prod!

Phase 12.2 — Ordering Service COMPLETE! ✅

Phase 12.2 — Ordering Service completed:
✅ Ordering.Core — Domain layer:
   → Entities: Order, OrderItem, OrderStatus (enum: Pending/Confirmed/Shipped/Delivered/Cancelled)
   → Interfaces: IOrderRepository, IEventPublisher
   → CQRS Features (MediatR):
      Commands: PlaceOrder, CancelOrder
      Queries:  GetOrderById, GetOrdersByCustomer, GetAllOrders
   → Domain Events: OrderPlacedEvent, OrderCancelledEvent
   → FluentValidation: PlaceOrderCommandValidator (CustomerEmail, Items required)
   → ValidationBehavior<TRequest,TResponse>: automatic pipeline validation!
   → AssemblyMarker: used for MediatR + FluentValidation scanning

✅ Ordering.Infrastructure — Data layer:
   → OrderingDbContext (SQL Server via EF Core 10)
   → OrderRepository (IOrderRepository implementation)
   → OrderingDataSeeder (idempotent — safe to run multiple times!)
   → EF Core Migration: InitialOrderingSchema (tables auto-created at startup!)
   → InMemoryEventPublisher (logs to console — no Azure needed for dev!)

✅ Ordering.API — Presentation layer:
   → OrdersController: GET /api/orders, GET /api/orders/{id},
     GET /api/orders/customer/{customerId}, POST /api/orders,
     POST /api/orders/{id}/cancel
   → DTOs: OrderDto, OrderItemDto
   → ExceptionMiddleware: ValidationException → 400 Bad Request with field errors JSON
   → Program.cs: MigrateAsync() + SeedAsync() run at startup (Development only!)
   → User Secrets: ConnectionStrings:OrderingDb (never in git!)
   → Swagger UI configured

✅ Unit Tests — 16 tests all passing:
   → PlaceOrderCommandHandlerTests  (3 tests)
   → CancelOrderCommandHandlerTests (4 tests)
   → GetOrderByIdQueryHandlerTests  (3 tests)
   → GetOrdersByCustomerQueryHandlerTests (3 tests)
   → GetAllOrdersQueryHandlerTests  (3 tests)
   → Tools: xUnit + Moq + FluentAssertions

✅ Git workflow completed:
   → feature/phase12-ordering-service → PR → merged to develop ✅

Key Architecture Decisions made in Phase 12.2:
→ Same Clean Architecture pattern as Catalog (Core / Infrastructure / API)!
→ OrderStatus enum guards valid state transitions at the domain level!
→ CancelOrder: returns bool (true=cancelled, false=not found) — no exceptions for 404!
→ PlaceOrder: returns full Order entity immediately after save!
→ InMemoryEventPublisher: zero Azure cost in dev — swap to ServiceBus in prod!

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
| 39 | Split monolith → Order Service (.NET) | 🟢 Free | ✅ Done (Phase 12.2!) |
| 40 | Split monolith → Customer Service (.NET) | 🟢 Free | ✅ Done (Phase 12.3!) |
| 41 | Split monolith → Identity Service (.NET) | 🟢 Free | ✅ Done (Phase 12.4!) |
| 42 | Service-to-service communication (HTTP sync + async messaging) | 🟢 Free | ✅ Done (Phase 12.6!) |
| 42a | gRPC service-to-service (typed contracts, 7x faster than REST) | 🟢 Free | ✅ Done (Phase 12.7!) |
| 43 | Azure App Configuration — central hub (settings + KV refs!) | 🟢 Free | ➡️ Moved to Phase 15 (Cloud Deployment) |
| 44 | Each microservice reads from App Config only (one source!) | 🟢 Free | ➡️ Moved to Phase 15 (Cloud Deployment) |
| 45 | Docker Compose for ALL microservices locally | 🟢 Free | ➡️ Moved to Phase 15 (Cloud Deployment) |
| 46 | .NET Aspire — orchestrate all services locally + dashboard! | 🟢 Free | ✅ Done (Phase 12.5!) |
| 47 | Azure Container Registry (ACR) — private registry for all images! | 🟡 $5 | ➡️ Moved to Phase 15 (Cloud Deployment) |
| 48 | Update CI/CD pipelines → build + push each service image to ACR | 🟢 Free | ➡️ Moved to Phase 15 (Cloud Deployment) |
| 49 | Managed Identity → AKS pulls from ACR (no password needed!) | 🟢 Free | ➡️ Moved to Phase 15 (Cloud Deployment) |

---

### ⚛️ Phase 13 — React Frontend (UI for EShop!) ← IN PROGRESS 🔄
> Build the UI FIRST — provides immediate visible value and unlocks real auth testing!
> Backend is complete. APIs run locally. No new infrastructure needed to start.

**Confirmed Tech Stack:**
```
Core:          React 19.2  +  TypeScript 6  +  Vite 8
Routing:       React Router DOM v7
State:         Redux Toolkit 2.12  (store + slices)
API Calls:     RTK Query  (built into RTK — replaces createAsyncThunk)
Auth calls:    Axios  (JWT interceptor for login/register/refresh only)
Forms:         React Hook Form v7  +  Zod v4  (schema validation)
UI:            Shadcn/ui  +  Tailwind CSS v4  +  Lucide React (icons)
Charts:        Recharts  (order stats, product analytics)
Dates:         DayJS
Hosting:       Azure Static Web Apps  (FREE tier)

Builds on FinanceTracker knowledge:
  ✅ RTK (createSlice, configureStore, typed hooks) — already know
  ✅ React Router DOM v7 — already know
  ✅ Axios interceptor — already know
  ✅ Recharts — already know
  NEW: RTK Query (upgrade from createAsyncThunk)
  NEW: Shadcn/ui + Tailwind v4 (instead of MUI)
  NEW: React Hook Form + Zod (instead of useReducer manual forms)
  NEW: React 19 hooks — use(), useActionState, useOptimistic, useTransition
```

**Hooks used in this app:**
```
React built-in:   useState, useEffect, useReducer, useMemo, useCallback, useRef
React 19 new:     useTransition, useActionState, useOptimistic, use()
React Router:     useNavigate, useParams, useLocation
Redux:            useAppSelector, useAppDispatch
RTK Query:        useGetProductsQuery, usePlaceOrderMutation (auto-generated)
RHF:              useForm, useWatch
Custom built:     useAuth, useCart, useDebounce
```

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 50 | Vite 8 + React 19 + TypeScript 6 project scaffold | 🟢 Free | ✅ Done |
| 51 | Tailwind CSS v4 setup (one CSS import — no config file!) | 🟢 Free | ✅ Done |
| 52 | Shadcn/ui init + core components (Button, Card, Input, Table, Dialog) | 🟢 Free | ✅ Done |
| 53 | Folder architecture (api/, features/, pages/, components/, hooks/, types/, lib/) | 🟢 Free | ✅ Done |
| 54 | Redux store + typed hooks (useAppSelector, useAppDispatch) | 🟢 Free | ✅ Done |
| 55 | RTK Query — createApi, baseQuery with JWT header, endpoints | 🟢 Free | ✅ Done |
| 56 | Axios auth client — JWT interceptor for login/register/refresh | 🟢 Free | ✅ Done |
| 57 | React Router DOM v7 — routes, nested layouts, ProtectedRoute | 🟢 Free | ✅ Done |
| 58 | Auth slice — login, logout, token + user in Redux | 🟢 Free | ✅ Done |
| 59 | Zod schemas + React Hook Form (replaces useReducer manual forms) | 🟢 Free | ⏭️ Skipped — forms use plain useState (works fine, RHF optional upgrade later) |
| 60 | Login page — wider card (700px), placeholders, dark text fix | 🟢 Free | ✅ Done |
| 61 | Register page — wider card (700px), all field placeholders | 🟢 Free | ✅ Done |
| 62 | Products listing — RTK Query + search debounce + category filter + pagination + Picsum images | 🟢 Free | ✅ Done |
| 63 | Product detail page — image, description, reviews, qty selector, Add to Cart | 🟢 Free | ✅ Done |
| 64 | Admin page — product CRUD with Create/Edit/Delete dialog (Admin only) | 🟢 Free | ✅ Done |
| 65 | Categories page — admin CRUD table | 🟢 Free | ⏳ Optional — not built yet |
| 66 | Cart — Redux slice, qty controls, remove, subtotal, live badge in Sidebar + TopBar | 🟢 Free | ✅ Done |
| 67 | Checkout page — shipping address + customer lookup + PlaceOrder API call | 🟢 Free | ✅ Done |
| 68 | Orders page + Order Detail page — list, status badges, items, Cancel Order | 🟢 Free | ✅ Done |
| 69 | Customers page — admin-only list with search filter | 🟢 Free | ✅ Done |
| 70 | Dashboard — stat cards (Products/Orders/Customers/Revenue) + Recent Orders table | 🟢 Free | ✅ Done |
| 71 | Dark mode toggle — useDarkMode hook, localStorage, Moon/Sun in TopBar | 🟢 Free | ✅ Done |
| 72 | CORS config on all 4 backend APIs | 🟢 Free | ✅ Done |
| 73 | React 19 new hooks — useTransition, useActionState, useOptimistic | 🟢 Free | ⏳ Optional deep dive later |
| 74 | Custom hooks — useAuth, useCart, useDebounce, useDarkMode | 🟢 Free | ✅ Done |
| 75 | Azure Static Web Apps deploy (FREE — CI/CD from GitHub) | 🟢 Free | 🔄 **NEXT** |
| — | Profile page — avatar, account info, role badges, order stats, actions | 🟢 Free | ✅ Done (extra!) |
| — | Lenovo Vantage UI — 165px sidebar, solid blue active, flat borders, Segoe UI | 🟢 Free | ✅ Done (extra!) |
| — | Font scaling — rem-based Tailwind classes, 20px base in index.css | 🟢 Free | ✅ Done (extra!) |

---

### 🔑 Phase 14 — Authentication Deep Dive (with React UI!)
> UI must exist before Auth makes sense to implement and test end-to-end!
> Logical sequence: Concepts → Already built → Standard flows → Enterprise → Advanced → Passwordless
> Total cost: £0 — all tools free tier or local

**Already done in Identity.API (✅):**
```
JWT Authentication       — email + password → JWT token
Refresh Token            — silent re-auth without re-login
Role-based auth          — Admin / User roles in JWT claims
```

**Tools used in Phase 14:**
```
Auth0 free tier          — OAuth 2.0, OIDC, Social Logins, Refresh Tokens, M2M
Azure AD B2C free tier   — Consumer identity, Social Logins via Azure
Entra ID free tier       — Enterprise employee login
Keycloak (local)         — SAML 2.0, SSO — runs as local Java app, zero cost
ASP.NET Core Identity    — 2FA built-in (GenerateTwoFactorTokenAsync)
MailKit NuGet            — sends OTP email (Gmail SMTP free)
Fido2.NET NuGet          — Passkeys / WebAuthn implementation
Local self-signed certs  — mTLS between microservices
```

| # | Auth Method | Tool | Cost | Status |
|---|------------|------|------|--------|
| 76 | **Session vs Token** — concept, when to use which | Theory | 🟢 Free | ⏳ |
| 77 | **OAuth 2.0 concepts** — flows, tokens, scopes, grants | Theory | 🟢 Free | ⏳ |
| 78 | **OAuth 2.0 — Authorization Code + PKCE** — React SPA login | Auth0 | 🟢 Free | ⏳ |
| 79 | **OAuth 2.0 — Client Credentials** — microservice to microservice | Auth0 | 🟢 Free | ⏳ |
| 80 | **OAuth 2.0 — Refresh Token flow** — silent re-auth | Auth0 | 🟢 Free | ⏳ |
| 81 | **OAuth 2.0 — Device Authorization** — CLI / smart TV / IoT | Auth0 | 🟢 Free | ⏳ |
| 82 | **OAuth 2.1** — updated spec (PKCE mandatory, implicit removed) | Theory | 🟢 Free | ⏳ |
| 83 | **OpenID Connect (OIDC)** — ID Token, userinfo endpoint, discovery | Auth0 | 🟢 Free | ⏳ |
| 84 | **Social Logins** — Google + GitHub via Auth0 | Auth0 | 🟢 Free | ⏳ |
| 85 | **2FA — Email OTP** — Identity built-in + MailKit (no Google Authenticator!) | MailKit | 🟢 Free | ⏳ |
| 86 | **Magic Links** — passwordless email login (HMAC signed token) | Identity.API | 🟢 Free | ⏳ |
| 87 | **Azure AD B2C** — consumer identity, Azure native OIDC | Azure free | 🟢 Free | ⏳ |
| 88 | **Azure AD B2C + React** — replace Auth0 with AD B2C | Azure free | 🟢 Free | ⏳ |
| 89 | **Social Logins via Azure AD B2C** — Google + Microsoft | Azure free | 🟢 Free | ⏳ |
| 90 | **Entra ID (Azure AD)** — enterprise employee login (Login with Microsoft) | Azure free | 🟢 Free | ⏳ |
| 91 | **SAML 2.0 + SSO** — corporate SSO, SP-initiated flow | Keycloak local | 🟢 Free | ⏳ |
| 92 | **API Key Authentication** — developer / service account access | Identity.API | 🟢 Free | ⏳ |
| 93 | **Passkeys / WebAuthn (FIDO2)** — fingerprint/Face ID, no password | Fido2.NET | 🟢 Free | ⏳ |
| 94 | **Mutual TLS (mTLS)** — certificate-based service-to-service auth | Local certs | 🟢 Free | ⏳ |
| 95 | **Zero Trust concept** — never trust, always verify architecture | Theory | 🟢 Free | ⏳ |
| 96 | **Risk-based / Adaptive Auth** — new device triggers extra verification | Identity.API | 🟢 Free | ⏳ |

---

### ☁️ Phase 15 — Cloud Deployment (App Config + Multi-Env + AKS)
> Deploy the COMPLETE, AUTHENTICATED app (UI + Auth + Services) in one go!
> App Config and Key Vault are wired here — where multi-pod value is REAL!

**Phase 15a — Containerization & Config**

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 70 | Dockerfiles for all 4 microservices (multi-stage builds) | 🟢 Free | ⏳ |
| 71 | Build images via GitHub Actions (no local Docker needed!) | 🟢 Free | ⏳ |
| 72 | Azure Container Registry (ACR) — private image registry | 🟡 $5 | ⏳ |
| 73 | Azure App Configuration — central config hub (settings + KV refs + feature flags) | 🟢 Free | ⏳ |
| 74 | Key Vault references — secrets in KV, App Config holds pointers | 🟢 Free | ⏳ |
| 75 | Each microservice reads from App Config only (one source of truth!) | 🟢 Free | ⏳ |
| 76 | Managed Identity → AKS pulls from ACR + reads Key Vault (no passwords!) | 🟢 Free | ⏳ |

**Phase 15b — Multi-Environment Strategy**

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 77 | Environment strategy concept (DEV → STAGING → PROD) | 🟢 Free | ⏳ |
| 78 | App Config labels (dev / staging / prod) — same store, different values | 🟢 Free | ⏳ |
| 79 | Pipeline with approval gates (GitHub Actions environments) | 🟢 Free | ⏳ |
| 80 | Promote build: local → STAGING → PROD (swap!) | 🟢 Free | ⏳ |

**Phase 15c — Kubernetes (AKS)**

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 81 | Azure Container Apps (stepping stone before AKS!) | 🟡 $1-3 | ⏳ |
| 82 | Kubernetes fundamentals (pods, deployments, services) | 🟢 Free | ⏳ |
| 83 | Azure Kubernetes Service (AKS) cluster setup | 🟡 ~$5 | ⏳ |
| 84 | Deploy ALL microservices + React SWA to AKS | 🟡 ~$5 | ⏳ |
| 85 | Kubernetes ConfigMaps + Secrets (CSI + Key Vault) | 🟢 Free | ⏳ |
| 86 | Horizontal Pod Autoscaler (scale each service independently!) | 🟢 Free | ⏳ |
| 87 | AKS Ingress Controller (NGINX — one entry point for all!) | 🟢 Free | ⏳ |
| 88 | Azure Front Door (global entry point for AKS!) | 🔴 Delete! | ⏳ |
| 89 | Helm Charts (package each microservice deployment) | 🟢 Free | ⏳ |
| 90 | CI/CD per microservice → auto deploy to AKS | 🟢 Free | ⏳ |
| 91 | Delete AKS cluster after learning | 🔴 Delete! | ⏳ |

---

### 🤖 Phase 16 — Search & AI (full app ready!)
| # | Topic | Cost | Status |
|---|-------|------|--------|
| 92 | Azure Cognitive Search (product search + fuzzy!) | 🟢 Free | ⏳ |
| 93 | Azure OpenAI — GPT-4 (product recommendations) | 🟡 $1-2 | ⏳ |
| 94 | AI Chatbot in React (customer support!) | 🟡 $1-2 | ⏳ |
| 95 | AI product description generator | 🟡 $1-2 | ⏳ |

---

### 🏗️ Phase 17 — Infrastructure as Code (Terraform)
> Do at the END! Know all resources first, then automate everything!
> Real world: Build manually → understand → then codify!
> Nobody clicks portal in production! Everything is code!

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 96 | Terraform Fundamentals (providers, state, plan, apply) | 🟢 Free | ⏳ |
| 97 | terraform import → import all existing resources! | 🟢 Free | ⏳ |
| 98 | Terraform modules for each microservice (SQL, KV, ACR) | 🟢 Free | ⏳ |
| 99 | Terraform — AKS cluster + networking + RBAC | 🟢 Free | ⏳ |
| 100 | Terraform — Full EShop infra in one command! | 🟢 Free | ⏳ |
| 101 | Terraform remote state (Azure Blob Storage backend) | 🟢 Free | ⏳ |
| 102 | Terraform workspaces (DEV / STAGING / PROD configs) | 🟢 Free | ⏳ |

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

### 🤖 Phase 29b — Agentic AI (.NET + Azure)
> The 2025-2026 paradigm shift: from single chatbots → networks of autonomous agents that
> perceive, reason, plan, act, and remember. Principal Cloud & AI Architects design agent
> SYSTEMS — not just individual models. This is the highest-demand AI skill in 2026.
>
> Learning order: Concepts → MCP (tools) → Single Agent → Multi-Agent → Azure runtime → EShop integration

#### What makes AI "Agentic"?
```
Traditional AI:   User asks question → LLM generates answer → DONE
                  One turn. Passive. No side effects.

Agentic AI:       User gives GOAL → Agent perceives context
                              → Agent reasons and plans steps
                              → Agent calls tools / other agents
                              → Agent observes results
                              → Agent repeats until goal is met
                  Multi-turn. Autonomous. Takes real actions.

EShop example:
  User: "Find me the cheapest laptop under ₹50,000, check if it's in stock,
         and place an order if you find one"

  Traditional: Returns text listing laptops (no action taken)

  Agentic:     Step 1 → ProductSearchAgent.SearchProducts("laptop", maxPrice=50000)
               Step 2 → InventoryAgent.CheckStock(productId)   ← calls EShop API
               Step 3 → PricingAgent.ApplyDiscounts(productId) ← calls EShop API
               Step 4 → OrderAgent.PlaceOrder(customerId, productId) ← calls EShop API
               Result → "Ordered Lenovo IdeaPad for ₹47,999. Order ID: ORD-001"
               The agent DID the task — not just described it!
```

| # | Topic | Cost | Status |
|---|-------|------|--------|
| 163a | Agentic AI concepts — Perception→Reasoning→Planning→Action→Memory loop | 🟢 Free | ⏳ |
| 163b | Agent patterns — ReAct, Tool Use, Reflection, Plan-and-Execute, Multi-Agent | 🟢 Free | ⏳ |
| 163c | MCP — Model Context Protocol (Anthropic/industry standard — "USB-C for AI tools") | 🟢 Free | ⏳ |
| 163d | Build custom MCP Server in C# — expose EShop tools (GetProducts, CheckInventory, PlaceOrder) | 🟡 $1-2 | ⏳ |
| 163e | Connect Claude/GPT-4 to EShop via MCP — any AI model, same server, zero integration changes | 🟡 $1-2 | ⏳ |
| 163f | Semantic Kernel ChatCompletionAgent — single agent with SK plugins + memory | 🟡 $1-2 | ⏳ |
| 163g | Semantic Kernel AgentGroupChat — multiple agents collaborating (debate, correct, hand off) | 🟡 $1-2 | ⏳ |
| 163h | Azure AI Foundry Agent Service — managed agent runtime, persistent threads, file search, code interpreter | 🟡 $2-3 | ⏳ |
| 163i | Multi-agent orchestration — Orchestrator + Specialists: ShoppingOrchestrator → Product + Pricing + Inventory + Order agents | 🟡 $2-3 | ⏳ |
| 163j | A2A Protocol (Agent-to-Agent, Google 2025) — agents discover + call other agents over HTTP | 🟢 Free | ⏳ |
| 163k | EShop.AI.Agent microservice — Clean Architecture agent service, REST + gRPC endpoints, called by other microservices like any other service | 🟡 $2-3 | ⏳ |

#### Architecture — how EShop.AI.Agent fits the microservices system
```
                     ┌─────────────────────────────────────────────┐
                     │           EShop.AI.Agent Service             │
                     │                                              │
                     │  ┌──────────────────────────────────────┐   │
                     │  │      ShoppingOrchestratorAgent        │   │
                     │  │  (Semantic Kernel AgentGroupChat)     │   │
                     │  └────────┬──────────┬──────────┬───────┘   │
                     │           │          │          │            │
                     │  ┌────────▼─┐ ┌──────▼──┐ ┌───▼───────┐   │
                     │  │ Product  │ │ Pricing │ │   Order   │   │
                     │  │  Agent   │ │  Agent  │ │   Agent   │   │
                     │  └────┬─────┘ └────┬────┘ └─────┬─────┘   │
                     └───────┼────────────┼─────────────┼─────────┘
                             │            │             │
                     gRPC ───▼────        │      gRPC ──▼────
                     Catalog.API    (internal calc)  Ordering.API
                     Customer.API                    Customer.API

Clean Architecture applies here too:
  AI.Agent.Core          → IShoppingAgent, IProductSearchTool, IOrderTool (interfaces)
  AI.Agent.Infrastructure→ SK Agents, MCP Server, Azure AI Foundry client
  AI.Agent.API           → REST /api/agent/chat, gRPC AgentService
```

#### Key decisions for this phase
```
→ MCP first — learn the tool protocol BEFORE building agents (agents need tools!)
→ Single agent before multi-agent — master ChatCompletionAgent first
→ Azure AI Foundry = production runtime; SK = local dev and fine control
→ IShoppingAgent in Core — clean architecture means swapping SK ↔ AutoGen ↔ Foundry
   without touching business logic, same as swapping HTTP ↔ gRPC in Phase 12.7!
→ A2A = future-proof — as agent ecosystems grow, services will expose A2A endpoints
   the same way they expose REST and gRPC today
→ Cost: most is $1-3/run during learning — use Azure OpenAI with token limits!
```

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
Total Topics   →  187  (+11 Agentic AI topics added Phase 29b)
🟢 Free        →  128 topics
🟡 Cheap       →   55 topics (~$10-15/month)
🔴 Delete!     →    4 topics (create → learn → delete)
─────────────────────────────────────────────
Order          →  Microservices → React Frontend → Auth (Auth0 → AD B2C!)
               →  Cloud Deploy (Dockerfiles + App Config + Multi-env + AKS)
               →  Search & AI → Terraform LAST
               →  AI Concepts → Python → Classical ML → Deep Learning
               →  HuggingFace → GenAI Concepts → GenAI Implementation
               →  Prompt Engineering → RAG → Azure OpenAI
               →  Semantic Kernel → AGENTIC AI (MCP + Agents + A2A) → Azure AI Services → MLOps
Key decisions  →  Auth0 FREE (easy start!) → AD B2C (Azure native!)
               →  Terraform at END (know all resources first!)
               →  Build manually → understand → automate! ✅
               →  AI: Concept first → Python → ML → GenAI (logical order!) ✅
               →  GenAI: Concepts phase → Implementation phase (dedicated!) ✅
               →  AI in C#/.NET for production (Semantic Kernel!) ✅
               →  Agentic AI: MCP tools first → single agent → multi-agent → Azure Foundry! ✅
               →  IShoppingAgent in Core = Clean Architecture for AI (swap SK↔Foundry freely!) ✅
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
| 27 | Phase 12.5 — .NET Aspire Orchestration (ServiceDefaults + AppHost + Dashboard) | ✅ Done |
| 28 | Phase 12.6 — Service-to-Service Communication (HTTP + Messaging + Aspire fixes) | ✅ Done |
| 28a | Phase 12.7 — gRPC (typed contracts, Protobuf, HTTP/2 — highest market demand) | ✅ Done |
| 29 | Phase 13 — React Frontend (Vite + TypeScript + Redux + RTK Query + full UI) | ✅ Done (Static Web Apps deploy = next!) |
| 30 | Phase 14 — Auth Deep Dive (Auth0 → AD B2C → SAML → mTLS!) | ⏳ |
| 31 | Phase 15 — Cloud Deployment (Dockerfiles + App Config + Multi-Env + AKS) | ⏳ |
| 32 | Phase 16 — Search & AI (Cognitive Search, OpenAI, Chatbot!) | ⏳ |
| 33 | Phase 17 — Terraform / IaC (automate everything at the end!) | ⏳ |
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
| 44b | Phase 29b — Agentic AI (MCP + SK Agents + Azure AI Foundry + A2A + EShop.AI.Agent microservice!) | ⏳ |
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
