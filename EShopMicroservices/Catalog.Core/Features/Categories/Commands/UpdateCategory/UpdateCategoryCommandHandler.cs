using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category?>
    {
        private readonly ICategoryRepository _repository;

        public UpdateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category?> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(request.Id, new Category
            {
                Name        = request.Name,
                Description = request.Description
            });
        }
    }
}
