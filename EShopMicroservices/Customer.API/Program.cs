using Azure.Identity;
using Customer.API.GrpcServices;
using Customer.API.Middleware;
using Customer.Core.Behaviors;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Azure App Configuration (Azure environments only) ────────────────────────
var appConfigEndpoint = builder.Configuration["AppConfig:Endpoint"];
if (!string.IsNullOrEmpty(appConfigEndpoint))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
               .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
    });
}

builder.AddServiceDefaults();

// ── Kestrel: separate ports for REST (HTTP/1.1) and gRPC (HTTP/2 h2c) ───────
// CRITICAL: per Microsoft docs, "Http1AndHttp2" on the SAME port CANNOT work
// over plain http:// because protocol negotiation requires TLS+ALPN. Without
// TLS, Kestrel falls back to HTTP/1.1 and rejects gRPC with HTTP_1_1_REQUIRED.
// Solution: dedicated HTTP/2-only port for gRPC, separate HTTP/1.1 port for REST.
// In production (HTTPS), a single port with Http1AndHttp2 works via ALPN.
builder.WebHost.UseUrls();
var inDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
builder.WebHost.ConfigureKestrel(options =>
{
    if (inDocker)
    {
        // 0.0.0.0 = all interfaces — required for Docker container networking
        options.ListenAnyIP(5011, lo => lo.Protocols = HttpProtocols.Http1);
        options.ListenAnyIP(5022, lo => lo.Protocols = HttpProtocols.Http2);
    }
    else
    {
        // 127.0.0.1 = localhost only — avoids Windows Firewall popup in local dev
        options.ListenLocalhost(5011, lo => lo.Protocols = HttpProtocols.Http1);
        options.ListenLocalhost(5022, lo => lo.Protocols = HttpProtocols.Http2);
    }
});

// ── CORS — origins come from appsettings.json, never hardcoded ──────────────
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("ReactFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// gRPC — adds gRPC middleware. Customer.API now serves BOTH REST + gRPC on the same port.
// REST: HTTP/1.1, JSON  → Swagger, mobile apps, external clients
// gRPC: HTTP/2, Protobuf → internal service calls (Ordering.API), 5-10x faster
builder.Services.AddGrpc();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Customer API", Version = "v1" }));

// MediatR — register all handlers from Customer.Core
// + ValidationBehavior runs BEFORE every command handler automatically!
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Customer.Core.AssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// FluentValidation — scan Customer.Core assembly for all validators
builder.Services.AddValidatorsFromAssembly(typeof(Customer.Core.AssemblyMarker).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure (DbContext, Repositories, Seeder, EventPublisher)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();

// ── Database migration + Seed (runs in ALL environments) ─────────────────────
// Seeder is idempotent, so it's safe to run on every startup, including
// Production. This avoids needing a separate SDK-based init container
// just to run "dotnet ef database update".
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<CustomerDataSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("ReactFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API v1"));
}

app.MapControllers();

// gRPC endpoint — routes /customer.CustomerGrpc/* to CustomerGrpcService.
// Only reachable on the HTTP/2 port (5022); REST controllers are reachable on 5011.
app.MapGrpcService<CustomerGrpcService>();

app.Run();
