using MediatR;

namespace Ordering.Core.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
