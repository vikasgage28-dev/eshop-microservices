# EShop Learning Project - Technical Bookshelf (Reference)

> Detailed historical logs, deep-dive notes, and the full long-term roadmap.
> Companion to Progress.md. Progress.md = active session context (where we are + next).
> This file = reference only. Open it when deep detail on a past phase is needed.

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

### Phase 14 — Authentication Deep Dive — IN PROGRESS ⬅️
```
Philosophy:
→ API Gateway owns auth validation — services trust the gateway (correct microservice pattern)
→ Individual services do NOT add [Authorize] — gateway handles it (like Netflix, Uber)
→ Each auth mode implemented in Identity.API + React frontend — end-to-end working demo
→ No Azure needed until items 20-22 — months of learning first!

Completed so far:
✅ Item 1 — Silent Token Refresh    (baseQueryWithReauth.ts — RTK Query 401 interceptor)
✅ Item 2 — Refresh Token Rotation  (backend already had it, frontend stores new token)
✅ Item 3 — JWT RS256 Asymmetric    (private.pem signs, public.pem verifies)
✅ Item 4 — 2FA Email OTP           (MailKit + Gmail SMTP + TOTP math, 2-min expiry)
✅ Item 9 — OAuth 2.0 + PKCE       (Auth0 + Google social login, AspNetUserLogins tracking)
✅ Item 11 — Social Logins         (Google + GitHub — separate buttons, Auth0 connection routing)

Complete Authentication Sequence:
─────────────────────────────────────────────────────────────────
🟢 No Azure Needed — Implement In Order
─────────────────────────────────────────────────────────────────
  1.  Silent Token Refresh        ✅  COMPLETE — baseQueryWithReauth.ts wraps all RTK Query APIs
  2.  Refresh Token Rotation      ✅  COMPLETE — already in backend, frontend now stores new token
  3.  JWT RS256 (Asymmetric)      ✅  COMPLETE — private.pem signs, public.pem verifies
  4.  2FA — Email OTP             ✅  COMPLETE — MailKit + Gmail SMTP + TOTP math, 2-min expiry
  5.  2FA — TOTP (Authenticator)  ⏳  QRCoder + OtpNet, Google Authenticator / Authy
  6.  SMS OTP                     ⏳  Twilio / MSG91, OTP on mobile number
  7.  Magic Links                 ⏳  Passwordless — HMAC-signed link emailed to user (Slack/Notion style)
  8.  Step-up Auth                ⏳  Re-verify for sensitive actions (e.g. cancel order > ₹10,000)
  9.  OAuth 2.0 + PKCE            ✅  COMPLETE — Auth0 + Google login, AspNetUserLogins tracking
  10. OIDC (OpenID Connect)       ⏳  id_token + userinfo endpoint + discovery doc
  11. Social Logins               ✅  COMPLETE — Google + GitHub via Auth0 connection routing
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

─────────────────────────────────────────────────────────────────
Item 1 — Silent Token Refresh ✅ COMPLETE
─────────────────────────────────────────────────────────────────
Problem: RTK Query uses native fetch (not Axios) → Axios interceptors don't work
Solution: Custom BaseQueryFn wrapper (RTK Query pattern)

Files created/changed:
→ eshop-frontend/src/api/baseQueryWithReauth.ts  (NEW)
     createBaseQueryWithReauth(baseUrl) factory function
     Intercepts 401 → POST /api/auth/refresh → retries original request
     Module-level refreshPromise singleton prevents duplicate refresh calls (race condition fix)
     On refresh fail → dispatch(logout()) → user sent to login page
→ eshop-frontend/src/features/auth/authSlice.ts
     Added updateTokens action — updates token + refreshToken ONLY, preserves userId/email/roles
→ eshop-frontend/src/api/catalogApi.ts     — swapped fetchBaseQuery → createBaseQueryWithReauth
→ eshop-frontend/src/api/customerApi.ts   — same swap
→ eshop-frontend/src/api/orderingApi.ts   — same swap
→ eshop-frontend/src/api/identityApi.ts   — same swap

Race condition solution:
→ 5 simultaneous 401s → only 1 refresh call fires → all 5 await same promise → all retry ✅

─────────────────────────────────────────────────────────────────
Item 2 — Refresh Token Rotation ✅ COMPLETE (backend already had it)
─────────────────────────────────────────────────────────────────
Backend (already existed):
→ RefreshTokenCommandHandler.cs — GenerateRefreshToken() every call → UpdateRefreshTokenAsync()
→ Old refresh token overwritten in DB — cannot be reused
→ Stolen old token → GetByRefreshTokenAsync returns null → 401

Frontend (completed via Item 1):
→ updateTokens() stores NEW refreshToken from refresh response into Redux + localStorage
→ Old token gone from both DB and client simultaneously

Not implemented (advanced, deferred):
→ Reuse detection (token family / history table) — requires separate DB table

─────────────────────────────────────────────────────────────────
Item 3 — JWT RS256 Asymmetric Signing ✅ COMPLETE
─────────────────────────────────────────────────────────────────
Why RS256 over HS256:
→ HS256 = 1 shared secret → anyone who has it can forge tokens
→ RS256 = private key signs (Identity.API only) + public key verifies (anyone)
→ Services/Gateway only need public key → private key never leaves Identity.API
→ Industry standard for microservices (Auth0, Google, Microsoft all use RS256)
→ Required for API Gateway integration (Phase 15)

Files changed:
→ EShopMicroservices/Identity.Infrastructure/Services/JwtTokenService.cs
     Removed: SymmetricSecurityKey + HmacSha256
     Added: RSA.Create() + ImportFromPem(privateKeyPem) + RsaSha256
→ EShopMicroservices/Identity.API/Program.cs
     Removed: SymmetricSecurityKey + SecretKey from config
     Added: RSA.Create() + ImportFromPem(publicKeyPem) + RsaSecurityKey
→ EShopMicroservices/Identity.API/appsettings.json
     Removed: SecretKey
     Added: PrivateKeyPath = "private.pem", PublicKeyPath = "public.pem"
→ .gitignore — added **/private.pem (NEVER commit private key!)

Key files on disk (not in git):
→ Identity.API/private.pem — RSA 2048-bit private key (signs tokens)
→ Identity.API/public.pem  — RSA 2048-bit public key (verifies tokens, shareable)

Verification: paste token at jwt.io → header shows "alg": "RS256" (was "HS256")

Key architecture decisions:
→ Services stay open (no [Authorize]) — API Gateway validates once at perimeter
→ public.pem can be shared with API Gateway / other services safely
→ generate-keys.ps1 (in .gitignore) — regenerate keys anytime with: pwsh -File generate-keys.ps1

─────────────────────────────────────────────────────────────────
Item 4 — 2FA Email OTP ✅ COMPLETE
─────────────────────────────────────────────────────────────────
How it works (TOTP — no DB table for OTP codes):
→ OTP = HMAC(SecurityStamp + CurrentTimeWindow) — pure math, nothing stored
→ SecurityStamp already exists in AspNetUsers — changes on password change → invalidates pending OTP
→ TwoFactorEnabled column already in AspNetUsers — no migration needed
→ Backend recomputes same HMAC on verify — if matches → valid, if time window passed → invalid
→ 2-minute expiry (set via DataProtectionTokenProviderOptions.TokenLifespan)

Login flow change:
→ Password correct + TwoFactorEnabled=1 → return { requires2FA: true, userId } (NO JWT yet)
→ Frontend redirects to /verify-otp → calls POST /send-otp → email sent
→ User enters 6-digit code → POST /verify-otp → code verified → JWT issued NOW

Backend files created/changed:
→ Identity.Core/Interfaces/IEmailService.cs                          (NEW)
     SendOtpEmailAsync(toEmail, toName, otpCode) — interface only, no MailKit dependency in Core
→ Identity.Core/Interfaces/IAuthRepository.cs
     Added: GetTwoFactorEnabledAsync, SetTwoFactorEnabledAsync, GenerateTwoFactorTokenAsync, VerifyTwoFactorTokenAsync
→ Identity.Core/Entities/ApplicationUser.cs
     Added: TwoFactorEnabled property (maps from AspNetUsers.TwoFactorEnabled)
→ Identity.Core/Features/Auth/Commands/Login/LoginCommand.cs
     Added: Requires2FA bool to LoginResult
→ Identity.Core/Features/Auth/Commands/Login/LoginCommandHandler.cs
     Added: GetTwoFactorEnabledAsync check → early return with Requires2FA=true (no JWT)
→ Identity.Core/Features/Auth/Commands/SendOtp/                       (NEW folder)
     SendOtpCommand.cs + SendOtpCommandHandler.cs
     Handler: GenerateTwoFactorTokenAsync → SendOtpEmailAsync
→ Identity.Core/Features/Auth/Commands/VerifyOtp/                     (NEW folder)
     VerifyOtpCommand.cs + VerifyOtpCommandHandler.cs
     Handler: VerifyTwoFactorTokenAsync → if valid → GenerateAccessToken + GenerateRefreshToken
→ Identity.Core/Features/Auth/Commands/Enable2FA/                     (NEW folder)
     Enable2FACommand.cs + Enable2FACommandHandler.cs
     Handler: SetTwoFactorEnabledAsync(enabled)
→ Identity.Infrastructure/Services/MailKitEmailService.cs              (NEW)
     Connects to Gmail SMTP (smtp.gmail.com:587 StartTls) using App Password
     Sends branded HTML email with large OTP code
→ Identity.Infrastructure/Repositories/AuthRepository.cs
     Implemented 4 new 2FA methods using UserManager built-ins
     Updated ToModel() mapping to include TwoFactorEnabled
→ Identity.Infrastructure/Extensions/InfrastructureServiceExtensions.cs
     DataProtectionTokenProviderOptions.TokenLifespan = 2 minutes
     Registered IEmailService → MailKitEmailService (Scoped)
→ Identity.API/Controllers/AuthController.cs
     POST /api/auth/send-otp        — public (no JWT yet)
     POST /api/auth/verify-otp      — public (no JWT yet)
     POST /api/auth/toggle-2fa      — [Authorize] (userId from JWT claim)
     GET  /api/auth/2fa-status      — [Authorize]
     Login endpoint: returns Requires2FA=true branch
→ Identity.API/DTOs/AuthDtos.cs
     Added: Requires2FA to AuthResponse, SendOtpRequest, VerifyOtpRequest, Toggle2FARequest
→ Identity.API/appsettings.json
     Added: EmailSettings (SmtpHost, SmtpPort, FromName, FromEmail, AppPassword)
→ User Secrets: EmailSettings:FromEmail + EmailSettings:AppPassword (never in git!)

Frontend files created/changed:
→ eshop-frontend/src/api/authClient.ts
     Added: requires2FA to AuthResponse type
     Added: sendOtp(), verifyOtp(), toggle2FA(), get2FAStatus() API methods
→ eshop-frontend/src/features/auth/authSlice.ts
     Added: requires2FA, pending2FAUserId, pending2FAEmail to AuthState
     Login fulfilled: if requires2FA → store userId temporarily, no token
     Added: clear2FAPending action
→ eshop-frontend/src/hooks/useAuth.ts
     Exposed: requires2FA, pending2FAUserId, pending2FAEmail, completeLogin, clear2FAPending
→ eshop-frontend/src/pages/auth/LoginPage.tsx
     Added: useEffect watching requires2FA → navigate to /verify-otp
→ eshop-frontend/src/pages/auth/VerifyOtpPage.tsx                     (NEW)
     Auto-sends OTP on mount (useRef guard prevents React Strict Mode double-send)
     6 individual digit inputs — auto-advance + paste support
     Single countdown = OTP expiry (2 min, matches email and backend)
     Resend silently unlocks after 60s (no separate timer shown — standard UX)
     Code expired state: Verify button disabled, resend highlighted
→ eshop-frontend/src/routes/AppRouter.tsx
     Added: /verify-otp public route → VerifyOtpPage
→ eshop-frontend/src/pages/profile/ProfilePage.tsx
     Added: 2FA security card with toggle switch (loads status on mount, calls toggle-2fa)

Key bugs fixed during implementation:
→ Duplicate email: React 18 Strict Mode runs effects twice — fixed with useRef hasSentOtp guard
→ Two confusing timers: removed separate resend countdown — one timer only (OTP expiry = 2 min)
→ Token lifespan: reduced from 5 min to 2 min — email and UI countdown both updated

Key architecture decisions:
→ TOTP (RFC 6238) — same math as Google/GitHub. No OTP table in DB ever.
→ SecurityStamp as shared secret — password change auto-invalidates pending OTPs
→ JWT issued ONLY after both factors verified — no partial auth state with a token
→ ISmsService NOT implemented — Email only (free). SMS needs Twilio (paid).
→ useRef (not useState) for Strict Mode guard — ref persists across double-invoke, state does not
→ pending2FAUserId in Redux — safe in-memory, cleared on logout, not in URL or localStorage

─────────────────────────────────────────────────────────────────
Item 9 — OAuth 2.0 + PKCE + Social Login ✅ COMPLETE
─────────────────────────────────────────────────────────────────
What was built:
→ Social Login via Auth0 as Authorization Server (Google as identity provider)
→ Full OAuth 2.0 Authorization Code Flow + PKCE (handled by @auth0/auth0-react SDK)
→ OIDC /userinfo endpoint validation on backend (never trust frontend claims)
→ AspNetUserLogins table used to track social users vs app users in DB
→ Account linking — existing app user signing in with Google links both accounts

Key concepts learned:
→ OAuth 2.0 = authorization framework (can this app access your data?)
→ PKCE = prevents authorization code interception attacks (code_verifier + code_challenge)
→ OIDC = identity layer on top of OAuth (who are you? → id_token with sub, email, name)
→ Auth0 = Authorization Server middleman (handles Google/GitHub/Microsoft in one integration)
→ ProviderKey (sub claim) = permanent unique ID per user per provider — never changes
→ Enterprise = software for large organizations (SSO, SCIM, Active Directory, SAML)

Auth0 setup:
→ Auth0 tenant: dev-p6qgjp2d5mvexwg7.us.auth0.com
→ App name: eShop (Single Page Application)
→ Allowed Callback URL: http://localhost:5173/auth0/callback
→ Allowed Web Origins: http://localhost:5173
→ Username-Password-Authentication: DISABLED (Google only)
→ Google social connection: ENABLED
→ Credentials in .env.local (never in git): VITE_AUTH0_DOMAIN, VITE_AUTH0_CLIENT_ID, VITE_AUTH0_CALLBACK_URL

Backend files created/changed:
→ Identity.Core/Interfaces/ISocialAuthProvider.cs                    (NEW)
     SocialUserInfo record: Provider, ProviderUserId, Email, FirstName, LastName, Picture, EmailVerified
     ISocialAuthProvider interface: GetUserInfoAsync(accessToken) → SocialUserInfo?
→ Identity.Core/Features/Auth/Commands/SocialLogin/                  (NEW folder)
     SocialLoginCommand.cs — carries Provider + AccessToken from frontend
     SocialLoginCommandHandler.cs — 3 steps: validate token → find/create user → issue JWT
→ Identity.Core/Interfaces/IAuthRepository.cs
     Added: FindOrCreateSocialUserAsync(SocialUserInfo) → ApplicationUser
→ Identity.Infrastructure/Services/Auth0UserInfoService.cs           (NEW)
     Implements ISocialAuthProvider
     Calls https://{domain}/userinfo with Bearer token (OIDC standard)
     Parses { sub, email, name, picture, email_verified } → SocialUserInfo
     Sets Provider = "Auth0" on returned record
→ Identity.Infrastructure/Repositories/AuthRepository.cs
     FindOrCreateSocialUserAsync — 3 scenarios:
       Path 1: FindByLoginAsync(provider, sub) → found → fast return (returning social user)
       Path 2: FindByEmailAsync → found → AddLoginAsync → link Google to app account
       Path 3: not found → CreateAsync (random password) + AddToRoleAsync("Customer") + AddLoginAsync
→ Identity.Infrastructure/Extensions/InfrastructureServiceExtensions.cs
     Added: services.AddHttpClient<ISocialAuthProvider, Auth0UserInfoService>()
→ Identity.API/Controllers/AuthController.cs
     Added: POST /api/auth/social-login — public (no JWT yet), validates + issues our JWT
→ Identity.API/DTOs/AuthDtos.cs
     Added: SocialLoginRequest { Provider, AccessToken }
→ Identity.API/appsettings.json
     Added: Auth0:Domain = dev-p6qgjp2d5mvexwg7.us.auth0.com

Frontend files created/changed:
→ eshop-frontend/package.json
     Added: @auth0/auth0-react v2.17.0
→ eshop-frontend/.env.local (never in git)
     VITE_AUTH0_DOMAIN, VITE_AUTH0_CLIENT_ID, VITE_AUTH0_CALLBACK_URL
→ eshop-frontend/src/main.tsx
     Wrapped app in <Auth0Provider domain clientId authorizationParams>
     scope: "openid profile email" — requests OIDC claims
→ eshop-frontend/src/pages/auth/LoginPage.tsx
     Added: "Continue with Auth0" button
     Uses loginWithRedirect({ authorizationParams: { prompt: 'login' } })
     prompt: 'login' forces Auth0 login screen even if session exists
→ eshop-frontend/src/pages/auth/Auth0CallbackPage.tsx               (NEW)
     useRef guard prevents React Strict Mode double-execution
     getAccessTokenSilently() → gets Auth0 access token (PKCE done by SDK)
     POST /social-login → receives our RS256 JWT
     dispatch(setCredentials) → stored in Redux (same as normal login)
     navigate('/products', { replace: true })
→ eshop-frontend/src/routes/AppRouter.tsx
     Added: /auth0/callback public route → Auth0CallbackPage
→ eshop-frontend/src/pages/profile/ProfilePage.tsx
     Added: OIDC learning card — shows raw id_token claims (sub, email_verified, updated_at)
     Visible only when logged in via Auth0
→ eshop-frontend/src/api/authClient.ts
     Added: socialLogin({ provider, accessToken }) → AuthResponse

DB tables involved:
→ AspNetUsers — all users (social + app) stored here
→ AspNetUserLogins — ONLY social users have rows here
     LoginProvider = "Auth0", ProviderKey = "google-oauth2|abc123", UserId = guid
→ Query to see social users: SELECT u.Email, l.LoginProvider, l.ProviderKey FROM AspNetUsers u JOIN AspNetUserLogins l ON u.Id = l.UserId

Key architecture decisions:
→ Auth0 token used ONLY for validation (call /userinfo) — then discarded
→ Our own RS256 JWT always issued — all microservices only know our tokens
→ ISocialAuthProvider in Core — Infrastructure implements (Clean Architecture)
→ AddHttpClient<> (IHttpClientFactory) — proper connection pooling, no socket exhaustion
→ AspNetUserLogins used (not custom table) — ASP.NET Identity built-in, no migration needed
→ ProviderKey (sub) = permanent — never changes even if user changes name/email on Google
→ Account linking — same email on Google + app account → automatically merged

─────────────────────────────────────────────────────────────────────────────────────────────────────
Item 11 — Social Logins (Google + GitHub) ✅ COMPLETE
─────────────────────────────────────────────────────────────────────────────────────────────────────
What was built:
→ GitHub added as second social provider via Auth0 dashboard
→ Replaced single "Continue with Auth0" button with two provider-specific buttons
→ Google and GitHub buttons route directly to their provider using Auth0 connection parameter
→ Zero backend changes needed — Auth0UserInfoService works for any Auth0 provider

Key concepts learned:
→ Auth0 has TWO levels of enabling a connection:
    Level 1 — Enable connection globally (Authentication → Social → GitHub → ON)
    Level 2 — Enable connection for your specific app (Applications → eShop → Connections tab → github ON)
    Missing Level 2 → "the connection is not enabled" error
→ connection parameter — routes Auth0 directly to a provider, skips Auth0's own login UI
    connection: 'google-oauth2'  → goes straight to Google login
    connection: 'github'         → goes straight to GitHub login
→ ProviderKey in AspNetUserLogins — github|abc123 vs google-oauth2|xyz — unique per provider
→ Same backend flow for both — /userinfo returns same shape regardless of social provider

Auth0 setup:
→ GitHub OAuth App created at github.com/settings/developers
→ Callback URL set to: https://dev-p6qgjp2d5mvexwg7.us.auth0.com/login/callback
→ GitHub Client ID + Secret added to Auth0 Social → GitHub connection
→ GitHub connection enabled for eShop app under Applications → Connections tab

Frontend files changed:
→ eshop-frontend/src/pages/auth/LoginPage.tsx
    Replaced: single Auth0 button
    With: two buttons in a flex column — Google + GitHub
    onClick Google: loginWithRedirect({ authorizationParams: { connection: 'google-oauth2', prompt: 'login' } })
    onClick GitHub: loginWithRedirect({ authorizationParams: { connection: 'github', prompt: 'login' } })

DB result:
→ AspNetUserLogins — LoginProvider="Auth0", ProviderKey="github|<id>" for GitHub users
→ Same account linking logic — if email matches existing user, accounts are merged

Next immediate step: Item 12 — OAuth 2.0 Client Credentials Flow
→ Machine-to-machine auth — Ordering.API calls Catalog.API with a service token
→ No user involved — purely service-to-service
→ Identity.API issues tokens to trusted services via client_id + client_secret
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
   → JwtTokenService: generates signed JWTs (upgraded HS256→RS256) + refresh tokens (RandomNumberGenerator)
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
> Logical sequence: Completed → Current → Standard flows → Enterprise → Advanced → Passwordless
> Total cost: £0 — all tools free tier or local

**Foundation already built before Phase 14 (✅):**
```
JWT Authentication       — email + password → JWT token (HS256, then upgraded to RS256)
Refresh Token            — silent re-auth without re-login
Role-based auth          — Admin / User roles in JWT claims
```

**Tools used in Phase 14:**
```
Identity.API             — custom JWT + 2FA + TOTP + MailKit (Phase 14 items 1-4)
Auth0 free tier          — OAuth 2.0, OIDC, Social Logins (Phase 14 items 9-12)
Azure AD B2C free tier   — Consumer identity, Social Logins via Azure (Phase 15)
Entra ID free tier       — Enterprise employee login (Phase 15)
Keycloak (local)         — SAML 2.0, SSO — runs as local Java app, zero cost
MailKit NuGet            — sends OTP email (Gmail SMTP free)
Fido2.NET NuGet          — Passkeys / WebAuthn implementation
Local self-signed certs  — mTLS between microservices
```

| # | Auth Method | Tool | Cost | Status |
|---|------------|------|------|--------|
| 76 | **Silent Token Refresh** — baseQueryWithReauth intercepts 401, retries with new token | RTK Query | 🟢 Free | ✅ Done |
| 77 | **Refresh Token Rotation** — new refresh token on every use, old one invalidated in DB | Identity.API | 🟢 Free | ✅ Done |
| 78 | **JWT RS256 Asymmetric Signing** — private.pem signs, public.pem verifies (never share private key) | Identity.API | 🟢 Free | ✅ Done |
| 79 | **2FA — Email OTP** — TOTP math + MailKit + Gmail SMTP, 2-min expiry, no OTP table in DB | MailKit | 🟢 Free | ✅ Done |
| 80 | **OAuth 2.0 + PKCE** — Authorization Code flow, React SPA as OAuth client, Auth0 as Auth Server | Auth0 | 🟢 Free | 🔄 In Progress |
| 81 | **OpenID Connect (OIDC)** — ID Token, userinfo endpoint, discovery doc (/.well-known) | Auth0 | 🟢 Free | ⏳ |
| 82 | **Social Logins** — Google + GitHub "Login with" via Auth0 (unlocked by OAuth + OIDC) | Auth0 | 🟢 Free | ⏳ |
| 83 | **OAuth 2.0 — Client Credentials** — machine-to-machine, no user involved (B2B APIs) | Auth0 | 🟢 Free | ⏳ |
| 84 | **2FA — TOTP Authenticator App** — QR code setup, Google Authenticator / Authy (30s codes) | OtpNet | 🟢 Free | ⏳ |
| 85 | **Magic Links** — passwordless email login, HMAC-signed expiring link (Slack/Notion style) | Identity.API | 🟢 Free | ⏳ |
| 86 | **Step-up Auth** — re-verify identity for sensitive actions (cancel order > ₹10,000) | Identity.API | 🟢 Free | ⏳ |
| 87 | **SMS OTP** — OTP on mobile number (requires paid Twilio / MSG91 account) | Twilio | 🔴 Paid | ⏳ |
| 88 | **API Key Authentication** — Stripe-style, for service accounts and external integrations | Identity.API | 🟢 Free | ⏳ |
| 89 | **PAT (Personal Access Token)** — GitHub-style long-lived scoped developer tokens | Identity.API | 🟢 Free | ⏳ |
| 90 | **Azure AD B2C** — consumer identity, Azure-native OIDC with custom policies + branding | Azure free | 🟢 Free | ⏳ |
| 91 | **Entra ID (Azure AD)** — enterprise employee login (Login with Microsoft) | Azure free | 🟢 Free | ⏳ |
| 92 | **SAML 2.0 + SSO** — corporate SSO, SP-initiated flow (Salesforce/Workday style) | Keycloak local | 🟢 Free | ⏳ |
| 93 | **Risk-based / Adaptive Auth** — new device/location detected → extra challenge | Identity.API | 🟢 Free | ⏳ |
| 94 | **OAuth 2.0 — Device Authorization** — CLI / smart TV / IoT (code shown on device) | Auth0 | 🟢 Free | ⏳ |
| 95 | **Passkeys / WebAuthn (FIDO2)** — fingerprint/Face ID login, no password at all | Fido2.NET | 🟢 Free | ⏳ |
| 96 | **Mutual TLS (mTLS)** — certificate-based service-to-service auth (local self-signed certs) | Local certs | 🟢 Free | ⏳ |
| 97 | **QR Code Login** — WhatsApp Web style, scan QR with phone to log in on desktop | Identity.API | 🟢 Free | ⏳ |
| 98 | **Session vs Token** — concept deep-dive, when to use which, tradeoffs | Theory | 🟢 Free | ⏳ |
| 99 | **Zero Trust Architecture** — never trust the network, verify every request every time | Theory | 🟢 Free | ⏳ |

---

### ☁️ Phase 15 — Cloud Deployment (AKS + Full Production Stack)
> Deploy the COMPLETE, AUTHENTICATED microservices app from scratch — everything new.
> Logical + learning sequence: understand concept → implement → verify → move on.
> ALL old monolith Azure resources deleted. Everything built fresh for microservices.
> Old Dockerfile + docker-compose.yml (monolith era) replaced entirely.

| Stage | Topic | Cost | Status |
|-------|-------|------|--------|
| 1  | Dockerize locally — multi-stage Dockerfiles + docker-compose | 🟢 Free | ✅ Done |
| 2  | Clean Azure slate — delete all monolith RGs, create rg-eshop-microservices | 🟢 Free | ✅ Done |
| 3  | Azure Data Layer — SQL x4, Cosmos DB, Blob Storage, Storage Queues | 🟢 Free | ✅ Done |
| 4  | Secrets + Central Config — Key Vault + App Configuration | 🟢 Free | ✅ Done |
| 5  | Container Registry — ACR (acreshop2026) | 🟡 ~₹420/mo | ✅ Done |
| 6  | CI/CD Pipelines + Trivy security scanning | 🟢 Free | ⏳ |
| 7  | Kubernetes Concepts — pure learning, no cluster cost | 🟢 Free | ⏳ |
| 8  | AKS Deployment — cluster up, all 4 services running in K8s | 🟡 ~₹2,500/mo | ⏳ |
| 9  | **Entra ID (Azure AD)** — "Login with Microsoft" for Admin users | 🟢 Free | ⏳ |
| 10 | **Azure AD B2C** — Consumer identity for customer login | 🟢 Free | ⏳ |
| 11 | Istio Service Mesh — mTLS zero-trust (PROMISED in Phase 12.7!) | 🟢 Free | ⏳ |
| 12 | Workload Identity + KEDA — pod identity + event-driven autoscaling | 🟢 Free | ⏳ |
| 13 | Observability — App Insights + Log Analytics + distributed tracing | 🟢 Free | ⏳ |
| 14 | Helm Charts — package + version all microservice deployments | 🟢 Free | ⏳ |
| 15 | DNS + SSL + Azure Front Door — HTTPS + custom domain + CDN/WAF | 🟡 ~₹40/mo | ⏳ |
| 16 | Azure Load Testing — prove HPA + KEDA autoscale under real load | 🟢 Free | ⏳ |
| 17 | GitOps — ArgoCD (Git is source of truth, 2026 standard) | 🟢 Free | ⏳ |
| 18 | Multi-Environment — DEV → STAGING → PROD with approval gates | 🟢 Free | ⏳ |

> **Why Stages 9 + 10 come right after AKS (Stage 8):**
> OAuth redirect URIs need REAL deployed URLs — app must be live first.
> Entra ID (simpler) teaches Azure AD concepts that B2C builds on.
> Both are application-layer auth — complete before infrastructure hardening (Istio).

---

#### Stage 1 — Dockerize Everything ✅ COMPLETE
> Multi-stage Dockerfiles for all 4 APIs + React frontend + Docker Compose wiring.
> Local docker-compose up test deferred — no Docker Desktop on office laptop. Verified via ACR build in Stage 5.

```
✅ LEARNED: Multi-stage builds — SDK image builds, aspnet image runs (saves ~700MB per image)
✅ LEARNED: Layer caching — .csproj files first → dotnet restore cached → fast rebuilds
✅ LEARNED: Non-root user + health checks + OCI labels — production best practices
✅ LEARNED: Identity.API special case — private.pem + public.pem baked in (dev only, KV in Stage 8)
✅ LEARNED: Nginx for React SPAs — try_files fallback + security headers + asset caching
✅ LEARNED: Docker Compose — service names = DNS, depends_on, healthcheck, volumes, env var override
✅ LEARNED: Aspire = local dev only — Docker DNS replaces service discovery in compose
✅ LEARNED: __ double underscore = : in ASP.NET Core env vars (array index override for CORS)
✅ LEARNED: ServiceDefaults = shared class library (health checks, telemetry) — not Aspire orchestrator
```

Key decisions:
→ Customer.API: ListenAnyIP in Docker (DOTNET_RUNNING_IN_CONTAINER) vs ListenLocalhost in Aspire (no Windows Firewall popup)
→ Ordering → Customer gRPC: ServiceUrls__CustomerApiGrpc=http://customer-api:5022 (Docker DNS)
→ CORS: Cors__AllowedOrigins__2=http://localhost:3000 added per service (frontend on port 3000 in Docker)
→ SA_PASSWORD + Gmail secrets in .env file (gitignored) — compose references via ${VAR}
→ sqlserver healthcheck with sqlcmd SELECT 1 — APIs wait via depends_on condition: service_healthy
→ Frontend browser calls localhost:5010/5011/5012/5013 (host port mappings) — constants.ts unchanged

Files created:
→ EShopMicroservices/Catalog.API/Dockerfile
→ EShopMicroservices/Customer.API/Dockerfile
→ EShopMicroservices/Ordering.API/Dockerfile
→ EShopMicroservices/Identity.API/Dockerfile
→ eshop-frontend/Dockerfile
→ eshop-frontend/nginx.conf
→ docker-compose.yml (repo root — replaces old monolith compose)
→ Deleted: Dockerfile (old monolith root)

| # | What | Status |
|---|------|--------|
| 15.1.1 | LEARN multi-stage Dockerfile — SDK → aspnet, layer caching, .dockerignore | ✅ |
| 15.1.2 | BUILD Dockerfile — Catalog.API | ✅ |
| 15.1.3 | BUILD Dockerfile — Customer.API | ✅ |
| 15.1.4 | BUILD Dockerfile — Ordering.API | ✅ |
| 15.1.5 | BUILD Dockerfile — Identity.API (includes private.pem + public.pem) | ✅ |
| 15.1.6 | LEARN Nginx for SPAs — try_files, runtime env injection | ✅ |
| 15.1.7 | BUILD Dockerfile — React frontend (Vite build → Nginx serves dist/) | ✅ |
| 15.1.8 | DELETE old monolith Dockerfile + docker-compose.yml from repo root | ✅ |
| 15.1.9 | BUILD new docker-compose.yml — 4 APIs + SQL Server + volumes + networks | ✅ |
| 15.1.10 | TEST docker-compose up — deferred (no Docker Desktop on office laptop) → verified via ACR build in Stage 5 | ⏭️ |

---

#### Stage 2 — Clean Azure Slate ✅ COMPLETE
> Deleted all old monolith resources. Created single fresh resource group for everything.

```
✅ LEARNED: One resource group for all environments (dev/staging/prod via K8s namespaces — cost efficient for learning)
✅ LEARNED: Resource Group delete cascade — one delete removes all child resources
✅ LEARNED: Terraform (Phase 17) will codify everything — terraform destroy/apply for zero cost when not studying
✅ DECISION: rg-eshop-microservices — one RG for entire learning journey
✅ DECISION: Multi-env (Stage 18) via K8s namespaces in ONE cluster — not separate clusters (saves ~₹7,500/mo)
```

| # | What | Status |
|---|------|--------|
| 15.2.1 | LIST + DELETE old monolith resource groups (rg-eshop-prod, rg-eshop-shared) | ✅ |
| 15.2.2 | CREATE rg-eshop-microservices — single resource group for all resources | ✅ |

---

#### Stage 3 — Azure Data Layer ✅ COMPLETE
> All 4 SQL databases + Cosmos DB + Storage Account (blob + queues) created fresh.

```
✅ LEARNED: Azure SQL — one server, multiple databases (cost efficient, shared compute)
✅ LEARNED: Database per service — each microservice owns its data, no cross-DB queries
✅ LEARNED: Cosmos DB — partitions, containers, partition key = most queried field (/productId)
✅ LEARNED: Storage Account = blob + queues in ONE resource (replaced Service Bus — ~₹0.03/mo)
✅ DECISION: Service Bus replaced with Azure Storage Queue (nearly free vs ₹83/2 days)
✅ DECISION: Storage account name must be globally unique across all Azure — steshop2026

