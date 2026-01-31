namespace PotteryWorkshop.Domain.Entities;

public class HeroImage : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
