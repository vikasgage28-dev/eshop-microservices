using Ordering.Core.Interfaces;
using System.Net.Http.Json;

namespace Ordering.Infrastructure.HttpClients
{
    public class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;

        public CustomerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CustomerDto>(
                    $"/api/customers/{customerId}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}