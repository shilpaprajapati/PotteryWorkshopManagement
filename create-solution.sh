#!/bin/bash
set -e

# Create solution
dotnet new sln -n PotteryWorkshop

# Create Domain Layer
dotnet new classlib -n PotteryWorkshop.Domain -f net10.0
dotnet sln add PotteryWorkshop.Domain/PotteryWorkshop.Domain.csproj

# Create Application Layer
dotnet new classlib -n PotteryWorkshop.Application -f net10.0
dotnet sln add PotteryWorkshop.Application/PotteryWorkshop.Application.csproj
dotnet add PotteryWorkshop.Application/PotteryWorkshop.Application.csproj reference PotteryWorkshop.Domain/PotteryWorkshop.Domain.csproj

# Create Infrastructure Layer
dotnet new classlib -n PotteryWorkshop.Infrastructure -f net10.0
dotnet sln add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj reference PotteryWorkshop.Application/PotteryWorkshop.Application.csproj

# Create API Layer
dotnet new webapi -n PotteryWorkshop.API -f net10.0
dotnet sln add PotteryWorkshop.API/PotteryWorkshop.API.csproj
dotnet add PotteryWorkshop.API/PotteryWorkshop.API.csproj reference PotteryWorkshop.Application/PotteryWorkshop.Application.csproj
dotnet add PotteryWorkshop.API/PotteryWorkshop.API.csproj reference PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj

# Create Blazor Web App
dotnet new blazor -n PotteryWorkshop.Web -f net10.0 -int Auto
dotnet sln add PotteryWorkshop.Web/PotteryWorkshop.Web.csproj
dotnet add PotteryWorkshop.Web/PotteryWorkshop.Web.csproj reference PotteryWorkshop.Application/PotteryWorkshop.Application.csproj
dotnet add PotteryWorkshop.Web/PotteryWorkshop.Web.csproj reference PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj

# Create Unit Test Projects
dotnet new xunit -n PotteryWorkshop.Domain.Tests -f net10.0
dotnet sln add PotteryWorkshop.Domain.Tests/PotteryWorkshop.Domain.Tests.csproj
dotnet add PotteryWorkshop.Domain.Tests/PotteryWorkshop.Domain.Tests.csproj reference PotteryWorkshop.Domain/PotteryWorkshop.Domain.csproj

dotnet new xunit -n PotteryWorkshop.Application.Tests -f net10.0
dotnet sln add PotteryWorkshop.Application.Tests/PotteryWorkshop.Application.Tests.csproj
dotnet add PotteryWorkshop.Application.Tests/PotteryWorkshop.Application.Tests.csproj reference PotteryWorkshop.Application/PotteryWorkshop.Application.csproj

echo "Solution structure created successfully!"
