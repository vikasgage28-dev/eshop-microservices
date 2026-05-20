using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService   _tokenService;

        public RefreshTokenCommandHandler(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService   = tokenService;
        }

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByRefreshTokenAsync(request.RefreshToken);

            if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return new RefreshTokenResult { Success = false, Error = "Invalid or expired refresh token." };

            var roles        = await _authRepository.GetRolesAsync(user);
            var newToken     = _tokenService.GenerateAccessToken(user, roles);
            var newRefresh   = _tokenService.GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(
                user.Id, newRefresh, DateTime.UtcNow.AddDays(7));

            return new RefreshTokenResult
            {
                Success      = true,
                Token        = newToken,
                RefreshToken = newRefresh
            };
        }
    }
}
