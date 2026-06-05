using MediatR;

namespace Identity.Core.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResult
    {
        public bool   Success        { get; init; }
        public string? Error         { get; init; }
        public string? Token         { get; init; }
        public string? RefreshToken  { get; init; }
        public string? UserId        { get; init; }
        public string? Email         { get; init; }
        public string? FullName      { get; init; }
        public IList<string> Roles   { get; init; } = new List<string>();

        // ── 2FA ───────────────────────────────────────────────────────────
        // When true: no token returned yet — frontend must redirect to /verify-otp
        public bool Requires2FA { get; init; }
    }
}
