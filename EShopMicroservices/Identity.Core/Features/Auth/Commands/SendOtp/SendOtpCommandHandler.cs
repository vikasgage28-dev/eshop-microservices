using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.SendOtp
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, SendOtpResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmailService   _emailService;

        public SendOtpCommandHandler(IAuthRepository authRepository, IEmailService emailService)
        {
            _authRepository = authRepository;
            _emailService   = emailService;
        }

        public async Task<SendOtpResult> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return new SendOtpResult { Success = false, Error = "User not found." };

            // Generate TOTP — no DB write, math-based using SecurityStamp + time
            var otpCode = await _authRepository.GenerateTwoFactorTokenAsync(request.UserId);

            // Send via email
            await _emailService.SendOtpEmailAsync(
                toEmail: user.Email ?? string.Empty,
                toName:  user.FullName,
                otpCode: otpCode);

            return new SendOtpResult { Success = true };
        }
    }
}
