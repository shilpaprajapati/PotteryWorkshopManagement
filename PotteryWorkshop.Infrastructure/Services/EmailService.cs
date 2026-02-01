using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;

namespace PotteryWorkshop.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["Email:Username"];
            var smtpPassword = _configuration["Email:Password"];
            var fromEmail = _configuration["Email:FromEmail"] ?? smtpUsername ?? "noreply@potteryworkshop.com";
            var fromName = _configuration["Email:FromName"] ?? "Pottery Workshop";

            // If SMTP not configured, log and return (for development)
            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername))
            {
                _logger.LogWarning("Email SMTP not configured. Email would be sent to: {To}, Subject: {Subject}", to, subject);
                _logger.LogInformation("Email body: {Body}", htmlBody);
                return;
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            // Don't throw - email failures shouldn't break the application
        }
    }

    public async Task SendBookingConfirmationAsync(string customerEmail, string bookingNumber, string workshopName, DateTime slotDate, decimal amount)
    {
        var subject = $"Booking Confirmation - {bookingNumber}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px;'>
                    <h2 style='color: #8B7355;'>Booking Confirmation</h2>
                    <p>Thank you for booking with Pottery Workshop!</p>
                    
                    <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Booking Number:</strong> {bookingNumber}</p>
                        <p><strong>Workshop:</strong> {workshopName}</p>
                        <p><strong>Date & Time:</strong> {slotDate:dddd, MMMM dd, yyyy 'at' hh:mm tt}</p>
                        <p><strong>Amount:</strong> ₹{amount:N2}</p>
                    </div>
                    
                    <p>We look forward to seeing you!</p>
                    <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                        If you have any questions, please contact us at support@potteryworkshop.com
                    </p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(customerEmail, subject, body);
    }

    public async Task SendPaymentSuccessAsync(string customerEmail, string bookingNumber, string transactionId, decimal amount)
    {
        var subject = $"Payment Successful - {bookingNumber}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px;'>
                    <h2 style='color: #26b050;'>Payment Successful!</h2>
                    <p>Your payment has been processed successfully.</p>
                    
                    <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Booking Number:</strong> {bookingNumber}</p>
                        <p><strong>Transaction ID:</strong> {transactionId}</p>
                        <p><strong>Amount Paid:</strong> ₹{amount:N2}</p>
                        <p><strong>Payment Date:</strong> {DateTime.UtcNow:dddd, MMMM dd, yyyy 'at' hh:mm tt}</p>
                    </div>
                    
                    <p>Your booking is now confirmed. We'll send you a reminder before your workshop session.</p>
                    <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                        If you have any questions, please contact us at support@potteryworkshop.com
                    </p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(customerEmail, subject, body);
    }

    public async Task SendPaymentFailureAsync(string customerEmail, string bookingNumber, string reason)
    {
        var subject = $"Payment Failed - {bookingNumber}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px;'>
                    <h2 style='color: #e50000;'>Payment Failed</h2>
                    <p>Unfortunately, we were unable to process your payment.</p>
                    
                    <div style='background-color: #fff5f5; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #e50000;'>
                        <p><strong>Booking Number:</strong> {bookingNumber}</p>
                        <p><strong>Reason:</strong> {reason}</p>
                    </div>
                    
                    <p>Please try again or contact us for assistance. Your booking will remain pending for 24 hours.</p>
                    <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                        If you have any questions, please contact us at support@potteryworkshop.com
                    </p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(customerEmail, subject, body);
    }
}
