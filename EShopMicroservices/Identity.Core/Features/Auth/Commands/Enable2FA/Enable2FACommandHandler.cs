using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.Enable2FA
{
    public class Enable2FACommandHandler : IRequestHandler<Enable2FACommand, Enable2FAResult>
    {
        private readonly IAuthRepository _authRepository;

        public Enable2FACommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Enable2FAResult> Handle(Enable2FACommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return new Enable2FAResult { Success = false, Error = "User not found." };

            await _authRepository.SetTwoFactorEnabledAsync(request.UserId, request.Enabled);

            return new Enable2FAResult { Success = true };
        }
    }
}
