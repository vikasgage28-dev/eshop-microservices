using MediatR;

namespace Catalog.Core.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;
}
