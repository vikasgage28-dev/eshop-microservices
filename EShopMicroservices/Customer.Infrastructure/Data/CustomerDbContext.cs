using Customer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

        public DbSet<Core.Entities.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Core.Entities.Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.LastName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(c => c.Email).IsUnique();
                entity.Property(c => c.Phone).HasMaxLength(20);

                // FullName is computed — not stored in DB
                entity.Ignore(c => c.FullName);

                // One Customer → many Addresses
                entity.HasMany(c => c.Addresses)
                      .WithOne(a => a.Customer)
                      .HasForeignKey(a => a.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Street).IsRequired().HasMaxLength(200);
                entity.Property(a => a.City).IsRequired().HasMaxLength(100);
                entity.Property(a => a.State).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Country).IsRequired().HasMaxLength(100);
                entity.Property(a => a.PostalCode).IsRequired().HasMaxLength(20);
            });
        }
    }
}
