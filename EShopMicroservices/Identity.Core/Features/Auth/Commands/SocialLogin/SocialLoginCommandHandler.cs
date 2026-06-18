using Identity.Core.Features.Auth.Commands.Login;
using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.SocialLogin
{
    public class SocialLoginCommandHandler : IRequestHandler<SocialLoginCommand, LoginResult>
    {
        private readonly ISocialAuthProvider _socialAuthProvider;
        private readonly IAuthRepository     _authRepository;
        private readonly ITokenService       _tokenService;

        public SocialLoginCommandHandler(
            ISocialAuthProvider socialAuthProvider,
            IAuthRepository     authRepository,
            ITokenService       tokenService)
        {
            _socialAuthProvider = socialAuthProvider;
            _authRepository     = authRepository;
            _tokenService       = tokenService;
        }

        public async Task<LoginResult> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate the access token against the provider's /userinfo endpoint
            var userInfo = await _socialAuthProvider.GetUserInfoAsync(request.AccessToken);
            if (userInfo is null)
                return new LoginResult { Success = false, Error = "Invalid or expired social token." };

            // 2. Find existing user by email OR create new Customer account
            var user = await _authRepository.FindOrCreateSocialUserAsync(userInfo);

            // 3. Issue our own JWT + refresh token (same as regular login)
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
