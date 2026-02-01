using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Application.Common.Models;
using PotteryWorkshop.Domain.Entities;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IApplicationDbContext _context;
    private readonly IPaymentGatewayFactory _gatewayFactory;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IApplicationDbContext context,
        IPaymentGatewayFactory gatewayFactory,
        IEmailService emailService,
        IConfiguration configuration,
        ILogger<PaymentService> logger)
    {
        _context = context;
        _gatewayFactory = gatewayFactory;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<PaymentResult> InitiatePaymentAsync(Guid bookingId, decimal amount, PaymentGateway gateway, string customerEmail, string customerPhone)
    {
        try
        {
            var booking = await _context.Bookings
                .Include(b => b.Workshop)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Booking not found"
                };
            }

            // Get payment gateway
            var paymentGateway = _gatewayFactory.GetPaymentGateway(gateway);

            // Create payment order
            var orderId = booking.BookingNumber;
            var result = await paymentGateway.CreatePaymentAsync(amount, orderId, customerEmail, customerPhone);

            if (result.Success)
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    BookingId = bookingId,
                    Amount = amount,
                    Gateway = gateway,
                    TransactionId = result.TransactionId ?? string.Empty,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);

                // Add payment log
                var log = new PaymentLog
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Event = "PaymentInitiated",
                    Message = "Payment initiated",
                    Data = result.GatewayResponse,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PaymentLogs.Add(log);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Payment initiated for booking {BookingId} with transaction {TransactionId}", bookingId, result.TransactionId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment for booking {BookingId}", bookingId);
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = $"Payment initiation failed: {ex.Message}"
            };
        }
    }

    public async Task<PaymentResult> VerifyPaymentAsync(string transactionId, string signature, PaymentGateway gateway)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.Workshop)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (payment == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Payment record not found"
                };
            }

            // Verify payment with gateway
            var paymentGateway = _gatewayFactory.GetPaymentGateway(gateway);
            var result = await paymentGateway.VerifyPaymentAsync(transactionId, signature);

            if (result.Success)
            {
                // Update payment status
                payment.Status = PaymentStatus.Completed;
                payment.CompletedAt = DateTime.UtcNow;
                payment.UpdatedAt = DateTime.UtcNow;

                // Update booking status
                payment.Booking.Status = BookingStatus.Confirmed;
                payment.Booking.UpdatedAt = DateTime.UtcNow;

                // Add payment log
                var log = new PaymentLog
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Event = "PaymentVerified",
                    Message = "Payment verified successfully",
                    Data = result.GatewayResponse,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PaymentLogs.Add(log);
                await _context.SaveChangesAsync();

                // Send confirmation emails
                await _emailService.SendPaymentSuccessAsync(
                    payment.Booking.CustomerEmail,
                    payment.Booking.BookingNumber,
                    transactionId,
                    payment.Amount
                );

                // Only send booking confirmation if slot information is available
                if (payment.Booking.Slot != null)
                {
                    await _emailService.SendBookingConfirmationAsync(
                        payment.Booking.CustomerEmail,
                        payment.Booking.BookingNumber,
                        payment.Booking.Workshop.Name,
                        payment.Booking.Slot.SlotDate.Add(payment.Booking.Slot.StartTime),
                        payment.Amount
                    );
                }

                _logger.LogInformation("Payment verified successfully for transaction {TransactionId}", transactionId);
            }
            else
            {
                // Update payment status as failed
                payment.Status = PaymentStatus.Failed;
                payment.UpdatedAt = DateTime.UtcNow;

                // Add payment log
                var log = new PaymentLog
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Event = "PaymentFailed",
                    Message = "Payment verification failed",
                    Data = result.ErrorMessage ?? "Unknown error",
                    CreatedAt = DateTime.UtcNow
                };

                _context.PaymentLogs.Add(log);
                await _context.SaveChangesAsync();

                // Send failure email
                await _emailService.SendPaymentFailureAsync(
                    payment.Booking.CustomerEmail,
                    payment.Booking.BookingNumber,
                    result.ErrorMessage ?? "Payment verification failed"
                );

                _logger.LogWarning("Payment verification failed for transaction {TransactionId}", transactionId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying payment for transaction {TransactionId}", transactionId);
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = $"Payment verification failed: {ex.Message}"
            };
        }
    }

    public async Task<PaymentResult> ProcessRefundAsync(Guid paymentId, decimal amount)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Payment not found"
                };
            }

            if (payment.Status != PaymentStatus.Completed)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Only completed payments can be refunded"
                };
            }

            // Process refund with gateway
            var paymentGateway = _gatewayFactory.GetPaymentGateway(payment.Gateway);
            var result = await paymentGateway.RefundPaymentAsync(payment.TransactionId, amount);

            if (result.Success)
            {
                // Update payment status
                payment.Status = PaymentStatus.Refunded;
                payment.UpdatedAt = DateTime.UtcNow;

                // Update booking status
                payment.Booking.Status = BookingStatus.Cancelled;
                payment.Booking.UpdatedAt = DateTime.UtcNow;

                // Add payment log
                var log = new PaymentLog
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Event = "PaymentRefunded",
                    Message = $"Refund processed for amount {amount}",
                    Data = result.GatewayResponse,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PaymentLogs.Add(log);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Refund processed for payment {PaymentId}, amount {Amount}", paymentId, amount);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for payment {PaymentId}", paymentId);
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = $"Refund processing failed: {ex.Message}"
            };
        }
    }
}
