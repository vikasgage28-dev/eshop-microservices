using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService   _tokenService;

        public LoginCommandHandler(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService   = tokenService;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.ValidateCredentialsAsync(request.Email, request.Password);

            if (user is null)
                return new LoginResult { Success = false, Error = "Invalid email or password." };

            var roles        = await _authRepository.GetRolesAsync(user);
            var token        = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(
                user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new LoginResult
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
