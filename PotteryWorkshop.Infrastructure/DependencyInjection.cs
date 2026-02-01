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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
