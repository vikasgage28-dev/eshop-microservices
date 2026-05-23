using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(
        Guid   Id,
        string Name,
        string Description
    ) : IRequest<Category?>;
}
