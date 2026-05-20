using MediatR;

namespace Identity.Core.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResult>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenResult
    {
        public bool   Success       { get; init; }
        public string? Error        { get; init; }
        public string? Token        { get; init; }
        public string? RefreshToken { get; init; }
    }
}
