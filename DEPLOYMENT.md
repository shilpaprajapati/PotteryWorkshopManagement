# Deployment Guide for SmarterASP.NET

This guide provides step-by-step instructions for deploying the Pottery Workshop Management System to SmarterASP.NET hosting.

## Prerequisites

- SmarterASP.NET hosting account
- SQL Server database on SmarterASP.NET
- FTP credentials from SmarterASP.NET
- Visual Studio or .NET CLI

## Step 1: Prepare the Database

### 1.1 Generate Migration Script

```bash
cd PotteryWorkshop.Infrastructure
dotnet ef migrations script -o ../deploy/database.sql --startup-project ../PotteryWorkshop.Web.Server
```

This creates a SQL script with all schema changes.

### 1.2 Create Database on SmarterASP.NET

1. Log in to your SmarterASP.NET Control Panel
2. Navigate to **SQL Server Database** section
3. Click **Create Database**
4. Note down the connection string details:
   - Server name
   - Database name
   - Username
   - Password

### 1.3 Execute Migration Script

1. In Control Panel, go to **SQL Server Management**
2. Select your database
3. Click **Open SQL Query Window**
4. Copy contents of `deploy/database.sql`
5. Execute the script
6. Verify tables are created

## Step 2: Configure Application Settings

### 2.1 Update Connection String

Edit `appsettings.json` in the publish folder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.sql.somee.com;Database=YourDbName;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "CHANGE-THIS-TO-A-SECURE-KEY-AT-LEAST-32-CHARACTERS-LONG",
    "Issuer": "PotteryWorkshop",
    "Audience": "PotteryWorkshopUsers",
    "DurationInMinutes": 1440
  },
  "Twilio": {
    "AccountSid": "your-twilio-account-sid",
    "AuthToken": "your-twilio-auth-token",
    "PhoneNumber": "+1234567890"
  },
  "Cashfree": {
    "AppId": "your-cashfree-app-id",
    "SecretKey": "your-cashfree-secret-key",
    "BaseUrl": "https://api.cashfree.com"
  },
  "Razorpay": {
    "KeyId": "your-razorpay-key-id",
    "KeySecret": "your-razorpay-key-secret",
    "BaseUrl": "https://api.razorpay.com/v1"
  }
}
```

**Important Security Notes:**
- Generate a strong random key for JWT
- Never commit credentials to source control
- Use environment variables in production

## Step 3: Publish the Application

### 3.1 Publish Web Application

```bash
cd PotteryWorkshop.Web.Server
dotnet publish -c Release -o ../publish/web
```

### 3.2 Verify Published Files

Check that `publish/web` contains:
- `PotteryWorkshop.Web.dll`
- `web.config`
- `wwwroot` folder
- All dependencies

## Step 4: Deploy via FTP

### 4.1 Connect to FTP

Use an FTP client like FileZilla:

- **Host**: ftp.yourdomain.com (or IP provided by SmarterASP.NET)
- **Username**: Your FTP username
- **Password**: Your FTP password
- **Port**: 21 (or as specified)

### 4.2 Upload Files

1. Navigate to your web root directory (usually `/wwwroot` or `/public_html`)
2. Upload all contents from `publish/web` folder
3. Maintain the folder structure
4. Upload can take 10-30 minutes depending on connection

### 4.3 Set Permissions

Ensure these folders have write permissions:
- `/wwwroot/uploads` (for file uploads)
- `/wwwroot/logs` (for Serilog file logs)

## Step 5: Configure IIS Settings on SmarterASP.NET

### 5.1 Set .NET Version

1. In Control Panel, go to **Website Settings**
2. Select **.NET Version**: .NET 10 (or latest available)
3. Set **Application Pool**: Integrated

### 5.2 Configure web.config

Ensure `web.config` includes:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\PotteryWorkshop.Web.dll" 
                  stdoutLogEnabled="false" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

### 5.3 Enable HTTPS

1. In Control Panel, go to **SSL Settings**
2. Enable **Force HTTPS**
3. Install SSL certificate (free Let's Encrypt available)

## Step 6: Initialize Database with Seed Data

### 6.1 First Run

The application will automatically:
1. Run EF Core migrations on first startup
2. Seed initial workshop data
3. Create sample admin user
4. Add hero images

### 6.2 Verify Seeding

Check database tables contain:
- 6 workshops
- 3 hero images
- 2 sample coupons
- 1 admin user

## Step 7: Test the Deployment

### 7.1 Basic Tests

1. Visit your domain: https://yourdomain.com
2. Verify home page loads with hero section
3. Check workshops page: https://yourdomain.com/workshops
4. Test navigation menu

### 7.2 Admin Access

1. Navigate to: https://yourdomain.com/admin/dashboard
2. Login with:
   - Email: admin@potteryworkshop.com
   - Password: Admin@123 (Change immediately!)

### 7.3 Booking Flow

1. Select a workshop
2. Fill booking form
3. Verify validation works
4. Check database for booking record

## Step 8: Post-Deployment Configuration

### 8.1 Change Admin Password

1. Log in as admin
2. Navigate to profile settings
3. Change default password immediately

### 8.2 Configure Payment Gateways

#### Cashfree Setup
1. Create account at https://www.cashfree.com
2. Get API credentials from dashboard
3. Update `appsettings.json` with credentials
4. Test in sandbox mode first

#### Razorpay Setup
1. Create account at https://razorpay.com
2. Get API Key and Secret from dashboard
3. Update `appsettings.json` with credentials
4. Configure webhooks for payment callbacks

### 8.3 Configure Twilio

1. Create account at https://www.twilio.com
2. Get Account SID and Auth Token
3. Purchase a phone number for SMS
4. Configure SendGrid for emails (if needed)
5. Update `appsettings.json`

### 8.4 Upload Hero Images

1. Log in as admin
2. Navigate to Hero Images management
3. Upload high-quality images (recommended: 1600x800px)
4. Set display order and activate

## Step 9: Monitoring and Maintenance

### 9.1 Check Logs

Logs are stored in `/wwwroot/logs` folder:
- Download and review regularly
- Check for errors or warnings
- Monitor performance issues

### 9.2 Database Backups

SmarterASP.NET typically provides:
- Automatic daily backups
- Manual backup option in Control Panel
- Download backups regularly

### 9.3 Update Process

To deploy updates:

```bash
# 1. Pull latest code
git pull origin main

