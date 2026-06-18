using Identity.Core.Entities;
using Identity.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings    = _configuration.GetSection("JwtSettings");
            var privateKeyPath = jwtSettings["PrivateKeyPath"] ?? throw new InvalidOperationException("JWT PrivateKeyPath not configured.");
            var issuer         = jwtSettings["Issuer"]         ?? "Identity.API";
            var audience       = jwtSettings["Audience"]       ?? "EShopClients";
            var expiryMins     = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            // ── Load RSA private key from PEM file ─────────────────────────────
            var pemContent = File.ReadAllText(privateKeyPath);
            using var rsa  = RSA.Create();
            rsa.ImportFromPem(pemContent);

            var key   = new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters: true));
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,   user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new("fullName", user.FullName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(expiryMins),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
