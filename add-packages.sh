#!/bin/bash
set -e

# Add Application Layer packages
dotnet add PotteryWorkshop.Application/PotteryWorkshop.Application.csproj package MediatR
dotnet add PotteryWorkshop.Application/PotteryWorkshop.Application.csproj package FluentValidation
dotnet add PotteryWorkshop.Application/PotteryWorkshop.Application.csproj package AutoMapper

# Add Infrastructure Layer packages
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Twilio
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package QRCoder
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Serilog.AspNetCore
dotnet add PotteryWorkshop.Infrastructure/PotteryWorkshop.Infrastructure.csproj package Serilog.Sinks.File

# Add API Layer packages
dotnet add PotteryWorkshop.API/PotteryWorkshop.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add PotteryWorkshop.API/PotteryWorkshop.API.csproj package Swashbuckle.AspNetCore

# Add Blazor Web packages
dotnet add PotteryWorkshop.Web.Server/PotteryWorkshop.Web.csproj package MudBlazor
dotnet add PotteryWorkshop.Web.Client/PotteryWorkshop.Web.Client.csproj package MudBlazor

echo "Packages added successfully!"
