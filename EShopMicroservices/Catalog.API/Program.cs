using Catalog.API.Middleware;
using Catalog.Core.Behaviors;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// ── Database migration + Seed (Development only!) ───────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1"));
}

app.UseAuthorization();
app.MapControllers();

app.Run();
