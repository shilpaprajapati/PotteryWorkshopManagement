using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid BookingId { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public PaymentGateway Gateway { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? GatewayResponse { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Navigation property
    public Booking Booking { get; set; } = null!;
    public ICollection<PaymentLog> Logs { get; set; } = new List<PaymentLog>();
}
