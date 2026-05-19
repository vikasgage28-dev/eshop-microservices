namespace Catalog.API.DTOs
{
    // What the API RETURNS for a category
    public record CategoryDto(
        Guid   Id,
        string Name,
        string Description);

    // What the API RECEIVES to create a category
    public record CreateCategoryRequest(
        string Name,
        string Description);

    // What the API RECEIVES to update a category
    public record UpdateCategoryRequest(
        string Name,
        string Description);
}
