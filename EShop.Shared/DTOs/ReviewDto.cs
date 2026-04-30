namespace EShop.Shared.DTOs
{
    public class ReviewDto
    {
        public string Id { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool VerifiedPurchase { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}