using EShop.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.Property(p => p.Category).IsRequired().HasMaxLength(50);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasData(
                    new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 999.99m, Stock = 10, Category = "Electronics", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    new Product { Id = 2, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 29.99m, Stock = 50, Category = "Electronics", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    new Product { Id = 3, Name = "Standing Desk", Description = "Adjustable standing desk", Price = 499.99m, Stock = 5, Category = "Furniture", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    new Product { Id = 4, Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard", Price = 149.99m, Stock = 20, Category = "Electronics", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    new Product { Id = 5, Name = "Office Chair", Description = "Ergonomic office chair", Price = 299.99m, Stock = 8, Category = "Furniture", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
                );
            });
        }
    }
}
