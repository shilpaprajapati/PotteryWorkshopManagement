using PotteryWorkshop.Application.Common.Models;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IUserAuthenticationService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName, string? phone);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<bool> ValidateTokenAsync(string token);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

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
