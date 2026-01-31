namespace PotteryWorkshop.Domain.Entities;

public class BookingParticipant : BaseEntity
{
    public Guid BookingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    
    // Navigation property
    public Booking Booking { get; set; } = null!;
}
