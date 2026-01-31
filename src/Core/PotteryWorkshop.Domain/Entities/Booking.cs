using PotteryWorkshop.Domain.Common;

namespace PotteryWorkshop.Domain.Entities;

public class Booking : BaseEntity
{
    public int WorkshopId { get; set; }
    public int CustomerId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfParticipants { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Notes { get; set; }
    
    // Navigation properties
    public Workshop Workshop { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3
}
