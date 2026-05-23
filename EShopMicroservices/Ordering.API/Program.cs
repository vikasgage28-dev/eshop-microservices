using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Middleware;
using Ordering.Core.Behaviors;
using Ordering.Core.Interfaces;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Ordering.Infrastructure.HttpClients;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Ordering API", Version = "v1" }));

// MediatR — register all handlers from Ordering.Core
// + ValidationBehavior runs BEFORE every command handler automatically!
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Ordering.Core.AssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// FluentValidation — scan Ordering.Core assembly for all validators
builder.Services.AddValidatorsFromAssembly(typeof(Ordering.Core.AssemblyMarker).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure (DbContext, Repositories, Seeder, EventPublisher)
builder.Services.AddInfrastructure(builder.Configuration);

// HTTP Client — Customer Service (Aspire Service Discovery resolves "customer-api")
builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
{
    var url = builder.Configuration["ServiceUrls:CustomerApi"]
              ?? "http://customer-api";
    client.BaseAddress = new Uri(url);
});

var app = builder.Build();
app.MapDefaultEndpoints();

// ── Database migration + Seed (Development only!) ───────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    // Step 1: Run EF Core migrations (creates tables if not exist!)
    var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    await db.Database.MigrateAsync();

    // Step 2: Seed sample data (only after tables exist!)
    var seeder = scope.ServiceProvider.GetRequiredService<OrderingDataSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ──────────────────────────────────────────────────────

// Global error handler — MUST be first so it catches all exceptions!
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering API v1"));
}

// Note: UseAuthorization() removed — no [Authorize] attributes here.
// Internal service-to-service calls don't use JWT.
// In AKS (Phase 14), Istio mTLS handles internal auth.
app.MapControllers();

app.Run();
