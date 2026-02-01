using PotteryWorkshop.Application.Common.Models;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> InitiatePaymentAsync(Guid bookingId, decimal amount, PaymentGateway gateway, string customerEmail, string customerPhone);
    Task<PaymentResult> VerifyPaymentAsync(string transactionId, string signature, PaymentGateway gateway);
    Task<PaymentResult> ProcessRefundAsync(Guid paymentId, decimal amount);
}
