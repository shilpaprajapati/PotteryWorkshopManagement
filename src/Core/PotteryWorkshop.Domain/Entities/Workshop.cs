using PotteryWorkshop.Domain.Common;

namespace PotteryWorkshop.Domain.Entities;

public class Workshop : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation property
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
