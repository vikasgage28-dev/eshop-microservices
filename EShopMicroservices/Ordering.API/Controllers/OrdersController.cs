using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Features.Orders.Commands.CancelOrder;
using Ordering.Core.Features.Orders.Commands.PlaceOrder;
using Ordering.Core.Features.Orders.Queries.GetAllOrders;
using Ordering.Core.Features.Orders.Queries.GetOrderById;
using Ordering.Core.Features.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders.Select(ToDto));
        }

        // GET api/orders/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            if (order is null) return NotFound();
            return Ok(ToDto(order));
        }

        // GET api/orders/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(string customerId)
        {
            var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerId));
            return Ok(orders.Select(ToDto));
        }

        // POST api/orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var order = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, ToDto(order));
        }

        // POST api/orders/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest request)
        {
            var cancelled = await _mediator.Send(new CancelOrderCommand
            {
                OrderId = id,
                Reason = request.Reason
            });

            if (!cancelled) return NotFound();
            return NoContent();
        }

        private static OrderDto ToDto(Order o) => new()
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            CustomerEmail = o.CustomerEmail,
            Status = o.Status,
            OrderDate = o.OrderDate,
            ShippingAddress = o.ShippingAddress,
            Notes = o.Notes,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }
}
