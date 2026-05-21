using Customer.API.Middleware;
using Customer.Core.Behaviors;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

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

app.UseAuthorization();
app.MapControllers();

app.Run();
