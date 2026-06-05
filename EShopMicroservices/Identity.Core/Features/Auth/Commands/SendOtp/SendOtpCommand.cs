using MediatR;

namespace Identity.Core.Features.Auth.Commands.SendOtp
{
    public class SendOtpCommand : IRequest<SendOtpResult>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class SendOtpResult
    {
        public bool    Success { get; init; }
        public string? Error   { get; init; }
    }
}
