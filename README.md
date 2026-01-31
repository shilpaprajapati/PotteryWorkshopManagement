# Pottery Workshop Management System

A comprehensive .NET 10 Blazor web application for managing online pottery workshop bookings with an aesthetic, mobile-responsive UI built with MudBlazor.

## 🌿 Features

### User Portal
- **Beautiful Landing Page**: Hero section with workshop showcase
- **Workshop Catalog**: Browse available workshops with images and details
- **Online Booking**: Easy booking process with multiple participants support
- **Bulk Booking**: Book slots for multiple people
- **Payment Integration**: Dual payment gateway (Cashfree & Razorpay)
- **QR Code**: Get QR code for workshop check-in
- **Selfie Upload**: Upload selfie when starting workshop
- **Feedback System**: Rate and review completed workshops
- **Coupon System**: Apply discount coupons during booking

### Admin Panel
- **Dashboard**: Analytics, today's schedule, revenue tracking
- **Workshop Management**: Create and manage workshops
- **Slot Management**: Date-wise availability and capacity management
- **Booking Management**: View and manage all bookings
- **Payment Logs**: Track all payment gateway events
- **Hero Images**: Upload and manage hero section images
- **QR Code Scanner**: Verify bookings via QR code scan

### Technical Features
- **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure layers
- **CQRS Pattern**: Command Query Responsibility Segregation (ready for MediatR)
- **Factory Pattern**: Payment gateway abstraction for Cashfree and Razorpay
- **Entity Framework Core**: SQL Server with code-first migrations
- **JWT Authentication**: Secure admin authentication
- **Logging**: Comprehensive logging with Serilog
- **Notifications**: Email and SMS via Twilio
- **Scalable Design**: Foundation for future e-commerce features

## 🏗️ Architecture

```
PotteryWorkshop/
├── PotteryWorkshop.Domain/          # Domain entities, enums, value objects
├── PotteryWorkshop.Application/     # Business logic, DTOs, interfaces
├── PotteryWorkshop.Infrastructure/  # Data access, external services
├── PotteryWorkshop.API/             # REST API controllers
├── PotteryWorkshop.Web.Server/      # Blazor Server components
├── PotteryWorkshop.Web.Client/      # Blazor WebAssembly components
├── PotteryWorkshop.Domain.Tests/    # Domain unit tests
└── PotteryWorkshop.Application.Tests/ # Application unit tests
```

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB for development)
- Visual Studio 2022 or VS Code
- Node.js (optional, for additional tooling)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/shilpaprajapati/PotteryWorkshopManagement.git
   cd PotteryWorkshopManagement
   ```

2. **Update Connection String**
   Edit `PotteryWorkshop.Web.Server/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PotteryWorkshopDb;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

3. **Run Database Migrations**
   ```bash
   cd PotteryWorkshop.Infrastructure
   dotnet ef database update --startup-project ../PotteryWorkshop.Web.Server
   ```

4. **Configure External Services** (Optional)
   
   Update `appsettings.json` with your credentials:
   ```json
   "Twilio": {
     "AccountSid": "your-account-sid",
     "AuthToken": "your-auth-token",
     "PhoneNumber": "your-phone-number"
   },
   "Cashfree": {
     "AppId": "your-app-id",
     "SecretKey": "your-secret-key"
   },
   "Razorpay": {
     "KeyId": "your-key-id",
     "KeySecret": "your-key-secret"
   }
   ```

5. **Build and Run**
   ```bash
   dotnet build
   cd PotteryWorkshop.Web.Server
   dotnet run
   ```

6. **Access the Application**
   - Web App: https://localhost:5001
   - API: https://localhost:7001

## 📦 Deployment to SmarterASP.NET

### Database Deployment

1. **Generate SQL Script**
   ```bash
   cd PotteryWorkshop.Infrastructure
   dotnet ef migrations script -o ../deploy/database.sql --startup-project ../PotteryWorkshop.Web.Server
   ```

