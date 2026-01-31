using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PotteryWorkshop.Infrastructure.Services;

public class TwilioNotificationService : INotificationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TwilioNotificationService> _logger;

    public TwilioNotificationService(IConfiguration configuration, ILogger<TwilioNotificationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        
        if (!string.IsNullOrEmpty(accountSid) && !string.IsNullOrEmpty(authToken))
        {
            TwilioClient.Init(accountSid, authToken);
        }
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            _logger.LogInformation("Sending email to {To}, subject: {Subject}", to, subject);
            
            // TODO: Implement email sending via Twilio SendGrid or other service
            // For now, just log the email
            _logger.LogInformation("Email content: {Body}", body);
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {To}", to);
            throw;
        }
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            _logger.LogInformation("Sending SMS to {PhoneNumber}", phoneNumber);
            
            var fromNumber = _configuration["Twilio:PhoneNumber"];
            
            if (string.IsNullOrEmpty(fromNumber))
            {
                _logger.LogWarning("Twilio phone number not configured. SMS not sent.");
                return;
            }
            
            var messageResource = await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(fromNumber),
                to: new PhoneNumber(phoneNumber)
            );

            _logger.LogInformation("SMS sent successfully. SID: {Sid}", messageResource.Sid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}
