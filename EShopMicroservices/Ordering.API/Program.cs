using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Middleware;
using Ordering.Core.Behaviors;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using System.Text.Json.Serialization;

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

// ── CORS — origins come from appsettings.json, never hardcoded ──────────────
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("ReactFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Services ────────────────────────────────────────────────────────────────
// JsonStringEnumConverter → Status serializes as "Pending" not 0
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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

// Infrastructure (DbContext, Repositories, Seeder, EventPublisher, gRPC clients)
// ICustomerServiceClient → CustomerGrpcClient is now registered inside AddInfrastructure!
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();

// ── Database migration + Seed (runs in ALL environments) ─────────────────────
// Seeder is idempotent, so it's safe to run on every startup, including
// Production. This avoids needing a separate SDK-based init container
// just to run "dotnet ef database update".
using (var scope = app.Services.CreateScope())
{
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
app.UseCors("ReactFrontend");

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
