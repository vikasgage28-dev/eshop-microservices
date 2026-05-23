using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;
using System.Net.Http.Json;

namespace Ordering.Infrastructure.HttpClients
{
    public class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerServiceClient> _logger;

        public CustomerServiceClient(HttpClient httpClient, ILogger<CustomerServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId)
        {
            var url = $"/api/customers/{customerId}";
            _logger.LogInformation("[CUSTOMER CLIENT] Calling {BaseAddress}{Url}",
                _httpClient.BaseAddress, url);
            try
            {
                var response = await _httpClient.GetAsync(url);
                _logger.LogInformation("[CUSTOMER CLIENT] Response: {StatusCode}", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[CUSTOMER CLIENT] Non-success response body: {Body}", body);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<CustomerDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CUSTOMER CLIENT] Exception calling Customer.API");
                return null;
            }
        }
    }
}