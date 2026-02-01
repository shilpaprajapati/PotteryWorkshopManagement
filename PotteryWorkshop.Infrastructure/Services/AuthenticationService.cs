using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Application.Common.Models;
using PotteryWorkshop.Domain.Entities;

namespace PotteryWorkshop.Infrastructure.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserAuthenticationService(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName, string? phone)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "A user with this email already exists."
                };
            }

            // Create new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                IsAdmin = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"Registration failed: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null || !user.IsActive)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"Login failed: {ex.Message}"
            };
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        // Combine salt and hash
        var hashBytes = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);
        
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            
            // Extract salt (first 128 bytes for HMACSHA512)
            var salt = new byte[128];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 128);
            
            // Extract hash
            var hash = new byte[hashBytes.Length - 128];
            Buffer.BlockCopy(hashBytes, 128, hash, 0, hash.Length);
            
            // Compute hash with the extracted salt
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Compare hashes
            return computedHash.SequenceEqual(hash);
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:DurationInMinutes"] ?? "1440")),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
