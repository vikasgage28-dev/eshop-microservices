using Azure.Communication.Email;
using EShop.Shared.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EShop.Functions.Functions;

public class WelcomeEmailFunction
{
    private readonly ILogger<WelcomeEmailFunction> _logger;
    private readonly EmailClient _emailClient;

    public WelcomeEmailFunction(ILogger<WelcomeEmailFunction> logger, EmailClient emailClient)
    {
        _logger = logger;
        _emailClient = emailClient;
    }

    [Function("WelcomeEmailFunction")]
    public async Task Run(
        [ServiceBusTrigger("welcome.email.queue", Connection = "ServiceBusConnection")] string messageBody)
    {
        _logger.LogInformation("WelcomeEmailFunction triggered. Message: {Message}", messageBody);

        var message = JsonSerializer.Deserialize<WelcomeEmailMessage>(messageBody);
        if (message == null)
        {
            _logger.LogError("Failed to deserialize message!");
            return;
        }

        var senderAddress = Environment.GetEnvironmentVariable("AcsSenderAddress");

        var emailMessage = new EmailMessage(
            senderAddress: senderAddress,
            recipients: new EmailRecipients(new List<EmailAddress>
            {
                new EmailAddress(message.Email, message.UserName)
            }),
            content: new EmailContent("Welcome to EShop!")
            {
                PlainText = $"Hello {message.UserName},\n\nWelcome to EShop! Your account has been created successfully.\n\nThank you!",
                Html = $"""
                    <h2>Welcome to EShop, {message.UserName}!</h2>
                    <p>Your account has been created successfully.</p>
                    <p>Thank you for joining us!</p>
                    """
            }
        );

        var result = await _emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage);
        _logger.LogInformation("Welcome email sent to {Email}. Status: {Status}", message.Email, result.Value.Status);
    }
}
