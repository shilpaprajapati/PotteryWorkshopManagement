namespace PotteryWorkshop.Application.DTOs;

public class CreateBookingDto
{
    public Guid WorkshopId { get; set; }
    public Guid SlotId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }
    public string? CouponCode { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
}

public class ParticipantDto
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
