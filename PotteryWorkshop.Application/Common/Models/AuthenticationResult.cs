namespace PotteryWorkshop.Application.Common.Models;

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsAdmin { get; set; }
    public string? ErrorMessage { get; set; }
}
