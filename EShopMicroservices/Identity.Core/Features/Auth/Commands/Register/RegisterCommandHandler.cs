using Identity.Core.Entities;
using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService   _tokenService;

        public RegisterCommandHandler(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService   = tokenService;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName  = request.Email,
                Email     = request.Email,
                FirstName = request.FirstName,
                LastName  = request.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var (success, error) = await _authRepository.RegisterAsync(user, request.Password, request.Role);

            if (!success)
                return new RegisterResult { Success = false, Error = error };

            var roles        = await _authRepository.GetRolesAsync(user);
            var token        = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(
                user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new RegisterResult
            {
                Success      = true,
                Token        = token,
                RefreshToken = refreshToken,
                UserId       = user.Id,
                Email        = user.Email,
                FullName     = user.FullName
            };
        }
    }
}
