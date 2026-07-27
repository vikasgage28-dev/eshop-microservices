using Azure.Identity;
using Catalog.API.Middleware;
using Catalog.Core.Behaviors;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Azure App Configuration (Azure environments only) ────────────────────────
// Locally → User Secrets are used (this block is skipped)
// AKS     → AppConfig__Endpoint env var is set → reads from App Config + Key Vault
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

// ── CORS — origins come from appsettings.json, never hardcoded ──
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("ReactFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Services ────────────────────────────────────────────────────
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Catalog API", Version = "v1" }));

// MediatR — register all handlers from Catalog.Core
// + ValidationBehavior runs BEFORE every command handler automatically!
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Catalog.Core.AssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// FluentValidation — scan Catalog.Core assembly for all validators
builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Core.AssemblyMarker).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure (DbContext, Cosmos, Repositories, Seeder)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();

// ── Database migration + Seed (runs in ALL environments) ────────
// Seeder is idempotent, so it's safe to run on every startup, including
// Production. This avoids needing a separate SDK-based init container
// just to run "dotnet ef database update".
using (var scope = app.Services.CreateScope())
{
    // Step 1: Run EF Core migrations (creates tables if not exist!)
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await db.Database.MigrateAsync();

    // Step 2: Seed data (only after tables exist!)
    var seeder = scope.ServiceProvider.GetRequiredService<CatalogDataSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ──────────────────────────────────────────

// Global error handler — MUST be first so it catches all exceptions!
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("ReactFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1"));
}

// Note: UseAuthorization() removed — no [Authorize] attributes here.
// Internal service-to-service calls don't use JWT.
// In AKS (Phase 14), Istio mTLS handles internal auth.
app.MapControllers();

app.Run();
