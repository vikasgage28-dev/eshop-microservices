namespace Catalog.API.DTOs
{
    // What the API RETURNS for a review
    public record ReviewDto(
        string   Id,
        Guid     ProductId,
        string   UserId,
        string   UserEmail,
        int      Rating,
        string   Comment,
        bool     VerifiedPurchase,
        DateTime CreatedAt);

    // What the API RECEIVES to create a review
    public record CreateReviewRequest(
        Guid   ProductId,
        string UserId,
        string UserEmail,
        int    Rating,
        string Comment,
        bool   VerifiedPurchase);
}
