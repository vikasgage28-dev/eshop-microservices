using Identity.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Identity.Infrastructure.Services
{
    /// <summary>
    /// Validates an Auth0 access token by calling Auth0's /userinfo endpoint.
    /// This is the standard OAuth 2.0 token introspection pattern — we never
    /// trust the frontend's claims; we verify with the Authorization Server.
    /// </summary>
    public class Auth0UserInfoService : ISocialAuthProvider
    {
        private readonly HttpClient      _httpClient;
        private readonly IConfiguration _configuration;

        public Auth0UserInfoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient    = httpClient;
            _configuration = configuration;
        }

        public async Task<SocialUserInfo?> GetUserInfoAsync(string accessToken)
        {
            var domain = _configuration["Auth0:Domain"]
                ?? throw new InvalidOperationException("Auth0:Domain is not configured.");

            // Call Auth0's OIDC /userinfo endpoint — standard endpoint all OIDC providers expose
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"https://{domain}/userinfo");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var info = JsonSerializer.Deserialize<Auth0UserInfoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (info?.Email is null) return null;

            // Auth0 returns full name in "name" — split into first/last
            var nameParts  = (info.Name ?? info.Email).Trim().Split(' ', 2);
            var firstName  = nameParts[0];
            var lastName   = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            return new SocialUserInfo(
                Provider:       "Auth0",
                ProviderUserId: info.Sub ?? string.Empty,
                Email:          info.Email,
                FirstName:      firstName,
                LastName:       lastName,
                Picture:        info.Picture,
                EmailVerified:  info.EmailVerified
            );
        }

        // Matches the JSON shape returned by Auth0's /userinfo endpoint
        private sealed class Auth0UserInfoResponse
        {
            public string? Sub             { get; set; }
            public string? Email           { get; set; }
            public string? Name            { get; set; }
            public string? Picture         { get; set; }

            [JsonPropertyName("email_verified")]
            public bool EmailVerified      { get; set; }
        }
    }
}