# 2. Build and publish
dotnet publish PotteryWorkshop.Web.Server -c Release -o ./publish/web

# 3. Upload via FTP (overwrite existing files)
# 4. If schema changed, run migration scripts

# 5. Restart application pool (in Control Panel)
```

## Troubleshooting

### Issue: 500 Internal Server Error

**Solution:**
1. Check `web.config` is present
2. Verify .NET version is set correctly
3. Check connection string is valid
4. Review logs in `/logs/` folder
5. Enable detailed errors in `web.config` temporarily

### Issue: Database Connection Failed

**Solution:**
1. Verify connection string in `appsettings.json`
2. Check firewall rules allow connection
3. Test connection from SQL Management Tool
4. Ensure `TrustServerCertificate=true` is set

### Issue: Assets Not Loading

**Solution:**
1. Verify `wwwroot` folder uploaded completely
2. Check file permissions
3. Clear browser cache
4. Check IIS MIME types configured

### Issue: Application Doesn't Start

**Solution:**
1. Check Application Pool is running
2. Verify `PotteryWorkshop.Web.dll` exists
3. Check all dependencies are uploaded
4. Review startup logs

## Performance Optimization

### 1. Enable Response Compression

Add to `Program.cs`:

```csharp
builder.Services.AddResponseCompression();
// ...
app.UseResponseCompression();
```

### 2. Enable Output Caching

```csharp
builder.Services.AddOutputCache();
// ...
app.UseOutputCache();
```

### 3. Optimize Images

- Use WebP format where possible
- Compress images before upload
- Use CDN for static assets

### 4. Database Optimization

- Add indexes on frequently queried columns
- Use pagination for large datasets
- Implement caching for workshop data

## Security Checklist

- [ ] Changed default admin password
- [ ] Generated strong JWT secret key
- [ ] Enabled HTTPS/SSL
- [ ] Configured CORS properly
- [ ] Set up rate limiting
- [ ] Enabled request validation
- [ ] Configured secure headers
- [ ] Regular security updates
- [ ] Database credentials secured
- [ ] API keys in secure storage

## Support

For issues specific to:
- **SmarterASP.NET Hosting**: Contact their support
- **Application Issues**: Create issue on GitHub
- **Payment Gateways**: Contact respective support teams

## Additional Resources

- [SmarterASP.NET Knowledge Base](https://www.smarterasp.net/support/kb)
- [.NET Deployment Documentation](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/)
- [MudBlazor Documentation](https://mudblazor.com/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

---

**Last Updated**: January 2026
**Application Version**: 1.0.0
