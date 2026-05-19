using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler
        : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoriesQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