🔴 COSTLY LESSONS (Jun 23-24, ₹313 mistake):
✅ LEARNED: Default DB size = 32 GB × ₹9/GB/month = ₹288/month per DB! 🔴
            ALWAYS set --max-size 1GB after creation for learning projects
            Command: az sql db update --name <db> --max-size 1GB
✅ LEARNED: General Purpose tier MINIMUM = 1 GB (not 100 MB)
            Free Limit tier CANNOT be resized (locked at default 32 GB, but free)
✅ LEARNED: Delete + Recreate cycle costs ~₹75 per round (vCore burst + 60min idle window)
            NEVER delete/recreate to "save cost" — it INCREASES cost
✅ LEARNED: vCore = variable cost (₹0 when paused, expensive when active)
            Storage = fixed cost (charged even when paused, based on max-size)
✅ LEARNED: Azure billing has 8-24 hour LAG
            Today's cost display does NOT include last few hours of activity
            Don't trust same-day cost — wait until next morning for accurate number
✅ LEARNED: "Auto-pause" still bills for 60-min idle window before pausing
            Each DB resume = 60min minimum compute charge even with no queries
✅ LEARNED: CustomerDb not recreated → saved ₹10-15/month (recreate in Stage 8 only)

OPTIMAL CONFIG (after lessons learned):
  CatalogDb    Free Limit       32 GB    AutoPause 60min   ₹0/month
  OrderingDb   Serverless GP    1 GB     AutoPause 60min   ₹9/month storage
  IdentityDb   Serverless GP    1 GB     AutoPause 60min   ₹9/month storage
  CustomerDb   DELETED (recreate in Stage 8 only)
  ─────────────────────────────────────────────────────────────────
  Expected:    ~₹25/month total (vs ₹860/month with defaults)
