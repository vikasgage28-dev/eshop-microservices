namespace EShop.Shared.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Non-breaking addition - works in V1 and V2!
        // Old clients simply ignore this field
        public string StockStatus => Stock == 0 ? "Out of Stock"
                                   : Stock <= 10 ? "Low Stock"
                                   : "In Stock";
    }
}
