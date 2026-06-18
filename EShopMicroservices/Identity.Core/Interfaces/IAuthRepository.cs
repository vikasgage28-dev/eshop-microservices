using Identity.Core.Entities;

namespace Identity.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<(bool Success, string? Error)> RegisterAsync(ApplicationUser user, string password, string role);
        Task<ApplicationUser?> ValidateCredentialsAsync(string email, string password);
        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiry);
        Task<ApplicationUser?> GetByRefreshTokenAsync(string refreshToken);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);

        // ── 2FA ───────────────────────────────────────────────────────────
        Task<bool>   GetTwoFactorEnabledAsync(string userId);
        Task         SetTwoFactorEnabledAsync(string userId, bool enabled);
        Task<string> GenerateTwoFactorTokenAsync(string userId);
        Task<bool>   VerifyTwoFactorTokenAsync(string userId, string token);

        // ── Social Login ──────────────────────────────────────────────────
        // Finds existing user by email OR creates a new Customer account.
        // Called after the external provider's access token has been validated.
        Task<ApplicationUser> FindOrCreateSocialUserAsync(SocialUserInfo userInfo);
    }
}
