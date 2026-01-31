using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Domain.Entities;

public class Workshop : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPerson { get; set; }
    public decimal PriceForTwo { get; set; }
    public int DurationInMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public string? ImageUrl { get; set; }
    public string? InstagramReelUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Inclusions { get; set; }
    public WorkshopStatus Status { get; set; } = WorkshopStatus.Scheduled;
    
    // Navigation properties
    public ICollection<WorkshopSlot> Slots { get; set; } = new List<WorkshopSlot>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
