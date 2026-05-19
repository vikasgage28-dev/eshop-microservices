using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id) : IRequest<Category?>;
}
