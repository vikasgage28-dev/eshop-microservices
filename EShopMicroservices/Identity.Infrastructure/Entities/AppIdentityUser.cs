using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Entities
{
    /// <summary>
    /// Infrastructure-only user entity that inherits from ASP.NET Core IdentityUser.
    /// Mapped to/from Identity.Core.Entities.ApplicationUser by AuthRepository.
    /// </summary>
    public class AppIdentityUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public string FullName  => $"{FirstName} {LastName}".Trim();
        public DateTime  CreatedAt           { get; set; } = DateTime.UtcNow;
        public string?   RefreshToken        { get; set; }
        public DateTime? RefreshTokenExpiry  { get; set; }
    }
}
