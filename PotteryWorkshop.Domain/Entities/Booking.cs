using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Domain.Entities;

public class Booking : BaseEntity
{
    public string BookingNumber { get; set; } = string.Empty;
    public Guid WorkshopId { get; set; }
    public Guid SlotId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string? CouponCode { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? QrCode { get; set; }
    public DateTime? CheckInTime { get; set; }
    public string? SelfieUrl { get; set; }
    public string? FeedbackText { get; set; }
    public int? FeedbackRating { get; set; }
    
    // Navigation properties
    public Workshop Workshop { get; set; } = null!;
    public WorkshopSlot Slot { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<BookingParticipant> Participants { get; set; } = new List<BookingParticipant>();
}
