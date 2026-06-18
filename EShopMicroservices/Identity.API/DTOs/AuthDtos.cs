namespace Identity.API.DTOs
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public string Email     { get; set; } = string.Empty;
        public string Password  { get; set; } = string.Empty;
        public string Role      { get; set; } = "Customer";
    }

    public class LoginRequest
    {
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string  Token        { get; init; } = string.Empty;
        public string  RefreshToken { get; init; } = string.Empty;
        public string? UserId       { get; init; }
        public string? Email        { get; init; }
        public string? FullName     { get; init; }
        public IList<string> Roles  { get; init; } = new List<string>();
        // 2FA — when true, Token/RefreshToken are empty; client must call /send-otp then /verify-otp
        public bool    Requires2FA  { get; init; }
    }

    public class SendOtpRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class VerifyOtpRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Code   { get; set; } = string.Empty;
    }

    public class Toggle2FARequest
    {
        public bool Enabled { get; set; }
    }

    public class SocialLoginRequest
    {
        /// <summary>"auth0" | "google" | "github"</summary>
        public string Provider    { get; set; } = "auth0";

        /// <summary>Access token obtained by the SPA from the OAuth provider.</summary>
        public string AccessToken { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public string  UserId    { get; init; } = string.Empty;
        public string  Email     { get; init; } = string.Empty;
        public string  FullName  { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
