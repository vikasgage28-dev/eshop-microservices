using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Data
{
    public class IdentityDataSeeder
    {
        private readonly UserManager<AppIdentityUser>  _userManager;
        private readonly RoleManager<IdentityRole>     _roleManager;
        private readonly ILogger<IdentityDataSeeder>   _logger;

        public IdentityDataSeeder(
            UserManager<AppIdentityUser>  userManager,
            RoleManager<IdentityRole>     roleManager,
            ILogger<IdentityDataSeeder>   logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger      = logger;
        }

        public async Task SeedAsync()
        {
            // Seed roles
            foreach (var role in new[] { "Admin", "Customer" })
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    _logger.LogInformation("Created role: {Role}", role);
                }
            }

            // Seed admin user (idempotent)
            const string adminEmail = "admin@eshop.com";
            if (await _userManager.FindByEmailAsync(adminEmail) is null)
            {
                var admin = new AppIdentityUser
                {
                    UserName  = adminEmail,
                    Email     = adminEmail,
                    FirstName = "System",
                    LastName  = "Admin",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(admin, "Admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    _logger.LogInformation("Seeded admin user: {Email}", adminEmail);
                }
            }

            // Seed sample customer
            const string customerEmail = "alice@eshop.com";
            if (await _userManager.FindByEmailAsync(customerEmail) is null)
            {
                var customer = new AppIdentityUser
                {
                    UserName  = customerEmail,
                    Email     = customerEmail,
                    FirstName = "Alice",
                    LastName  = "Smith",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(customer, "Customer@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(customer, "Customer");
                    _logger.LogInformation("Seeded customer user: {Email}", customerEmail);
                }
            }
        }
    }
}
