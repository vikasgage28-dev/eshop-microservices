using EShop.Contracts.Protos;
using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;

namespace Ordering.Infrastructure.GrpcClients;

/// <summary>
/// gRPC client — replaces CustomerServiceClient (HTTP/JSON).
/// Implements the SAME ICustomerServiceClient interface so the rest of the
/// application (PlaceOrderCommandHandler) needs ZERO changes.
///
/// Architecture lesson:
///   ICustomerServiceClient lives in Ordering.Core  → the interface
///   CustomerServiceClient  lives in Infrastructure → HTTP implementation
///   CustomerGrpcClient     lives in Infrastructure → gRPC implementation
///   Swap via DI registration in Program.cs — Core never knows the difference!
/// </summary>
public class CustomerGrpcClient : ICustomerServiceClient
{
    private readonly CustomerGrpc.CustomerGrpcClient _grpcClient;
    private readonly ILogger<CustomerGrpcClient> _logger;

    public CustomerGrpcClient(
        CustomerGrpc.CustomerGrpcClient grpcClient,
        ILogger<CustomerGrpcClient> logger)
    {
        _grpcClient = grpcClient;
        _logger     = logger;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId)
    {
        _logger.LogInformation("[GRPC CLIENT] GetCustomer called for id: {Id}", customerId);

        try
        {
            var response = await _grpcClient.GetCustomerAsync(
                new GetCustomerRequest { Id = customerId.ToString() });

            if (!response.Found)
            {
                _logger.LogWarning("[GRPC CLIENT] Customer not found: {Id}", customerId);
                return null;
            }

            _logger.LogInformation("[GRPC CLIENT] Customer found: {Email}", response.Email);

            return new CustomerDto
            {
                Id       = Guid.Parse(response.Id),
                Email    = response.Email,
                FullName = response.FullName
            };
        }
        catch (Grpc.Core.RpcException ex)
        {
            _logger.LogError(ex, "[GRPC CLIENT] gRPC call failed: {Status}", ex.Status);
            return null;
        }
    }
}
