using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Categories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery : IRequest<IEnumerable<Category>>;
}
