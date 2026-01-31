using Microsoft.Extensions.DependencyInjection;
using PotteryWorkshop.Application.Services;

namespace PotteryWorkshop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<IWorkshopService, WorkshopService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<ICustomerService, CustomerService>();
        
        return services;
    }
}
