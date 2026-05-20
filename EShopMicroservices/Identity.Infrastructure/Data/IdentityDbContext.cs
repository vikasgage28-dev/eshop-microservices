using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename default ASP.NET Identity tables to cleaner names
            builder.Entity<AppIdentityUser>().ToTable("Users");
        }
    }
}
