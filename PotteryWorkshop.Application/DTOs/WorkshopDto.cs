namespace PotteryWorkshop.Application.DTOs;

public class WorkshopDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPerson { get; set; }
    public decimal PriceForTwo { get; set; }
    public int DurationInMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public string? ImageUrl { get; set; }
    public string? InstagramReelUrl { get; set; }
    public string? Inclusions { get; set; }
    public bool IsActive { get; set; }
}
