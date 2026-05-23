using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderingDbContext : DbContext
    {
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerId).IsRequired().HasMaxLength(200);
                entity.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(300);
                entity.Property(o => o.Status).HasConversion<int>();
                entity.Property(o => o.ShippingAddress).HasMaxLength(500);
                entity.Property(o => o.Notes).HasMaxLength(1000);

                // TotalAmount is computed — not stored in DB
                entity.Ignore(o => o.TotalAmount);

                // One Order → many OrderItems
                entity.HasMany(o => o.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");

                // TotalPrice is computed — not stored in DB
                entity.Ignore(i => i.TotalPrice);
            });
        }
    }
}
