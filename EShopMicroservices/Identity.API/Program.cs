using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.Middleware;
using Identity.Core.Behaviors;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;

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

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// Swagger with JWT lock icon
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Identity API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.ApiKey,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter: Bearer {your JWT token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// MediatR + ValidationBehavior
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Identity.Core.AssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Identity.Core.AssemblyMarker).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure (Identity + EF Core + JwtTokenService + AuthRepository)
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication — RS256 (asymmetric: verify with public key only)
var jwtSettings    = builder.Configuration.GetSection("JwtSettings");
var publicKeyPath  = jwtSettings["PublicKeyPath"] ?? throw new InvalidOperationException("JWT PublicKeyPath not configured.");
var publicKeyPem   = File.ReadAllText(publicKeyPath);
var rsaPublic      = RSA.Create();
rsaPublic.ImportFromPem(publicKeyPem);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings["Issuer"],
        ValidAudience            = jwtSettings["Audience"],
        IssuerSigningKey         = new RsaSecurityKey(rsaPublic)   // public key only — secret never exposed
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.MapDefaultEndpoints();

// ── Database migration + Seed ─────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("ReactFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API v1"));
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
