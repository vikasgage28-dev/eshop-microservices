var builder = DistributedApplication.CreateBuilder(args);

// ── Port assignments ─────────────────────────────────────────────────────────
// WithEndpoint() MODIFIES the existing "http" endpoint from launchSettings.json.
// WithHttpEndpoint() would CREATE a duplicate "http" endpoint → conflict!
// Fixed ports prevent corporate-DNS resolving service names to external proxies.
//
// ── Service dependency map ───────────────────────────────────────────────────
// Add WithReference(x) + WaitFor(x) ONLY when this service makes sync HTTP calls to x.
// Async messaging (StorageQueue/ServiceBus) does NOT need WithReference or WaitFor.
//
//   catalog-api  → (none)         standalone, no sync HTTP dependencies
//   customer-api → (none)         standalone, no sync HTTP dependencies
//   identity-api → (none)         standalone, no sync HTTP dependencies
//   ordering-api → customer-api   calls GET /api/customers/{id} on every PlaceOrder
// ─────────────────────────────────────────────────────────────────────────────

var catalogApi  = builder.AddProject<Projects.Catalog_API>("catalog-api")
                         .WithEndpoint("http", e => e.Port = 5010);

// Customer.API exposes TWO endpoints:
//   "http" on 5011 → HTTP/1.1 (REST + Swagger)
//   "grpc" on 5022 → HTTP/2  (gRPC h2c)
// IsProxied=false bypasses Aspire's HTTP/1.1-only reverse proxy so gRPC's HTTP/2
// connection reaches Kestrel directly. Service discovery still works via WithReference.
var customerApi = builder.AddProject<Projects.Customer_API>("customer-api")
                         .WithEndpoint("http", e => {
                             e.Port = 5011;
                             e.IsProxied = false;
                         })
                         .WithEndpoint("grpc", e => {
                             e.Port = 5022;
                             e.UriScheme = "http";
                             e.IsProxied = false;
                         });

var identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
                         .WithEndpoint("http", e => e.Port = 5013);

// ordering-api calls customer-api synchronously → WithReference (discovery) + WaitFor (startup order)
builder.AddProject<Projects.Ordering_API>("ordering-api")
       .WithEndpoint("http", e => e.Port = 5012)
       .WithReference(customerApi)
       .WaitFor(customerApi);

builder.Build().Run();