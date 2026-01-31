#!/bin/bash
set -e

# Move Blazor projects to correct location
mv PotteryWorkshop.Web/PotteryWorkshop.Web ./PotteryWorkshop.Web.Server
mv PotteryWorkshop.Web/PotteryWorkshop.Web.Client ./PotteryWorkshop.Web.Client
rm -rf PotteryWorkshop.Web

# Add Blazor projects to main solution
dotnet sln add PotteryWorkshop.Web.Server/PotteryWorkshop.Web.csproj
dotnet sln add PotteryWorkshop.Web.Client/PotteryWorkshop.Web.Client.csproj

# Add references
dotnet add PotteryWorkshop.Web.Server/PotteryWorkshop.Web.csproj reference PotteryWorkshop.Application/PotteryWorkshop.Application.csproj
dotnet add PotteryWorkshop.Web.Server/PotteryWorkshop.Web.csproj reference PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj

# Create test projects
dotnet add PotteryWorkshop.Domain.Tests/PotteryWorkshop.Domain.Tests.csproj package Moq
dotnet add PotteryWorkshop.Domain.Tests/PotteryWorkshop.Domain.Tests.csproj package FluentAssertions

dotnet add PotteryWorkshop.Application.Tests/PotteryWorkshop.Application.Tests.csproj package Moq
dotnet add PotteryWorkshop.Application.Tests/PotteryWorkshop.Application.Tests.csproj package FluentAssertions

echo "Blazor structure fixed!"