```

Key resources created:
→ sql-eshop-dev          — Azure SQL Server (Serverless Gen5, auto-pause 60min)
→ CatalogDb              — products, categories
→ CustomerDb             — customers, addresses
→ OrderingDb             — orders, order items
→ IdentityDb             — users, roles, JWT refresh tokens
→ cosmos-eshop-dev       — Cosmos DB NoSQL (free tier) — EShopDb/reviews (/productId partition)
→ steshop2026            — Storage Account (Standard_LRS)
→ product-images         — Blob container (product images)
→ order-placed-catalog   — Storage Queue (catalog stock reduction events)
→ order-placed-customer  — Storage Queue (customer notification events)

| # | What | Cost | Status |
|---|------|------|--------|
| 15.3.1 | CREATE Azure SQL Server — sql-eshop-dev (Central India) | 🟢 Free | ✅ |
| 15.3.2 | CREATE CatalogDb on sql-eshop-dev | 🟢 Free | ✅ |
| 15.3.3 | CREATE CustomerDb on sql-eshop-dev | 🟢 Free | ✅ |
| 15.3.4 | CREATE OrderingDb on sql-eshop-dev | 🟢 Free | ✅ |
| 15.3.5 | CREATE IdentityDb on sql-eshop-dev | 🟢 Free | ✅ |
| 15.3.6 | ADD firewall rule — AllowAzureServices (0.0.0.0 → 0.0.0.0) | 🟢 Free | ✅ |
| 15.3.7 | CREATE Cosmos DB (free tier) — EShopDb → reviews container (/productId) | 🟢 Free | ✅ |
| 15.3.8 | CREATE Storage Account — steshop2026 → product-images blob + 2 queues | 🟢 Free | ✅ |

---

#### Stage 4 — Secrets + Central Config ✅ COMPLETE
> No passwords in code, YAML, or environment variables — ever.
> Key Vault holds secrets. App Configuration holds settings + KV references.
> All 4 microservices read config from ONE place.

```
✅ LEARNED: Key Vault uses RBAC — must assign "Key Vault Secrets Officer" role to your user
✅ LEARNED: App Config stores KV references (pointers), never raw secret values
✅ LEARNED: DefaultAzureCredential — az login locally, Managed Identity in AKS (zero code change)
✅ LEARNED: Configuration sources must be added in Program.cs before builder.Build() — not in AddInfrastructure
✅ LEARNED: AddAzureAppConfiguration auto-resolves KV references — API only needs App Config URL
✅ DECISION: Messaging:Provider = "StorageQueue" in App Config (switch to ServiceBus = change 1 value)
✅ DECISION: AppConfig:Endpoint not set locally → User Secrets used (no dev workflow change)
```

Key resources created:
→ kv-eshop-dev            — Key Vault (RBAC mode)
→ appconfig-eshop-dev     — App Configuration (free tier)

Secrets stored in Key Vault (8 total):
→ ConnectionStrings--CatalogDb, CustomerDb, OrderingDb, IdentityDb
→ ConnectionStrings--Storage (steshop2026), CosmosDb
→ EmailSettings--AppPassword, EmailSettings--FromEmail

KV references in App Config (+ 1 plain value):
→ ConnectionStrings:CatalogDb/CustomerDb/OrderingDb/IdentityDb/Storage/CosmosDb
→ EmailSettings:AppPassword, EmailSettings:FromEmail
→ Messaging:Provider = "StorageQueue"

Code changes (all 4 Program.cs files):
→ Added Azure.Identity + Microsoft.Extensions.Configuration.AzureAppConfiguration NuGet
→ Added conditional AddAzureAppConfiguration block — only active when AppConfig:Endpoint is set

| # | What | Cost | Status |
|---|------|------|--------|
| 15.4.1 | CREATE Key Vault — kv-eshop-dev | 🟢 Free | ✅ |
| 15.4.2 | STORE 8 secrets in KV (connection strings + email credentials) | 🟢 Free | ✅ |
| 15.4.3 | CREATE App Configuration — appconfig-eshop-dev | 🟢 Free | ✅ |
| 15.4.4 | ADD 8 KV references + Messaging:Provider in App Config | 🟢 Free | ✅ |
| 15.4.5 | CODE — wire all 4 microservices to read from App Config (conditional) | 🟢 Free | ✅ |
| 15.4.6 | BUILD — dotnet build passes with zero errors | 🟢 Free | ✅ |

---

#### Stage 5 — Azure Container Registry (ACR) ✅ COMPLETE
> Private Docker registry — images pushed here, AKS pulls from here.

```
✅ LEARNED: ACR names globally unique — acreshopdev taken, used acreshop2026
✅ LEARNED: az acr build — builds Docker image in Azure cloud (no local Docker needed!)
✅ LEARNED: Build context must match Dockerfile COPY paths — use EShopMicroservices/ not repo root
✅ LEARNED: .dockerignore in build context — exclude bin/obj to avoid Windows path errors
✅ LEARNED: NuGet.Config — clear fallback folders for Linux Docker builds
✅ LEARNED: AcrPull role — assigned to AKS Managed Identity in Stage 8 (not now)
✅ LEARNED: TypeScript checking vs Docker build — TWO separate concerns:
            CI/CD Step 1 → npx tsc --noEmit   = validates types, no output, fast fail
            CI/CD Step 2 → az acr build        = Dockerfile runs npx vite build (no tsc)
            WHY: Docker job = bundle code only (fast, focused)
                 CI job    = validate code quality (types, tests, security scan)
            RULE: tsc errors caught in CI BEFORE Docker build even starts → no wasted time
