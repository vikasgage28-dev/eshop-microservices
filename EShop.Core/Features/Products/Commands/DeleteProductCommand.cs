using MediatR;

namespace EShop.Core.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        // Only needs Id to delete
        // Returns bool = success or not
        public int Id { get; set; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}