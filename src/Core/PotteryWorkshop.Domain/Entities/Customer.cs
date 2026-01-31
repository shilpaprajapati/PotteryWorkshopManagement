using PotteryWorkshop.Domain.Common;

namespace PotteryWorkshop.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