✅ DECISION: Delete ACR when not studying (₹14/day), recreate with same name next session
✅ DECISION: Microsoft.ContainerRegistry provider must be registered once per subscription
```

Key resources:
→ acreshop2026         — ACR Basic tier (acreshop2026.azurecr.io)
→ catalog-api:1.0.0   — pushed ✅
→ customer-api:1.0.0  — pushed ✅
→ ordering-api:1.0.0  — pushed ✅
→ identity-api:1.0.0  — pushed ✅
→ frontend:1.0.0      — pushed ✅

Files added/modified:
→ EShopMicroservices/.dockerignore       — excludes bin/obj from Docker build context
→ EShopMicroservices/NuGet.Config        — clears Windows fallback package folders
→ eshop-frontend/.dockerignore           — excludes node_modules/dist (Windows symlinks break Linux)
→ eshop-frontend/Dockerfile              — use npx vite build instead of npm run build (skip tsc)

Issues resolved during Stage 5:
→ MissingSubscriptionRegistration        — az provider register --namespace Microsoft.ContainerRegistry
→ AlreadyInUse (acreshopdev taken)       — renamed to acreshop2026
→ Windows path in project.assets.json    — fixed with .dockerignore (exclude bin/obj)
→ node_modules Windows symlinks          — fixed with eshop-frontend/.dockerignore
→ TypeScript errors blocking build       — fixed with npx vite build (tsc moved to CI step)

| # | What | Cost | Status |
|---|------|------|--------|
| 15.5.1 | CREATE ACR — acreshop2026 (Basic tier) | 🟡 ~₹420/mo | ✅ |
| 15.5.2 | BUILD + PUSH all 5 images (az acr build — no Docker Desktop needed) | 🟢 Free | ✅ |
| 15.5.3 | ASSIGN AcrPull role — deferred to Stage 8 when AKS is created | 🟢 Free | ⏳ Stage 8 |

---

#### Stage 6 — CI/CD Pipelines + Security Scanning + Versioning
> Every PR builds and scans images. Every merge deploys automatically.
> Trivy scans for CVEs before any image reaches ACR.
> Git tags control semantic versions shown in UI and DLLs.

```
LEARN: GitHub Actions OIDC — federated identity to Azure, no stored secrets needed
LEARN: Build matrix — build all 5 images in parallel in one workflow
LEARN: PR-vs-Push workflow split — PRs scan only, pushes deploy
LEARN: Trivy — open-source CVE scanner for Docker images and NuGet packages
LEARN: CodeQL — GitHub native SAST (Static Application Security Testing)
LEARN: Code coverage gates — fail build if coverage drops below threshold
LEARN: Build caching — cache NuGet + node_modules → 8min → 3min builds
LEARN: Dependabot — auto-PRs for dependency updates (NuGet + npm + Docker)
LEARN: Branch protection — block direct push to main, require CI checks
LEARN: Status badges — README shows live build status (recruiter-friendly)
LEARN: TypeScript separation — tsc --noEmit in CI (validate), vite build in Dockerfile (bundle)
LEARN: Azure Static Web Apps CI/CD — deployment token, auto-deploy on push
```

```
VERSIONING STRATEGY (decided in Stage 5):
─────────────────────────────────────────────────────────────────────
Layer 1 — Docker Images (ACR):
  Every commit → catalog-api:sha-a3f9c12    (automatic, always)
  Every commit → catalog-api:latest          (automatic, always)
  git tag v1.0.0 → catalog-api:1.0.0        (only on release)
  AKS always uses SHA tag in production — never :latest

Layer 2 — Semantic Versioning (Git Tags):
  v1.0.0 → Stage 8 complete (first AKS deployment)
  v1.1.0 → Stage 9 complete (Entra ID added)
  v1.2.0 → Stage 10 complete (B2C added)
  Rule: git tag = time machine → checkout any version, reproduce any bug

Layer 3 — .NET DLLs (Directory.Build.props):
  FileVersion:          1.0.0
  InformationalVersion: 1.0.0-sha-a3f9c12
  All 4 services inherit from one central file

Layer 4 — Frontend UI:
  VITE_APP_VERSION build arg → React footer shows version
  git push only  → footer shows sha-a3f9c12
  git tag v1.0.0 → footer shows v1.0.0  ✅

CI/CD version logic:
  if tag push  → VERSION = v1.0.0     (human-friendly, UI shows this)
  if PR/commit → VERSION = sha-abc123  (technical, traceable)

Real companies (Netflix, Amazon, Spotify) use this exact approach.
Conventional Commits auto-bump versions:
  fix: ...        → patch (1.0.0 → 1.0.1)
  feat: ...       → minor (1.0.1 → 1.1.0)
  BREAKING CHANGE → major (1.1.0 → 2.0.0)
