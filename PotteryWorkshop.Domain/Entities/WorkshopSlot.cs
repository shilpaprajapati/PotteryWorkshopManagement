namespace PotteryWorkshop.Domain.Entities;

public class WorkshopSlot : BaseEntity
{
    public Guid WorkshopId { get; set; }
    public DateTime SlotDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int AvailableCapacity { get; set; }
    public int TotalCapacity { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    // Navigation properties
    public Workshop Workshop { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
