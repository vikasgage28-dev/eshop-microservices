using Azure.Communication.Email;
using EShop.Infrastructure.Data;
using EShop.Core.Interfaces;
using EShop.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register Email Client
builder.Services.AddSingleton<EmailClient>(sp =>
{
    var connectionString = builder.Configuration["AcsConnection"];
    return new EmailClient(connectionString);
});

builder.Build().Run();