─────────────────────────────────────────────────────────────────────
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.6.1 | CREATE Service Principal with OIDC federation (no stored client secrets) | 🟢 Free | ✅ |
| 15.6.2 | CONFIGURE GitHub repository secrets (AZURE_CLIENT_ID, TENANT_ID, SUBSCRIPTION_ID) | 🟢 Free | ✅ |
| 15.6.3 | BUILD pr-validation.yml — runs on PRs (build + tsc check, NO push) | 🟢 Free | ✅ |
| 15.6.4 | BUILD build-and-push.yml — runs on main push (build + push to ACR via matrix) | 🟢 Free | ✅ |
| 15.6.5 | ADD build matrix — parallelize 4 API image builds (catalog, customer, ordering, identity) | 🟢 Free | ✅ |
| 15.6.6 | ADD Trivy scan step — fail pipeline if CRITICAL CVE found | 🟢 Free | ✅ |
| 15.6.7 | ADD CodeQL workflow — GitHub native SAST (C# + TypeScript) | 🟢 Free | ✅ |
| 15.6.8 | ADD code coverage step — dotnet test XPlat Code Coverage → artifact uploaded | 🟢 Free | ✅ |
| 15.6.9 | ADD build caching — NuGet cache (54s → 4s per run) | 🟢 Free | ✅ |
| 15.6.10 | IMPLEMENT versioning — SHA tag in build-and-push.yml + dev fallback | 🟢 Free | ✅ |
| 15.6.11 | ADD Directory.Build.props — .NET DLL versioning for all 4 services | 🟢 Free | ✅ |
| 15.6.12 | ADD VITE_APP_VERSION — frontend footer shows version from build-time injection | 🟢 Free | ✅ |
| 15.6.13 | ADD dependabot.yml — auto-PRs for NuGet + npm + Actions updates | 🟢 Free | ✅ |
| 15.6.14 | CONFIGURE GitHub branch protection — main + develop require PR + passing checks | 🟢 Free | ✅ |
| 15.6.15 | ADD status badges to README — build, coverage, security scan | 🟢 Free | ⏳ |
| 15.6.16 | BUILD deploy-frontend.yml — React build → Azure Static Web Apps | 🟢 Free | ⏳ |
| 15.6.17 | TEST (when ACR recreated) — open PR → pipeline runs → merge → images appear in ACR | 🟢 Free | ⏳ |
| 15.6.18 | FIX MessagePack vulnerability — pinned to 2.5.302 in AppHost (was 2.5.192) | 🟢 Free | ✅ |
| 15.6.19 | ADD path-based filtering to build-and-push.yml — only changed services rebuild on push | 🟢 Free | ✅ |

> **COST NOTE — Azure SQL deleted to stop vCore charges (Jun 2026).**
> Serverless DBs start ONLINE on creation and bill vCore until 60-min idle pause; any
> Portal/connection access re-wakes them. All 3 DBs (Catalog/Identity/Ordering) deleted and
> server `publicNetworkAccess` set to Disabled. Recreate ONLY at Stage 8 deployment with
> cost-safe flags: `--min-capacity 0.5 --auto-pause-delay 60 --max-size 1GB --compute-model Serverless`.

---

#### Stage 7 — Kubernetes Concepts (Pure Learning — FREE, No Cluster Yet)
> Understand every K8s concept deeply BEFORE creating the AKS cluster.
> Saves money and avoids trial-and-error mistakes on a paid cluster.

```
LEARN: Pod — smallest unit, one container, ephemeral (dies and restarts automatically)
LEARN: Node — VM that runs pods (B2s in our case)
LEARN: Cluster — group of nodes + control plane (AKS manages control plane FREE)
LEARN: Deployment — desired state ("keep 2 Catalog.API pods running always")
LEARN: ReplicaSet — enforces the desired pod count automatically
LEARN: Service — stable DNS name + IP for pods (ClusterIP = internal, LoadBalancer = external IP)
LEARN: ConfigMap — non-secret config stored in K8s (env vars, URLs)
LEARN: Secret — base64 encoded only (NOT encrypted!) — why CSI Key Vault Driver is needed
LEARN: Ingress — one public IP, path-based routing (/api/catalog → Catalog pod)
LEARN: Namespace — logical isolation (eshop namespace separates our pods from system pods)
LEARN: Init Container — runs once before main container starts (EF migrations use this!)
LEARN: Resource Requests + Limits — CPU/memory per pod, REQUIRED for HPA to work
LEARN: Helm — package manager for K8s (why 50 raw YAML files is unmanageable)
LEARN: Independent service deployment — each service has its own image SHA + version lifecycle
LEARN: UI version = frontend's own SHA only — independent of all backend service versions
LEARN: path-based CI/CD — workflow-level paths filter + per-service paths in matrix + git diff detect
LEARN: fetch-depth: 2 — needed so git diff HEAD^ HEAD can compare last 2 commits in GitHub Actions
```

| # | What | Status |
|---|------|--------|
| 15.7.1 | LEARN Pod, Node, Cluster, Control Plane — draw the architecture | ✅ |
| 15.7.2 | LEARN Deployment + ReplicaSet + Service (ClusterIP vs LoadBalancer) | ✅ |
| 15.7.3 | LEARN ConfigMap + Secret + why raw K8s Secrets are NOT secure alone | ✅ |
| 15.7.4 | LEARN Ingress — path routing, host routing, TLS termination | ✅ |
| 15.7.5 | LEARN Namespace, Init Container, Resource Requests + Limits | ✅ |
| 15.7.6 | LEARN Helm — Chart.yaml, values.yaml, templates/, helm install/upgrade | ✅ |
| 15.7.7 | WRITE all K8s YAML manually first — understand raw manifests before Helm | ✅ |

```
15.7.7 OUTPUT — k8s/ folder (15 files total):
  k8s/
    ├── namespace.yaml                   → eshop namespace (isolates our pods from system)
    ├── ingress.yaml                     → NGINX path routing for all 4 services
    ├── catalog-api/
    │     ├── configmap.yaml             → ASPNETCORE_ENVIRONMENT, URLS, Messaging provider
    │     ├── service.yaml               → ClusterIP, port 80 → targetPort 8080
    │     └── deployment.yaml            → replicas:2, initContainer (EF migrations), probes, limits
    ├── customer-api/
    │     ├── configmap.yaml
    │     ├── service.yaml
    │     └── deployment.yaml            → replicas:2, initContainer, probes, limits
    ├── ordering-api/
    │     ├── configmap.yaml
    │     ├── service.yaml
    │     └── deployment.yaml            → replicas:2, initContainer, probes, limits
    └── identity-api/
          ├── configmap.yaml
          ├── service.yaml
          └── deployment.yaml            → replicas:2, initContainer (has own DB!), probes, limits

KEY LEARNINGS:
  → All containers expose port 8080 (EXPOSE 8080 in Dockerfile)
  → Service port 80 → targetPort 8080 (ClusterIP internal only)
  → initContainer runs EF migrations BEFORE main container starts
  → ALL 4 services need initContainer (each has own SQL DB)
  → livenessProbe  = K8s restarts pod if /health fails
  → readinessProbe = K8s stops traffic until /health passes
  → configMapRef injects ALL configmap keys as env vars
  → ingress.yaml routes: /api/catalog → /api/customers → /api/orders → /api/auth
  → ConfigMap = minimal bootstrap only (ASPNETCORE vars)
    real app config pulled from Azure App Configuration at runtime
```

---

#### Stage 8 — AKS Deployment
> Cluster up. Deploy all 4 microservices. NGINX routes traffic. React frontend live.
> Stop AKS node when not studying → cost drops to ~₹0.

```
PRE-STAGE 8 SMOKE TEST FIXES:
  → Cosmos DB emulator → real Azure Cosmos DB (cosmos-eshop-dev)
    Fixed via: dotnet user-secrets set "ConnectionStrings:CosmosDb" "<connection-string>"
  → Write a Review form added to ProductDetailPage.tsx (was missing from frontend)
    catalogApi.ts → createReview mutation + useCreateReviewMutation export
    ProductDetailPage → star rating + comment textarea + submit button

COST ANALYSIS (revised — SQL Server in AKS pod, NOT Azure SQL):
  Control Plane     → FREE (AKS Free tier)
  B2s node          → ₹4/hr → ₹240/mo studying 2hrs/day
  OS Disk (Std HDD) → ₹80/mo (use Standard HDD not Premium SSD!)
  Azure Disk (PVC)  → ₹8/mo  (32GB HDD for SQL Server data)
  Load Balancer     → ₹0 (use kubectl port-forward during learning!)
  Public IP         → ₹0 (no LB = no IP)
  ACR Basic         → ₹420/mo (keep during Phase 15, delete after)
  Azure SQL         → ₹0 (ABANDONED — bills per second even with auto-pause,
                           portal access wakes DB, unreliable cost control!)
  TOTAL             → ~₹748/mo while studying ✅  (was ₹3,000/mo!)

  Strategy: az aks start before studying → az aks stop after studying
  LB only when specifically testing Ingress → delete immediately after
  ACR = keep always (delete/recreate = 30min rebuild pain not worth ₹14/day)

WHY SQL IN AKS POD (not Azure SQL):
  → Azure SQL bills per second from creation — no true zero cost
  → Serverless auto-pause unreliable (portal access wakes it immediately!)
  → SQL Server pod stops WITH AKS node = guaranteed ₹0 when not studying
  → Data on Azure Disk via PVC = survives pod restarts and node stops
  → Teaches PVC, StatefulSet pattern, pod-to-pod DNS = more K8s learning!
  → Real companies also run SQL in K8s = valid production pattern

CONNECTION STRING CHANGE:
  Old (Azure SQL):  Server=sqlserver-eshop.database.windows.net;Database=CatalogDb;...
  New (SQL pod):    Server=sql-server,1433;Database=CatalogDb;User Id=sa;Password=...
  sql-server = K8s ClusterIP Service name = internal DNS auto-resolved inside cluster
  Key Vault stores new value → App Config references it → pods read at startup (same flow!)

PROBLEM SOLVED — SQL pod volume permission denied:
  → SQL Server container runs as non-root user (UID 10001, "mssql")
  → Azure Disk PVC mounted as root-owned by default → SQL couldn't write to /var/opt/mssql
  → Fix: added `securityContext.fsGroup: 10001` at the POD level in deployment.yaml
    This tells Kubernetes to chown the mounted volume to group 10001 on attach,
    so the mssql user (already in that group inside the image) can read/write it.

WORKLOAD IDENTITY SETUP (passwordless Azure auth for pods — no secrets/keys!):
  Concept: OIDC federation — AKS cluster issues its own OIDC tokens for pods.
  Azure AD trusts that OIDC issuer for a specific Managed Identity + K8s ServiceAccount pair.
  A pod using that ServiceAccount can request an Azure AD token with ZERO stored credentials.

  Steps performed:
  1. `az aks update --enable-oidc-issuer --enable-workload-identity` on aks-eshop
  2. `az identity create` → id-eshop-workload (a User-Assigned Managed Identity)
  3. `az role assignment create` ×2 → granted this identity:
       - Key Vault Secrets User (read secrets)
       - App Configuration Data Reader (read config)
  4. `az identity federated-credential create` → linked:
       id-eshop-workload  ⟷  system:serviceaccount:eshop:eshop-sa
     (this is the actual trust relationship — "this K8s SA token = this Azure identity")
  5. `kubectl create serviceaccount eshop-sa -n eshop`
     + annotate: azure.workload.identity/client-id=<id-eshop-workload clientId>
     + label:    azure.workload.identity/use: "true"
  6. Every deployment.yaml → serviceAccountName: eshop-sa + pod label
     azure.workload.identity/use: "true" (a mutating webhook injects the
     token-projection volume + env vars automatically at pod creation)
  7. App code just calls DefaultAzureCredential() — it silently discovers and
     uses the projected Workload Identity token, no code changes needed.

  Why this matters: no ClientSecret, no ConnectionString-with-key anywhere —
  the only "identity" is Kubernetes' own ServiceAccount token, federated to Azure.

PROBLEM — ErrImagePull (all 4 microservice pods) — ✅ RESOLVED:
  → kubectl apply succeeded for all deployments/services/configmaps
  → But pods stuck in ErrImagePull / ImagePullBackOff
  → Root cause: `az acr repository list --name acreshop2026` → EMPTY.
    Dockerfiles were written long ago but images were never built + pushed.
  → Fix: committed k8s/ changes on develop → pushed → PR develop→main →
    merged → build-and-push.yml (GitHub Actions) built all 4 images and
    pushed to ACR tagged :latest.
  → AKS Kubelet identity's AcrPull RBAC role on acreshop2026 confirmed
    working — images pulled successfully once pushed, no further action
    needed (role was already attached from an earlier stage).

PROBLEM — CrashLoopBackOff on 3/4 services after images existed — ✅ RESOLVED:
  → After ACR had images, catalog-api came up fine, but customer-api,
    identity-api, and ordering-api went into CrashLoopBackOff.
  → `kubectl describe pod` showed liveness/readiness probes failing:
    GET /health → 404 Not Found (probe kept restarting the container).
  → Root cause: a production fix earlier mapped health-check endpoints
    outside the `IsDevelopment()` block (needed for them to respond in
    AKS's Production environment). That fix lived in shared
    `ServiceDefaults` project code, not in any single service's folder.
  → `build-and-push.yml`'s GitHub Actions path filters were scoped per
    service, e.g. paths only matched `EShopMicroservices/Catalog.**` for
    the catalog job. None of the filters included the shared
    `ServiceDefaults`/`Contracts` projects that ALL 4 services reference.
  → Net effect: when the health-check fix was committed, only catalog-api
    (which happened to also have its own folder touched in the same
    commit) got rebuilt. The other 3 services silently kept running a
    STALE image that still 404'd on /health — invisible in `git diff`
    review because the workflow file itself looked "correct" at a glance.
  → Fix: updated `.github/workflows/build-and-push.yml` so every service's
    `paths` filter also includes
    `EShopMicroservices/EShopMicroservices.ServiceDefaults/**` and
    `EShopMicroservices/EShop.Contracts/**` (regex-OR'd with its own
    folder). Merged develop→main → all 4 matrix jobs rebuilt → verified
    all 4 images tagged with the same fresh commit SHA → `kubectl rollout
    restart` → all pods Running, 0 restarts.
  → Lesson: any CI path-filter/monorepo build trigger MUST explicitly
    account for shared/common projects, not just each service's own path —
    otherwise a shared-code fix can pass code review and CI green, yet
    never actually reach the services that depend on it.

PROBLEM — SQL Server unreachable from SSMS via kubectl port-forward — ✅ RESOLVED:
  → `kubectl port-forward svc/sql-server 14330:1433` showed "Forwarding
    from 127.0.0.1:14330" and `Test-NetConnection localhost -Port 14330`
    returned TcpTestSucceeded: True — raw TCP tunnel was alive.
  → SSMS still failed: first with error 1225 (connection refused) — cause
    was the port-forward terminal having been closed/returned to a normal
    prompt (it's a blocking foreground process; closing/interrupting it
    kills the tunnel even though it looked "done").
  → After restarting the tunnel, SSMS failed differently: error 258 ("wait
    operation timed out") at the TCP provider level — this is a known
    `kubectl port-forward` + SQL Server TDS/TLS handshake flakiness; the
    login/encryption negotiation doesn't always survive the proxy well,
    even though a raw TCP connect succeeds.
  → Fix: connect using `127.0.0.1,14330` explicitly (not `localhost` —
    IPv4 loopback resolved more reliably than the localhost hostname over
    the tunnel), leave Database Name as `<default>` (a stale
    autocompleted DB name from an unrelated project caused extra
    failures), and enable "Trust Server Certificate". With those three
    changes SSMS connected successfully.
  → Reliable fallback if port-forward + SSMS ever misbehaves again:
    `kubectl exec -it <sql-pod> -- /opt/mssql-tools18/bin/sqlcmd -S
    localhost -U sa -P "<password>" -C` — runs entirely inside the
    cluster, bypasses the tunnel and TDS-over-proxy issues entirely.

PROBLEM — Identity API RS256 JWT chain — ✅ VERIFIED END-TO-END:
  → Confirmed `/app/private.pem` + `/app/public.pem` mounted correctly via
    the `identity-pem-keys` K8s Secret + volume mount in identity-api's
    deployment.yaml.
  → `kubectl logs` showed clean startup: EF migrations applied, DB seeder
    ran (IdentityDataSeeder.cs), no RSA/PEM key-loading errors.
  → Seeded credentials confirmed from IdentityDataSeeder.cs:
    admin@eshop.com / Admin@12345 (Admin role),
    alice@eshop.com / Customer@12345 (Customer role).
  → Port-forwarded identity-api (`kubectl port-forward svc/identity-api
    8082:80`) and called `POST /api/auth/login` via PowerShell's
    `Invoke-RestMethod` (plain `curl -H ...` fails in PowerShell because
    `curl` is aliased to `Invoke-WebRequest`, which uses different flag
    syntax — use `Invoke-RestMethod` or `curl.exe` explicitly instead).
  → Got back a valid RS256-signed JWT + refresh token + correct Admin
    role claim — proves the private key signs correctly and the full
    Identity.API → Identity.Infrastructure → SQL pod chain works in AKS.
  → Remaining/minor: hitting a protected endpoint (e.g. GET /api/auth/me)
    with the bearer token would additionally prove the PUBLIC key side of
    validation — not yet done, low priority since the key pair is a single
    matched PEM file set mounted from the same Secret.
```

```
LEARN: AKS architecture — managed control plane (free) + worker nodes (paid)
LEARN: PVC (Persistent Volume Claim) — how pods get durable storage in K8s
LEARN: StatefulSet pattern — databases need stable identity + persistent storage
LEARN: Pod-to-pod DNS — K8s Service name resolves inside cluster (e.g., sql-server)
LEARN: fsGroup securityContext — fixes non-root container volume permission errors
LEARN: AKS Workload Identity — OIDC federation between K8s ServiceAccount + Azure AD identity
LEARN: Federated credential — the actual trust link (SA ↔ Managed Identity), no secrets stored
LEARN: DefaultAzureCredential — same app code works locally (VS) and in AKS (Workload Identity)
LEARN: AcrPull role — separate from Workload Identity; lets AKS nodes pull container images
LEARN: CSI Key Vault Driver — pods mount KV secrets as files (safer than env vars)
LEARN: Init Containers for EF migrations — run once before pods start (multi-pod safe)
LEARN: Resource Requests + Limits — required for HPA to function correctly
LEARN: NGINX Ingress Controller — one public IP, routes by path prefix
LEARN: HPA — scale by CPU/memory (KEDA handles event-driven scaling in Stage 12)
LEARN: Azure Static Web Apps — free React hosting with CI/CD
LEARN: kubectl port-forward — test pods locally without creating expensive LoadBalancer
LEARN: az aks start/stop — deallocates node VMs (saves compute, disk still charged if exists)
LEARN: CI path filters in a monorepo must include shared/common projects, not just each
       service's own folder — otherwise shared-code fixes silently skip rebuilding dependents
LEARN: kubectl port-forward can tunnel raw TCP fine but still choke on SQL Server's TDS/TLS
       handshake — prefer 127.0.0.1 over localhost, Trust Server Certificate, and keep
       `kubectl exec` + sqlcmd as a guaranteed fallback
LEARN: PowerShell aliases `curl` to Invoke-WebRequest (different flag syntax) — use
       Invoke-RestMethod or curl.exe explicitly when testing REST APIs from PowerShell
LEARN: RS256 JWT end-to-end proof = confirm PEM mount + clean logs + successful login
       returning a correctly-signed token with expected claims/roles
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.8.1 | CREATE AKS cluster — aks-eshop (1 node × B2s, Standard HDD) | 🟡 ~₹748/mo | ✅ |
| 15.8.2 | GET credentials — az aks get-credentials + verify kubectl connection | 🟢 Free | ✅ |
| 15.8.3 | CREATE namespace — kubectl apply -f k8s/namespace.yaml | 🟢 Free | ✅ |
| 15.8.4 | CREATE SQL StorageClass + PVC — k8s/sql-server/ (Azure Disk 32GB HDD) | 🟡 ~₹8/mo | ✅ Bound |
| 15.8.5 | CREATE SQL Secret — k8s/sql-server/secret.yaml (SA password) | 🟢 Free | ✅ |
| 15.8.6 | CREATE SQL Deployment + Service — k8s/sql-server/deployment+service.yaml | 🟢 Free | ✅ Fixed with fsGroup: 10001 |
| 15.8.7 | VERIFY SQL pod running — kubectl get pods -n eshop + sqlcmd login | 🟢 Free | ✅ |
| 15.8.8 | UPDATE Key Vault — connection strings → Server=sql-server,1433 (×4 DBs) | 🟢 Free | ✅ |
| 15.8.8a | ENABLE OIDC issuer + Workload Identity on aks-eshop | 🟢 Free | ✅ |
| 15.8.8b | CREATE Managed Identity — id-eshop-workload | 🟢 Free | ✅ |
| 15.8.8c | GRANT RBAC — Key Vault Secrets User + App Config Data Reader | 🟢 Free | ✅ |
| 15.8.8d | CREATE federated credential — eshop-sa ↔ id-eshop-workload | 🟢 Free | ✅ |
| 15.8.8e | CREATE + annotate + label K8s ServiceAccount — eshop-sa | 🟢 Free | ✅ |
| 15.8.8f | WIRE eshop-sa + workload identity label into all 4 deployment.yaml | 🟢 Free | ✅ |
| 15.8.8g | ADD AppConfig__Endpoint to all 4 configmap.yaml | 🟢 Free | ✅ |
| 15.8.8h | CREATE identity-pem-keys Secret + mount into identity-api (RS256 JWT) | 🟢 Free | ✅ |
| 15.8.9 | ASSIGN AcrPull role — AKS Kubelet Identity → acreshop2026 | 🟢 Free | ✅ Confirmed (images pull successfully) |
| 15.8.10 | INSTALL CSI Key Vault Driver — pods read KV secrets as mounted files | 🟢 Free | ⏳ (superseded by Workload Identity + App Config approach) |
| 15.8.11 | APPLY all microservice YAML — kubectl apply -f k8s/ | 🟢 Free | ✅ Applied — all objects created |
| 15.8.12 | VERIFY all pods running — kubectl get pods -n eshop | 🟢 Free | ✅ All 4 pods Running, 0 restarts. Fixed two issues: (1) ACR had zero images → fixed by merging k8s/ changes develop→main; (2) 3/4 services stayed on stale images (CrashLoopBackOff, /health 404) because CI path filters didn't cover shared ServiceDefaults/Contracts → fixed `build-and-push.yml` filters, rebuilt all 4 images. |
| 15.8.13 | TEST services — kubectl port-forward each service | 🟢 Free | ✅ SQL Server via SSMS (127.0.0.1,14330) + identity-api login verified end-to-end (RS256 JWT issued for admin@eshop.com) |
| 15.8.14 | INSTALL NGINX Ingress Controller | 🟢 Free | ⏳ |
| 15.8.15 | APPLY ingress.yaml — test path routing via public IP | 🟡 LB cost | ⏳ |
| 15.8.16 | ADD HPA — Catalog.API scales 1→3 pods at 70% CPU | 🟢 Free | ⏳ |
| 15.8.17 | DEPLOY React frontend — Azure Static Web Apps | 🟢 Free | ⏳ |
| 15.8.18 | TEST end-to-end — login → browse → review → order → all working in AKS | 🟢 Free | ⏳ |
| 15.8.19 | STOP AKS node — az aks stop (save cost when not studying) | 🟢 Free | ✅ Stopped again after verifying login end-to-end |

---

#### Stage 8b — APIM (Optional Enterprise Layer in Front of AKS)
> AKS is running. NGINX Ingress is the internal gateway.
> APIM sits OUTSIDE AKS as an optional enterprise layer.
> Adds rate limiting, analytics, developer portal, API monetization.
> Zero changes to K8s when added — APIM just points to NGINX public IP.

```
LEARN: APIM tiers — Consumption (FREE base) vs Developer vs Basic vs Standard vs Premium
LEARN: APIM sits outside AKS — NGINX becomes internal only (more secure!)
LEARN: Request flow — Internet → APIM → NGINX Ingress → Pod
LEARN: APIM policies — rate limiting, API key validation, caching, transformation
LEARN: Developer portal — external developers browse + test your APIs
LEARN: APIM vs NGINX — APIM = enterprise governance, NGINX = K8s routing
LEARN: Adding APIM later = zero K8s changes — plug and play!
```

```
APIM Pricing:
  Consumption  → ₹0 base + ₹350 per million calls (first 1M FREE) ← use this!
  Developer    → ~₹6,000/mo  (test enterprise features, NOT production)
  Basic        → ~₹12,000/mo (production small)
  Standard     → ~₹60,000/mo (medium companies)
  Premium      → ~₹2,40,000/mo (large enterprise, multi-region)
```

```
Flow WITHOUT APIM (Stage 8):
  Internet → NGINX Ingress (public IP) → Pods

Flow WITH APIM (Stage 8b):
  Internet → APIM (public facing)
               → rate limiting + API key + analytics
             → NGINX Ingress (internal IP only — more secure!)
               → Pods
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.8b.1 | CREATE APIM — Consumption tier (free base) | 🟢 Free | ⏳ |
| 15.8b.2 | IMPORT APIs from NGINX Ingress public IP | 🟢 Free | ⏳ |
| 15.8b.3 | ADD rate limiting policy — 100 calls/min per user | 🟢 Free | ⏳ |
| 15.8b.4 | MAKE NGINX internal only — remove public IP from NGINX | 🟢 Free | ⏳ |
| 15.8b.5 | TEST — Internet → APIM → NGINX → Pod end to end | 🟢 Free | ⏳ |
| 15.8b.6 | EXPLORE developer portal — browse + test APIs | 🟢 Free | ⏳ |

---

#### Stage 9 — Azure Container Apps (ACA) — Same App, Simpler Platform
> AKS is now understood deeply. Now see how ACA abstracts all that complexity away.
> Your company uses ACA — this stage makes you immediately productive on day one at work.
> Same 4 microservices, same ACR images — deployed in a fraction of the YAML.

```
LEARN: ACA vs AKS — what ACA manages for you vs what AKS makes you manage manually
LEARN: ACA Environment — the equivalent of a K8s Namespace + cluster combined
LEARN: Container App — the equivalent of a K8s Deployment + Service + Ingress in one resource
LEARN: Revisions — immutable snapshots (like K8s ReplicaSets) — enable blue/green deploys
LEARN: Traffic splitting — 90% → revision-1, 10% → revision-2 (canary deploys built-in)
LEARN: Scale rules — KEDA built-in — scale by HTTP requests, queue depth, CPU, cron
LEARN: Scale to zero — 0 requests = 0 running containers = ₹0 idle cost
LEARN: Dapr integration — built-in service invocation, pub/sub, state (no Istio needed)
LEARN: Managed identity in ACA — pod-level Azure identity without Workload Identity complexity
LEARN: ACA Secrets — equivalent of K8s Secrets, referenced in env vars
LEARN: ACA Ingress — internal (ClusterIP equivalent) vs external (LoadBalancer equivalent)
LEARN: ACA vs AKS cost model — per vCPU-second vs per node-hour
LEARN: Service-to-service DNS — Container App name IS the internal hostname (http://customer-api) — no Service YAML needed
LEARN: Health probes — liveness (restart if dead), readiness (wait if not ready), startup (grace period for slow starts)
LEARN: Log streaming — az containerapp logs show --follow + portal Log stream + Log Analytics KQL queries
LEARN: Workload Profiles vs Consumption — Consumption=serverless/scale-to-zero, Workload=dedicated/always-on (companies use both)
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.9.1 | CREATE ACA Environment — eshop-env (equivalent of K8s cluster) | 🟢 Free tier | ⏳ |
| 15.9.2 | DEPLOY Catalog.API — Container App using ACR image | 🟢 Free tier | ⏳ |
| 15.9.3 | DEPLOY Customer.API — Container App, internal ingress only | 🟢 Free tier | ⏳ |
| 15.9.4 | DEPLOY Ordering.API — Container App, internal ingress only | 🟢 Free tier | ⏳ |
| 15.9.5 | DEPLOY Identity.API — Container App, external ingress for login | 🟢 Free tier | ⏳ |
| 15.9.6 | CONFIGURE ACA Secrets — connection strings, JWT keys as ACA secrets | 🟢 Free | ⏳ |
| 15.9.7 | CONFIGURE scale-to-zero — HTTP trigger, min replicas = 0 | 🟢 Free | ⏳ |
| 15.9.8 | VERIFY service-to-service DNS — Ordering.API calls http://customer-api internally (no IP, no config) | 🟢 Free | ⏳ |
| 15.9.9 | ADD health probes — liveness + readiness on all 4 Container Apps | 🟢 Free | ⏳ |
| 15.9.10 | DEBUG with log streaming — az containerapp logs show + portal live tail | 🟢 Free | ⏳ |
| 15.9.11 | TEST end-to-end — same app working in ACA with no kubectl required | ⏳ |
| 15.9.12 | COMPARE — side-by-side: AKS YAML count vs ACA config lines | 🟢 Free | ⏳ |
| 15.9.13 | ADD GitHub Actions deploy step — push to ACR → ACA auto-pulls new revision | 🟢 Free | ⏳ |

---

#### Stage 10 — Entra ID (Azure AD) — Enterprise Login for Admins
> App is live in AKS. Redirect URIs are real. Now add "Login with Microsoft" for admin users.
> Entra ID = Azure Active Directory rebranded. Used by 95% of Fortune 500 companies.
> Admin staff logs in with their Microsoft/organizational account — no separate password needed.

```
LEARN: Entra ID vs Auth0 — Microsoft's own identity platform, built into Azure ecosystem
LEARN: App Registration — how you tell Azure about your app (client ID, redirect URI, scopes)
LEARN: Authorization Code Flow + PKCE — same flow we used with Auth0, now with Microsoft
LEARN: id_token claims — oid (object ID), preferred_username, name, roles from Entra
LEARN: Tenant types — Single-tenant (one org only) vs Multi-tenant (any Microsoft account)
LEARN: Microsoft Graph API — fetch user profile, group memberships from Entra
LEARN: Admin Consent — why enterprise apps need IT admin to grant permissions
LEARN: Entra vs B2C — Entra = employees/internal, B2C = customers/external (key distinction!)
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.10.1 | CREATE App Registration in Entra ID — eShop-Admin app | 🟢 Free | ⏳ |
| 15.10.2 | SET Redirect URI — https://app.eshop.dev/auth/microsoft/callback (SWA URL) | 🟢 Free | ⏳ |
| 15.10.3 | CONFIGURE Identity.API — add Entra ID as second login provider | 🟢 Free | ⏳ |
| 15.10.4 | ADD "Login with Microsoft" button on LoginPage.tsx (Admin only) | 🟢 Free | ⏳ |
| 15.10.5 | IMPLEMENT callback — exchange Entra code → validate id_token → issue our RS256 JWT | 🟢 Free | ⏳ |
| 15.10.6 | ENFORCE role — Entra login maps to Admin role in our eShop JWT | 🟢 Free | ⏳ |
| 15.10.7 | TEST — admin logs in with Microsoft account → gets Admin JWT → Dashboard accessible | ⏳ |
| 15.10.8 | LEARN Microsoft Graph — fetch admin's display name + email from Entra | 🟢 Free | ⏳ |

---

#### Stage 11 — Azure AD B2C — Consumer Identity for Customers
> B2C = Business-to-Consumer. Azure's dedicated platform for customer-facing identity.
> Can replace our custom email/password + 2FA + Social login with a fully managed Azure service.
> Used by: ASOS, Heineken, Maersk — large consumer apps at millions of users scale.

```
LEARN: B2C vs Entra ID — B2C = external customers, Entra = internal employees (never mix!)
LEARN: B2C Tenant — separate Azure tenant just for B2C (not your main Azure subscription tenant)
LEARN: User Flows — pre-built policies: SignUpSignIn, PasswordReset, ProfileEdit
LEARN: Custom Policies — XML-based, full control over every step of the auth journey
LEARN: B2C Token — JWT issued by B2C with custom claims (we map it to our RS256 JWT)
LEARN: B2C + Social providers — B2C natively handles Google, GitHub, Facebook without Auth0
LEARN: B2C pricing — first 50,000 MAU (Monthly Active Users) FREE
LEARN: B2C vs Auth0 — B2C is Azure-native (no 3rd party), Auth0 is simpler setup but paid at scale
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.11.1 | CREATE Azure AD B2C Tenant — eshopb2c.onmicrosoft.com | 🟢 Free | ⏳ |
| 15.11.2 | REGISTER eShop app in B2C tenant (client ID + redirect URI) | 🟢 Free | ⏳ |
| 15.11.3 | CREATE SignUpSignIn User Flow — email + password + email OTP verification | 🟢 Free | ⏳ |
| 15.11.4 | ADD Google as social provider in B2C (replaces our Auth0 Google login) | 🟢 Free | ⏳ |
| 15.11.5 | CONFIGURE Identity.API — validate B2C JWT, map claims to our ApplicationUser | 🟢 Free | ⏳ |
| 15.11.6 | UPDATE LoginPage.tsx — "Continue with Microsoft B2C" customer flow | 🟢 Free | ⏳ |
| 15.11.7 | CREATE PasswordReset User Flow — self-service password reset via B2C | 🟢 Free | ⏳ |
| 15.11.8 | TEST — customer signs up via B2C → gets our RS256 JWT → browses + orders | ⏳ |
| 15.11.9 | COMPARE flows — Auth0 (Phase 14) vs Azure AD B2C — pros/cons documented | 🟢 Free | ⏳ |

---

#### Stage 12 — Istio Service Mesh (PROMISED in Phase 12.7!)
> Phase 12.7 explicitly deferred: "Internal service auth: NONE intentionally — Istio mTLS in Phase 15"
> This delivers that promise. Pod-to-pod traffic is now encrypted and identity-verified.

```
LEARN: Service mesh — sidecar proxy pattern, why it works at the network level
LEARN: Istio architecture — istiod (control plane), Envoy proxy sidecar (data plane)
LEARN: mTLS — every pod gets a certificate, pods cryptographically prove identity
LEARN: PeerAuthentication — enforce STRICT mTLS across all pods in namespace
LEARN: AuthorizationPolicy — only Ordering.API can call Customer.API (zero-trust)
LEARN: Traffic management — retries, circuit breaker, timeouts (resilience built-in)
LEARN: Kiali dashboard — live visual service graph, see actual pod-to-pod traffic
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.12.1 | INSTALL Istio on AKS (istioctl install) | 🟢 Free | ⏳ |
| 15.12.2 | LABEL eshop namespace — istio-injection=enabled (sidecars auto-injected) | 🟢 Free | ⏳ |
| 15.12.3 | APPLY PeerAuthentication — STRICT mTLS in eshop namespace | 🟢 Free | ⏳ |
| 15.12.4 | APPLY AuthorizationPolicy — only Ordering.API allowed to call Customer.API | 🟢 Free | ⏳ |
| 15.12.5 | TEST mTLS — verify pod-to-pod calls are encrypted (kubectl exec curl test) | 🟢 Free | ⏳ |
| 15.12.6 | INSTALL Kiali → view live service graph (Ordering → Customer → Identity) | 🟢 Free | ⏳ |
| 15.12.7 | ADD circuit breaker — if Catalog.API fails 5x → stop calling temporarily | 🟢 Free | ⏳ |

---

#### Stage 13 — Azure Workload Identity + KEDA
> Workload Identity: pod-level Azure identity (replaces deprecated Pod Managed Identity).
> KEDA: scale pods by Service Bus queue depth — not CPU. Modern event-driven scaling.

```
LEARN: Workload Identity — why VM-level Managed Identity is not granular enough for pods
LEARN: Federated credentials — K8s ServiceAccount bound to Azure Managed Identity
LEARN: KEDA — Kubernetes Event-Driven Autoscaling, works with Service Bus / HTTP / Cron
LEARN: ScaledObject — KEDA CRD that watches a trigger and adjusts pod count
LEARN: KEDA vs HPA — CPU-based (HPA) vs event-based (KEDA) — both work together
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.13.1 | ENABLE Workload Identity on AKS cluster | 🟢 Free | ⏳ |
| 15.13.2 | CREATE Managed Identity per service — mi-catalog, mi-ordering, etc. | 🟢 Free | ⏳ |
| 15.13.3 | BIND K8s ServiceAccount → Azure Managed Identity (federated credential) | 🟢 Free | ⏳ |
| 15.13.4 | ASSIGN KV Secrets User role — each identity reads only its own secrets | 🟢 Free | ⏳ |
| 15.13.5 | INSTALL KEDA on AKS | 🟢 Free | ⏳ |
| 15.13.6 | CREATE ScaledObject — Catalog.API scales by order-placed queue depth | 🟢 Free | ⏳ |
| 15.13.7 | TEST KEDA — push 50 messages to Service Bus → watch pods scale up | ⏳ |

---

#### Stage 14 — Observability (App Insights + Log Analytics)
> Distributed tracing across all 4 pods. Centralized logs. Visual service map.

```
LEARN: Distributed tracing — one request spans 4 services, trace ID follows it end-to-end
LEARN: Application Insights — traces, metrics, exceptions, live metrics stream
LEARN: Log Analytics Workspace — raw pod logs (stdout/stderr) queryable in one place via KQL
LEARN: Container Insights — AKS-native monitoring (node CPU, pod restarts, OOM kills)
LEARN: OpenTelemetry — already wired via Aspire ServiceDefaults, configure for production
LEARN: KQL basics — Kusto Query Language for searching logs
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.14.1 | CREATE Application Insights — appi-eshop-dev | 🟢 Free | ⏳ |
| 15.14.2 | CREATE Log Analytics Workspace — law-eshop-dev | 🟢 Free | ⏳ |
| 15.14.3 | CONNECT App Insights → Log Analytics (unified backend) | 🟢 Free | ⏳ |
| 15.14.4 | ENABLE Container Insights on AKS (node + pod metrics) | 🟢 Free | ⏳ |
| 15.14.5 | CONFIGURE OpenTelemetry in all 4 services → sends traces to App Insights | 🟢 Free | ⏳ |
| 15.14.6 | TEST — place an order → view full distributed trace across all 4 services in App Insights | ⏳ |
| 15.14.7 | WRITE KQL query — find all failed requests in last 1 hour across all services | 🟢 Free | ⏳ |
| 15.14.8 | CREATE Azure Monitor Alert — alert if any service has >5 errors/min | 🟢 Free | ⏳ |

---

#### Stage 15 — Helm Charts
> Replace raw YAML with reusable, versioned, parameterized packages.

```
LEARN: Helm structure — Chart.yaml (metadata), values.yaml (parameters), templates/ (YAML + Go templating)
LEARN: Templating — {{ .Values.image.tag }}, {{ .Release.Name }}, range, if/else
LEARN: helm upgrade --install — idempotent (safe to run on first deploy AND updates)
LEARN: Values override — helm upgrade --set image.tag=1.2.0 (CI/CD uses this per deploy)
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.15.1 | CREATE Helm chart for Catalog.API (convert existing YAML to templates) | 🟢 Free | ⏳ |
| 15.15.2 | CREATE Helm charts for Customer.API, Ordering.API, Identity.API | 🟢 Free | ⏳ |
| 15.15.3 | UPDATE CI/CD pipelines — replace kubectl apply with helm upgrade --install | 🟢 Free | ⏳ |
| 15.15.4 | TEST — helm upgrade with new image tag → zero-downtime rolling update | ⏳ |

---

#### Stage 16 — DNS + SSL + Azure Front Door
> HTTPS everywhere. Custom domain. Global CDN + WAF.

```
LEARN: cert-manager — K8s operator that auto-provisions + renews Let's Encrypt SSL certs
LEARN: ClusterIssuer — cert-manager config pointing to Let's Encrypt ACME endpoint
LEARN: Azure DNS Zone — manage DNS records for a domain in Azure
LEARN: Azure Front Door — global CDN, WAF, DDoS protection, geo-routing in front of AKS
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.16.1 | INSTALL cert-manager on AKS | 🟢 Free | ⏳ |
| 15.16.2 | CREATE ClusterIssuer — Let's Encrypt production | 🟢 Free | ⏳ |
| 15.16.3 | UPDATE Ingress YAML — TLS block → cert-manager auto-provisions cert | 🟢 Free | ⏳ |
| 15.16.4 | CREATE Azure DNS Zone + A record → AKS Ingress IP | 🟡 ~₹40/mo | ⏳ |
| 15.16.5 | CREATE Azure Front Door — global entry point for AKS + Static Web Apps | 🔴 Delete after learning | ⏳ |
| 15.16.6 | TEST HTTPS — https://api.eshop.dev/api/catalog → 200 OK, padlock shows | ⏳ |
| 15.16.7 | DELETE Front Door after learning (expensive to keep running) | 🔴 Delete | ⏳ |

---

#### Stage 17 — Azure Load Testing
> Stress test the running system. Prove HPA and KEDA work under real load.
> See pods scale in real time in Azure portal.

```
LEARN: Azure Load Testing — JMeter-based, runs from Azure, no local setup needed
LEARN: Load test scenarios — ramp up, sustained load, spike test
LEARN: What to measure — P95 latency, error rate, throughput (requests/sec)
LEARN: Reading AKS metrics under load — pod count, CPU, memory in Container Insights
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.17.1 | CREATE Azure Load Testing resource | 🟢 Free (50 VUs/mo free) | ⏳ |
| 15.17.2 | WRITE load test — 100 concurrent users GET /api/catalog for 5 minutes | 🟢 Free | ⏳ |
| 15.17.3 | RUN test — watch HPA scale Catalog.API pods 1→3 in real time | ⏳ |
| 15.17.4 | PUSH 100 messages to Service Bus — watch KEDA scale consumers | ⏳ |

---

#### Stage 18 — GitOps with ArgoCD
> The 2026 industry standard for Kubernetes deployments.
> Git is the single source of truth — AKS state always matches Git.

```
LEARN: GitOps — push-based CI/CD vs pull-based GitOps (why GitOps wins at scale)
LEARN: ArgoCD architecture — runs inside AKS, watches Git repo, pulls + applies changes
LEARN: Sync policies — auto-sync + self-heal (drift detected → auto-corrected)
LEARN: Rollback — git revert → ArgoCD detects → rolls back AKS automatically
LEARN: App of Apps pattern — one ArgoCD Application manages all microservice apps
LEARN: Who uses GitOps — Netflix, Spotify, Airbnb — every serious K8s shop in 2026
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.18.1 | CREATE k8s/ folder in repo — move all YAML + Helm charts here | 🟢 Free | ⏳ |
| 15.18.2 | INSTALL ArgoCD on AKS | 🟢 Free | ⏳ |
| 15.18.3 | CREATE ArgoCD Application — watches k8s/ folder in GitHub repo | 🟢 Free | ⏳ |
| 15.18.4 | UPDATE CI/CD — pipelines push image tag to Git, ArgoCD deploys to AKS | 🟢 Free | ⏳ |
| 15.18.5 | TEST drift detection — manually delete a pod → ArgoCD detects + recreates | ⏳ |
| 15.18.6 | TEST rollback — git revert bad commit → ArgoCD auto-rolls back AKS | ⏳ |

---

#### Stage 19 — Multi-Environment (DEV → STAGING → PROD)
> Full promote flow with approval gates. Real team workflow.

```
LEARN: GitHub Environments — protection rules, required reviewers, wait timers
LEARN: Approval gates — manual approve before PROD deploy
LEARN: App Config labels — dev / staging / prod — same key, different value per env
LEARN: Promote flow — same Docker image promoted across envs (NO rebuild between envs!)
```

| # | What | Cost | Status |
|---|------|------|--------|
| 15.19.1 | CREATE GitHub Environments — dev, staging, prod | 🟢 Free | ⏳ |
| 15.19.2 | ADD approval gate — prod environment requires manual approval | 🟢 Free | ⏳ |
| 15.19.3 | ADD App Config labels per environment — dev / staging / prod values | 🟢 Free | ⏳ |
| 15.19.4 | UPDATE pipelines — PR → DEV auto, merge → STAGING auto, PROD needs approval | 🟢 Free | ⏳ |
| 15.18.5 | TEST full flow — push → auto deploys DEV → approve → deploys PROD | ⏳ |

---

```
Phase 15 — Cost Summary:
  Stages 1–7   → ₹0          (local + Azure free tier only)
  Stage 5 ACR  → ~₹400/mo    (Basic ACR)
  Stage 8 AKS  → ~₹2,500/mo  (1 node B2s — STOP NODE when not studying → ₹0)
  Stage 9 Entra ID  → ₹0     (Free — App Registration only)
  Stage 10 B2C → ₹0          (Free — first 50,000 MAU/mo free)
  Stage 15 DNS → ~₹40/mo
  Stage 15 Front Door → delete after learning
  Total while studying  → ~₹3,000/mo
  Total when AKS stopped → ~₹440/mo (ACR + DNS only)

Phase 15 — What you will be able to say when done:
  Containerized 4 microservices with production-grade multi-stage Dockerfiles
  Deployed to AKS with NGINX Ingress, Helm charts, HPA + KEDA autoscaling
  Admin staff log in with Microsoft (Entra ID) — no separate password, enterprise-grade
  Customers use Azure AD B2C — fully managed identity, Google social login, self-service reset
  Pods communicate via mTLS — Istio enforces zero-trust service identity (PROMISED in Phase 12.7!)
  No secrets in code, YAML, or env vars — Key Vault + Workload Identity
  CI/CD scans images for CVEs with Trivy before any image reaches production
  Pods scale by Service Bus queue depth, not just CPU (KEDA)
  K8s state driven by Git — ArgoCD detects drift and self-heals (GitOps)
  Full distributed traces visible across all 4 services in App Insights
  Load tested — system handles 500 concurrent users, pods autoscale visibly
  → Senior cloud engineer profile
```

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
