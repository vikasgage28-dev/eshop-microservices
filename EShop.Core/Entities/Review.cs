using Newtonsoft.Json;

namespace EShop.Core.Entities
{
    public class Review
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; } = string.Empty;

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; } = string.Empty;

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; } = string.Empty;

        [JsonProperty("verifiedPurchase")]
        public bool VerifiedPurchase { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}