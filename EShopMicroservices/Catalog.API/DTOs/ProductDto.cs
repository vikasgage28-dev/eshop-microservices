namespace Catalog.API.DTOs
{
    // What the API RETURNS for a product
    // CategoryName included directly — client doesn't need to make a second call!
    public record ProductDto(
        Guid    Id,
        string  Name,
        string  Description,
        decimal Price,
        int     Stock,
        bool    IsActive,
        DateTime CreatedAt,
        Guid    CategoryId,
        string  CategoryName);

    // What the API RECEIVES to create a product
    public record CreateProductRequest(
        string  Name,
        string  Description,
        decimal Price,
        int     Stock,
        Guid    CategoryId);

    // What the API RECEIVES to update a product
    public record UpdateProductRequest(
        string  Name,
        string  Description,
        decimal Price,
        int     Stock,
        Guid    CategoryId,
        bool    IsActive);

    // Paged result wrapper
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int            TotalCount,
        int            Page,
        int            PageSize,
        int            TotalPages);
}
