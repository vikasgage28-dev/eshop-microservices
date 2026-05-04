using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EShop.Functions.Functions
{
    public class HealthCheckFunction
    {
        private readonly ILogger<HealthCheckFunction> _logger;

        public HealthCheckFunction(ILogger<HealthCheckFunction> logger)
        {
            _logger = logger;
        }

        [Function("HealthCheck")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")]
            HttpRequestData req)
        {
            _logger.LogInformation("Health check called at: {time}", DateTime.UtcNow);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString($"{{\"status\":\"healthy\",\"service\":\"EShop.Functions\",\"timestamp\":\"{DateTime.UtcNow:O}\"}}");

            return response;
        }
    }
}