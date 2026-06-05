using Identity.Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Identity.Infrastructure.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public MailKitEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpEmailAsync(string toEmail, string toName, string otpCode)
        {
            var host        = _configuration["EmailSettings:SmtpHost"]     ?? "smtp.gmail.com";
            var port        = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var fromEmail   = _configuration["EmailSettings:FromEmail"]    ?? throw new InvalidOperationException("EmailSettings:FromEmail is not configured.");
            var fromName    = _configuration["EmailSettings:FromName"]     ?? "eShop";
            var appPassword = _configuration["EmailSettings:AppPassword"]  ?? throw new InvalidOperationException("EmailSettings:AppPassword is not configured.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = "Your eShop verification code";

            message.Body = new TextPart("html")
            {
                Text = $"""
                    <div style="font-family:Arial,sans-serif;max-width:480px;margin:0 auto;padding:32px;border:1px solid #e5e7eb;border-radius:8px;">
                      <h2 style="color:#0067c0;margin-bottom:8px;">eShop Verification Code</h2>
                      <p style="color:#374151;">Hi {toName},</p>
                      <p style="color:#374151;">Use the code below to complete sign-in. It expires in <strong>2 minutes</strong>.</p>
                      <div style="font-size:40px;font-weight:700;letter-spacing:8px;color:#0067c0;text-align:center;padding:24px 0;">
                        {otpCode}
                      </div>
                      <p style="color:#6b7280;font-size:13px;">If you didn't request this, you can safely ignore this email.</p>
                    </div>
                    """
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromEmail, appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(quit: true);
        }
    }
}
