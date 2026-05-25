using Customer.Core.Features.Customers.Queries.GetCustomerById;
using EShop.Contracts.Protos;
using Grpc.Core;
using MediatR;

namespace Customer.API.GrpcServices;

/// <summary>
/// gRPC server endpoint — exposes CustomerGrpc service defined in customer.proto.
/// Runs ALONGSIDE the REST controller — both protocols served on the same port.
///
/// Why MediatR here?
///   Same pattern as REST controller: query → handler → repository.
///   The Core layer is completely unaware of whether the caller is HTTP or gRPC.
///   This is Clean Architecture: transport is an implementation detail.
/// </summary>
public class CustomerGrpcService : CustomerGrpc.CustomerGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomerGrpcService> _logger;

    public CustomerGrpcService(IMediator mediator, ILogger<CustomerGrpcService> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }

    /// <summary>
    /// Called by Ordering.API via gRPC when validating a customer during PlaceOrder.
    /// Replaces the HTTP GET /api/customers/{id} call — same data, 5-10x faster.
    /// </summary>
    public override async Task<CustomerResponse> GetCustomer(
        GetCustomerRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("[GRPC SERVER] GetCustomer called for id: {Id}", request.Id);

        if (!Guid.TryParse(request.Id, out var customerId))
        {
            _logger.LogWarning("[GRPC SERVER] Invalid GUID format: {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid customer id: {request.Id}"));
        }

        var customer = await _mediator.Send(new GetCustomerByIdQuery(customerId), context.CancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("[GRPC SERVER] Customer not found: {Id}", request.Id);
            // Return a "not found" response — caller checks the 'found' flag
            return new CustomerResponse { Found = false };
        }

        _logger.LogInformation("[GRPC SERVER] Customer found: {Email}", customer.Email);

        return new CustomerResponse
        {
            Id       = customer.Id.ToString(),
            Email    = customer.Email,
            FullName = customer.FullName,
            Found    = true
        };
    }
}
