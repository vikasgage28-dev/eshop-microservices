namespace Identity.Core.Interfaces
{
    /// <summary>
    /// User profile returned by any external OAuth/OIDC provider (Auth0, Google, GitHub…)
    /// after validating the access token against their /userinfo endpoint.
    /// </summary>
    public record SocialUserInfo(
        string  Provider,        // "Auth0", "Google", "GitHub" — stored in AspNetUserLogins.LoginProvider
        string  ProviderUserId,  // Auth0: "auth0|abc123" — unique per provider (sub claim)
        string  Email,
        string  FirstName,
        string  LastName,
        string? Picture,
        bool    EmailVerified
    );

    /// <summary>
    /// Validates an external access token and returns the user's profile.
    /// Implemented in Infrastructure (Auth0UserInfoService) — Core stays clean.
    /// </summary>
    public interface ISocialAuthProvider
    {
        Task<SocialUserInfo?> GetUserInfoAsync(string accessToken);
    }
}
