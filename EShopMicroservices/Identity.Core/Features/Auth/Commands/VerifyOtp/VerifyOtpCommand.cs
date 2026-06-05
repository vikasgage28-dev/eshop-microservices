using MediatR;

namespace Identity.Core.Features.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommand : IRequest<VerifyOtpResult>
    {
        public string UserId { get; set; } = string.Empty;
        public string Code   { get; set; } = string.Empty;
    }

    public class VerifyOtpResult
    {
        public bool   Success       { get; init; }
        public string? Error        { get; init; }
        public string? Token        { get; init; }
        public string? RefreshToken { get; init; }
        public string? UserId       { get; init; }
        public string? Email        { get; init; }
        public string? FullName     { get; init; }
        public IList<string> Roles  { get; init; } = new List<string>();
    }
}
