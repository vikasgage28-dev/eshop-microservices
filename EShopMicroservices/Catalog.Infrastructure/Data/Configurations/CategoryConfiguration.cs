using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        // Hardcoded Guids required for EF Core seed data
        // Guid.NewGuid() in seed = new value every migration! ❌
        public static readonly Guid ElectronicsId = new("a1b2c3d4-e5f6-7890-abcd-ef1234567801");
        public static readonly Guid FurnitureId   = new("a1b2c3d4-e5f6-7890-abcd-ef1234567802");

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);

            builder.HasData(
                new Category { Id = ElectronicsId, Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Id = FurnitureId,   Name = "Furniture",   Description = "Home and office furniture" }
            );
        }
    }
}
