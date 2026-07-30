using Customer.API.DTOs;
using Customer.Core.Entities;
using Customer.Core.Features.Addresses.Commands.AddAddress;
using Customer.Core.Features.Addresses.Commands.DeleteAddress;
using Customer.Core.Features.Customers.Commands.CreateCustomer;
using Customer.Core.Features.Customers.Commands.DeleteCustomer;
using Customer.Core.Features.Customers.Commands.UpdateCustomer;
using Customer.Core.Features.Customers.Queries.GetAllCustomers;
using Customer.Core.Features.Customers.Queries.GetCustomerByEmail;
using Customer.Core.Features.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            return Ok(customers.Select(ToDto));
        }

        // GET api/customers/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
            if (customer is null) return NotFound();
            return Ok(ToDto(customer));
        }

        // GET api/customers/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<CustomerDto>> GetByEmail(string email)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
            if (customer is null) return NotFound();
            return Ok(ToDto(customer));
        }

        // POST api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand command)
        {
            var customer = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, ToDto(customer));
        }

        // PUT api/customers/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> Update(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            command.Id = id;
            var customer = await _mediator.Send(command);
            if (customer is null) return NotFound();
            return Ok(ToDto(customer));
        }

        // DELETE api/customers/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteCustomerCommand { Id = id });
            if (!deleted) return NotFound();
            return NoContent();
        }

        // POST api/customers/{id}/addresses
        [HttpPost("{id:guid}/addresses")]
        public async Task<ActionResult<AddressDto>> AddAddress(Guid id, [FromBody] AddAddressCommand command)
        {
            command.CustomerId = id;
            var address = await _mediator.Send(command);
            if (address is null) return NotFound();
            return Ok(ToAddressDto(address));
        }

        // DELETE api/customers/{id}/addresses/{addressId}
        [HttpDelete("{id:guid}/addresses/{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid id, Guid addressId)
        {
            var deleted = await _mediator.Send(new DeleteAddressCommand { CustomerId = id, AddressId = addressId });
            if (!deleted) return NotFound();
            return NoContent();
        }

        private static CustomerDto ToDto(Core.Entities.Customer c) => new()
        {
            Id        = c.Id,
            FirstName = c.FirstName,
            LastName  = c.LastName,
            FullName  = c.FullName,
            Email     = c.Email,
            Phone     = c.Phone,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            Addresses = c.Addresses.Select(ToAddressDto).ToList()
        };

        private static AddressDto ToAddressDto(Address a) => new()
        {
            Id         = a.Id,
            Street     = a.Street,
            City       = a.City,
            State      = a.State,
            Country    = a.Country,
            PostalCode = a.PostalCode,
            IsDefault  = a.IsDefault
        };
    }
}
