namespace EShop.Shared.DTOs
{
    public class CreateReviewDto
    {
        public int Rating { get; set; }        // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public bool VerifiedPurchase { get; set; }
    }
}