namespace Catalog.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Foreign key to Category (proper entity — no longer a raw string!)
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
