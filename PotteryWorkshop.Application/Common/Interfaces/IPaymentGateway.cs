using PotteryWorkshop.Application.Common.Models;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IPaymentGateway
{
    Task<PaymentResult> CreatePaymentAsync(decimal amount, string orderId, string customerEmail, string customerPhone);
    Task<PaymentResult> VerifyPaymentAsync(string transactionId, string signature);
    Task<PaymentResult> RefundPaymentAsync(string transactionId, decimal amount);
}