2. **Deploy Database**
   - Log in to your SmarterASP.NET control panel
   - Navigate to SQL Server Database section
   - Create a new database
   - Use the SQL Server Management Tool to run `database.sql`
   - Update connection string in production

### Application Deployment

#### Option 1: FTP Deployment (Manual)

1. **Publish the Web Application**
   ```bash
   dotnet publish PotteryWorkshop.Web.Server/PotteryWorkshop.Web.csproj -c Release -o ./publish/web
   ```

2. **Publish the API (if separate)**
   ```bash
   dotnet publish PotteryWorkshop.API/PotteryWorkshop.API.csproj -c Release -o ./publish/api
   ```

3. **Upload via FTP**
   - Use FileZilla or similar FTP client
   - Connect to your SmarterASP.NET FTP server
   - Upload contents of `./publish/web` to your web root directory
   - Upload contents of `./publish/api` to a subdirectory (e.g., `/api`)

4. **Configure SmarterASP.NET**
   - Set .NET version to .NET 10 in control panel
   - Configure application pool to "Integrated" mode
   - Set start page to index.html or default document

#### Option 2: GitHub Actions (Automated)

1. **Configure GitHub Secrets**
   Go to Repository → Settings → Secrets and add:
   - `FTP_SERVER`: Your FTP server address
   - `FTP_USERNAME`: Your FTP username
   - `FTP_PASSWORD`: Your FTP password

2. **Enable Workflow**
   Uncomment the deployment steps in `.github/workflows/build-and-deploy.yml`

3. **Push to Main Branch**
   The workflow will automatically build and deploy on every push to main

### Post-Deployment Configuration

1. **Update appsettings.json** for production:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-sql-server;Database=PotteryWorkshopDb;User Id=your-username;Password=your-password;TrustServerCertificate=true"
     },
     "Jwt": {
       "Key": "use-a-strong-production-key-here-at-least-32-characters",
       "Issuer": "PotteryWorkshop",
       "Audience": "PotteryWorkshopUsers"
     }
   }
   ```

2. **SSL Certificate**: Ensure SSL is enabled in SmarterASP.NET control panel

3. **Test the Deployment**: Visit your domain and verify all features work

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test PotteryWorkshop.Domain.Tests
dotnet test PotteryWorkshop.Application.Tests
```

## 📝 Workshop Configuration

The system supports the following workshop types:

### Signature Experiences
- **7.0 Premium** (3 hrs): ₹7,000 - Pro photos, 2 reels, 50-70 DSLR shots
- **5.0 Deluxe** (3 hrs): ₹5,000 - Pro photos, 1 reel, 40-50 DSLR shots
- **4.0 Artistic** (2 hrs): ₹4,000 - Artistic photos, 1 reel, 35-40 DSLR shots

### Regular Workshops
- **2 Hours Basic**: ₹1,200 (1 person) / ₹1,700 (2 people)
- **3 Hours Advanced**: ₹1,900 (1 person) / ₹2,200 (2 people)
- **1 Hour Kids**: ₹750 (1 person) / ₹550 per person (2 people)

## 🔐 Security

- JWT-based authentication for admin panel
- Password hashing with ASP.NET Core Identity
- SQL injection prevention via EF Core parameterized queries
- XSS protection with Blazor's built-in encoding
- HTTPS enforcement in production

## 🛠️ Technologies Used

- **.NET 10** - Latest .NET framework
- **Blazor** - Web UI framework (Server + WebAssembly)
- **MudBlazor** - Material Design component library
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **Serilog** - Logging
- **Twilio** - SMS and email notifications
- **QRCoder** - QR code generation
- **Cashfree & Razorpay** - Payment gateways

## 📧 Support

For issues and questions:
- Create an issue on GitHub
- Email: support@potteryworkshop.com

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- MudBlazor team for the amazing component library
- The .NET team for Blazor framework
- All contributors and testers

---

Made with 🌿 Mud, Love & Laughter 🌿
