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
