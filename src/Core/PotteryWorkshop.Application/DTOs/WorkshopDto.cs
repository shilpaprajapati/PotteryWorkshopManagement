namespace PotteryWorkshop.Application.DTOs;

public class WorkshopDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public int AvailableSlots { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateWorkshopDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class UpdateWorkshopDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
