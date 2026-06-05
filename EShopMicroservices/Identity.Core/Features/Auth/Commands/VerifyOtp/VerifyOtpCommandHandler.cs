using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService   _tokenService;

        public VerifyOtpCommandHandler(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService   = tokenService;
        }

        public async Task<VerifyOtpResult> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // Verify the TOTP code mathematically (no DB lookup for the code itself)
            var valid = await _authRepository.VerifyTwoFactorTokenAsync(request.UserId, request.Code);

            if (!valid)
                return new VerifyOtpResult { Success = false, Error = "Invalid or expired code." };

            var user = await _authRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return new VerifyOtpResult { Success = false, Error = "User not found." };

            // Code is valid — issue full JWT now
            var roles        = await _authRepository.GetRolesAsync(user);
            var token        = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(
                user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new VerifyOtpResult
            {
                Success      = true,
                Token        = token,
                RefreshToken = refreshToken,
                UserId       = user.Id,
                Email        = user.Email,
                FullName     = user.FullName,
                Roles        = roles
            };
        }
    }
}
