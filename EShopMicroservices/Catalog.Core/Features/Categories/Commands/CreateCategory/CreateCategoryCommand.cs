using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(
        string Name,
        string Description
    ) : IRequest<Category>;
}
