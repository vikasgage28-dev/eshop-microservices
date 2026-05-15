using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Relationship: Product → Category (many-to-one)
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed data — same products as legacy, now with Guid Ids and CategoryId FK
            builder.HasData(
                new Product
                {
                    Id          = new Guid("b1000001-0000-0000-0000-000000000001"),
                    Name        = "Laptop",
                    Description = "High performance laptop",
                    Price       = 999.99m,
                    Stock       = 10,
                    CategoryId  = CategoryConfiguration.ElectronicsId,
                    CreatedAt   = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id          = new Guid("b1000001-0000-0000-0000-000000000002"),
                    Name        = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse",
                    Price       = 29.99m,
                    Stock       = 50,
                    CategoryId  = CategoryConfiguration.ElectronicsId,
                    CreatedAt   = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id          = new Guid("b1000001-0000-0000-0000-000000000003"),
                    Name        = "Standing Desk",
                    Description = "Adjustable standing desk",
                    Price       = 499.99m,
                    Stock       = 5,
                    CategoryId  = CategoryConfiguration.FurnitureId,
                    CreatedAt   = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id          = new Guid("b1000001-0000-0000-0000-000000000004"),
                    Name        = "Mechanical Keyboard",
                    Description = "RGB mechanical keyboard",
                    Price       = 149.99m,
                    Stock       = 20,
                    CategoryId  = CategoryConfiguration.ElectronicsId,
                    CreatedAt   = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id          = new Guid("b1000001-0000-0000-0000-000000000005"),
                    Name        = "Office Chair",
                    Description = "Ergonomic office chair",
                    Price       = 299.99m,
                    Stock       = 8,
                    CategoryId  = CategoryConfiguration.FurnitureId,
                    CreatedAt   = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
