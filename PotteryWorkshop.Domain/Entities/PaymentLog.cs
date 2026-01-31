namespace PotteryWorkshop.Domain.Entities;

public class PaymentLog : BaseEntity
{
    public Guid PaymentId { get; set; }
    public string Event { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Payment Payment { get; set; } = null!;
}
