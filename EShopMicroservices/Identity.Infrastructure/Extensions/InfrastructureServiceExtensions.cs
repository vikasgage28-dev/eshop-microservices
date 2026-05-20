using Identity.Core.Interfaces;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // EF Core — SQL Server
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDb")));

            // ASP.NET Core Identity
            services.AddIdentity<AppIdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit           = true;
                options.Password.RequireUppercase        = true;
                options.Password.RequiredLength          = 8;
                options.Password.RequireNonAlphanumeric  = false;
                options.User.RequireUniqueEmail          = true;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            // Services
            services.AddScoped<ITokenService,   JwtTokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Seeder
            services.AddScoped<IdentityDataSeeder>();

            return services;
        }
    }
}
