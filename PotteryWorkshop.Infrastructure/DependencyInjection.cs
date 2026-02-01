using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Infrastructure.Data;
using PotteryWorkshop.Infrastructure.PaymentGateways;
using PotteryWorkshop.Infrastructure.Services;

namespace PotteryWorkshop.Infrastructure;

public static class DependencyInjection
{
    private const string LocalDbIdentifier = "(localdb)";
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database - Support both SQL Server and SQLite
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var useSqlite = configuration.GetValue<bool>("UseSqlite", false);
        
        // Use SQLite if explicitly configured, or if using LocalDB (for easier development)
        if (useSqlite || string.IsNullOrEmpty(connectionString) || connectionString.Contains(LocalDbIdentifier))
        {
            // Use SQLite for development/testing
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection") ?? "Data Source=potteryworkshop.db"));
        }
        else
        {
            // Use SQL Server for production
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Services
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<INotificationService, TwilioNotificationService>();
        services.AddScoped<IQrCodeService, QrCodeService>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        // Payment Gateways
        services.AddScoped<CashfreePaymentGateway>();
        services.AddScoped<RazorpayPaymentGateway>();
        services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        return services;
    }
}
