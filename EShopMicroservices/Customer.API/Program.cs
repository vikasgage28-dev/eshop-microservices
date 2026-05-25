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
builder.AddServiceDefaults();

// ── Kestrel: separate ports for REST (HTTP/1.1) and gRPC (HTTP/2 h2c) ───────
// CRITICAL: per Microsoft docs, "Http1AndHttp2" on the SAME port CANNOT work
// over plain http:// because protocol negotiation requires TLS+ALPN. Without
// TLS, Kestrel falls back to HTTP/1.1 and rejects gRPC with HTTP_1_1_REQUIRED.
// Solution: dedicated HTTP/2-only port for gRPC, separate HTTP/1.1 port for REST.
// In production (HTTPS), a single port with Http1AndHttp2 works via ALPN.
builder.WebHost.UseUrls();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5011, lo => lo.Protocols = HttpProtocols.Http1);    // REST + Swagger
    options.ListenLocalhost(5022, lo => lo.Protocols = HttpProtocols.Http2);    // gRPC (h2c)
});

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

// ── Database migration + Seed (Development only!) ───────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<CustomerDataSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

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
