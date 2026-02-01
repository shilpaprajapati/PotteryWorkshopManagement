# Authentication Setup Guide

This document describes the authentication system implemented in the Pottery Workshop Management System.

## Overview

The system uses JWT (JSON Web Token) based authentication for both the API and Blazor Web Server. User data is stored in a database with secure password hashing.

## Architecture

### Components

1. **Domain Layer** (`PotteryWorkshop.Domain`)
   - `User` entity with properties: Email, PasswordHash, FirstName, LastName, Phone, IsAdmin, IsActive

2. **Application Layer** (`PotteryWorkshop.Application`)
   - `IUserAuthenticationService` interface
   - `AuthenticationResult` model for login/register responses

3. **Infrastructure Layer** (`PotteryWorkshop.Infrastructure`)
   - `UserAuthenticationService` implementation
   - Password hashing using HMACSHA512
   - JWT token generation and validation
   - Database context with Users DbSet

4. **API Layer** (`PotteryWorkshop.API`)
   - `AuthController` with authentication endpoints
   - JWT Bearer authentication middleware
   - CORS support

5. **Web Server Layer** (`PotteryWorkshop.Web.Server`)
   - Login.razor page
   - Register.razor page
   - Cookie-based authentication for Blazor

## Database Setup

### SQL Server (Production)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=PotteryWorkshopDb;User Id=your-username;Password=your-password;TrustServerCertificate=true"
  },
  "UseSqlite": false
}
```

### SQLite (Development/Testing)

```json
{
  "UseSqlite": true,
  "ConnectionStrings": {
    "SqliteConnection": "Data Source=potteryworkshop.db"
  }
}
```

### Migrations

Run migrations from the Infrastructure project:

```bash
# For API project
dotnet ef database update --startup-project ../PotteryWorkshop.API

# For Web Server project
dotnet ef database update --startup-project ../PotteryWorkshop.Web.Server
```

### Default Admin User

The database seeding creates a default admin user:
- **Email**: admin@potteryworkshop.com
- **Password**: Admin@123
- **Role**: Admin

⚠️ **Important**: Change the default admin password in production!

## API Endpoints

### POST /api/auth/register

Register a new user account.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890"
}
```

**Response (Success):**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "guid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isAdmin": false,
  "errorMessage": null
}
```

### POST /api/auth/login

Login with email and password.

**Request:**
```json
{
  "email": "admin@potteryworkshop.com",
  "password": "Admin@123"
}
```

**Response (Success):**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "guid",
  "email": "admin@potteryworkshop.com",
  "firstName": "Admin",
  "lastName": "User",
  "isAdmin": true,
  "errorMessage": null
}
```

**Response (Failure):**
```json
{
  "success": false,
  "token": null,
  "userId": null,
  "email": null,
  "firstName": null,
  "lastName": null,
  "isAdmin": false,
  "errorMessage": "Invalid email or password."
}
```

### POST /api/auth/validate

Validate a JWT token.

**Request:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response:**
```json
{
  "isValid": true
}
```

### GET /api/auth/me

Get current authenticated user information (requires Bearer token).

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
  "userId": "guid",
  "email": "user@example.com",
  "name": "John Doe",
  "isAdmin": false
}
```

## JWT Configuration

Configure JWT settings in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyForJwtTokenGenerationMustBeAtLeast32Characters",
    "Issuer": "PotteryWorkshop",
    "Audience": "PotteryWorkshopUsers",
    "DurationInMinutes": 1440
  }
}
```

⚠️ **Security Notes**:
- Use a strong, random key (at least 32 characters)
- Keep the key secret and different in each environment
- Use environment variables for production

## Blazor Web Pages

### Login Page

URL: `/login`

Features:
- Email and password input
- Password visibility toggle
- Remember me option
- Error message display
- Redirect to admin dashboard for admin users
- Redirect to home page for regular users

### Register Page

URL: `/register`

Features:
- First name, last name, email, phone, password inputs
- Password confirmation
- Password visibility toggles
- Terms and conditions agreement
- Success message with redirect to login

## Testing

### API Testing with curl

**Login:**
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@potteryworkshop.com","password":"Admin@123"}'
```

**Register:**
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email":"newuser@example.com",
    "password":"Password123",
    "firstName":"New",
    "lastName":"User",
    "phone":"+1234567890"
  }'
```

**Protected Endpoint:**
```bash
TOKEN="your-jwt-token-here"
curl -X GET http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer $TOKEN"
```

## Security Features

1. **Password Hashing**: Uses HMACSHA512 with salt
2. **JWT Tokens**: Secure token-based authentication
3. **Token Expiration**: Configurable token lifetime
4. **HTTPS Enforcement**: Recommended for production
5. **CORS**: Configured for cross-origin requests
6. **Role-Based Access**: Admin and User roles

## Troubleshooting

### Database Connection Issues

If using SQL Server and getting connection errors:
1. Verify SQL Server is running
2. Check connection string in appsettings.json
3. Run migrations to create database schema
4. For development, consider switching to SQLite

### JWT Token Invalid

1. Check if token has expired (default: 24 hours)
2. Verify JWT Key matches between services
3. Ensure Issuer and Audience are configured correctly

### Login Fails with Correct Credentials

1. Verify database has seeded data
2. Check password hashing is consistent
3. Ensure user IsActive = true in database

## Production Deployment

### Checklist

- [ ] Change default admin password
- [ ] Use strong, random JWT key
- [ ] Store secrets in environment variables or Key Vault
- [ ] Use SQL Server (not SQLite)
- [ ] Enable HTTPS
- [ ] Configure proper CORS origins
- [ ] Set up proper logging
- [ ] Configure email/SMS notifications

### Environment Variables

```bash
export ConnectionStrings__DefaultConnection="Server=..."
export Jwt__Key="your-production-key"
export Jwt__DurationInMinutes="60"
```

## References

- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [JWT.io](https://jwt.io/) - JWT debugger
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
