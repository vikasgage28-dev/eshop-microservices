using MediatR;

namespace Identity.Core.Features.Auth.Commands.Enable2FA
{
    public class Enable2FACommand : IRequest<Enable2FAResult>
    {
        public string UserId  { get; set; } = string.Empty;
        public bool   Enabled { get; set; }
    }

    public class Enable2FAResult
    {
        public bool    Success { get; init; }
        public string? Error   { get; init; }
    }
}
