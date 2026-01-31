namespace PotteryWorkshop.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int WorkshopId { get; set; }
    public string WorkshopTitle { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public int NumberOfParticipants { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class CreateBookingDto
{
    public int WorkshopId { get; set; }
    public int CustomerId { get; set; }
    public int NumberOfParticipants { get; set; }
    public string? Notes { get; set; }
}

public class UpdateBookingDto
{
    public int Id { get; set; }
    public int NumberOfParticipants { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
