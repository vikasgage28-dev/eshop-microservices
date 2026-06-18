namespace Identity.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail, string toName, string otpCode);
    }
}
