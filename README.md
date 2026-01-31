# Pottery Workshop Management System

A comprehensive workshop management system built with Clean Architecture, Blazor, and MudBlazor.

## Architecture

This project follows Clean Architecture principles with the following layers:

### 1. Domain Layer (`PotteryWorkshop.Domain`)
- **Entities**: Core business entities (Workshop, Customer, Booking)
- **Interfaces**: Repository interfaces and domain contracts
- **Value Objects**: Enumerations and domain-specific types
- No external dependencies

### 2. Application Layer (`PotteryWorkshop.Application`)
- **DTOs**: Data Transfer Objects for communication between layers
- **Services**: Business logic and use cases
- **Mappings**: AutoMapper profiles for entity-to-DTO conversions
- **Validators**: FluentValidation rules (planned)
- Dependencies: Domain layer only

### 3. Infrastructure Layer (`PotteryWorkshop.Infrastructure`)
- **Data**: EF Core DbContext and migrations
- **Repositories**: Concrete implementations of repository interfaces
- **External Services**: Third-party integrations (if any)
- Dependencies: Domain and Application layers

### 4. Presentation Layer (`PotteryWorkshop.Web`)
- **Blazor Server App**: Interactive UI components with MudBlazor
- **Pages**: Workshop, Booking, and Customer management pages
- **Components**: Reusable UI components
- Dependencies: Application and Infrastructure layers

## Technology Stack

- **.NET 8.0**: Latest LTS version
- **Blazor Server**: Interactive web UI framework
- **MudBlazor**: Material Design component library
- **Entity Framework Core**: ORM for data access
- **SQL Server**: Database (LocalDB for development)
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Validation rules
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for tests
- **FluentAssertions**: Fluent test assertions

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server

### Installation

1. Clone the repository:
```bash
git clone https://github.com/shilpaprajapati/PotteryWorkshopManagement.git
cd PotteryWorkshopManagement
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update the database connection string in `src/Presentation/PotteryWorkshop.Web/appsettings.json` if needed.

4. Apply database migrations:
```bash
cd src/Infrastructure/PotteryWorkshop.Infrastructure
dotnet ef database update --startup-project ../../Presentation/PotteryWorkshop.Web
```

5. Run the application:
```bash
cd ../../Presentation/PotteryWorkshop.Web
dotnet run
```

6. Open your browser and navigate to `https://localhost:5001` (or the URL shown in the console).

## Running Tests

Run all tests:
```bash
dotnet test
```

Run tests with coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Project Structure

```
PotteryWorkshopManagement/
├── src/
│   ├── Core/
│   │   ├── PotteryWorkshop.Domain/          # Domain entities and interfaces
│   │   └── PotteryWorkshop.Application/     # Business logic and DTOs
│   ├── Infrastructure/
│   │   └── PotteryWorkshop.Infrastructure/  # Data access and repositories
│   └── Presentation/
│       └── PotteryWorkshop.Web/             # Blazor web application
├── tests/
│   ├── PotteryWorkshop.Domain.Tests/        # Domain layer tests
│   ├── PotteryWorkshop.Application.Tests/   # Application layer tests
│   └── PotteryWorkshop.Infrastructure.Tests/# Infrastructure layer tests
└── .github/
    └── workflows/
        └── dotnet.yml                       # CI/CD pipeline
```

## Features

### Workshop Management
- Create, edit, and delete workshops
- Set instructor, schedule, location, and pricing
- Manage participant capacity
- Activate/deactivate workshops

### Booking Management
- Create and manage bookings
- Track booking status (Pending, Confirmed, Cancelled, Completed)
- Calculate total amounts based on participants
- View bookings by workshop or customer

### Customer Management
- Add and manage customer information
- Store contact details (email, phone)
- Search and filter customers
- Track customer bookings

## CI/CD Pipeline

The project includes a GitHub Actions workflow that:
- Builds the solution
- Runs all tests
- Collects code coverage
- Uploads coverage reports to Codecov

The pipeline runs on:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches

## Database Migrations

To add a new migration:
```bash
cd src/Infrastructure/PotteryWorkshop.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../../Presentation/PotteryWorkshop.Web
```

To update the database:
```bash
dotnet ef database update --startup-project ../../Presentation/PotteryWorkshop.Web
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Contact

For questions or feedback, please open an issue on GitHub.
