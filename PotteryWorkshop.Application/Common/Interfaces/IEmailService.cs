namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
    Task SendBookingConfirmationAsync(string customerEmail, string bookingNumber, string workshopName, DateTime slotDate, decimal amount);
    Task SendPaymentSuccessAsync(string customerEmail, string bookingNumber, string transactionId, decimal amount);
    Task SendPaymentFailureAsync(string customerEmail, string bookingNumber, string reason);
}
