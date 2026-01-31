using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PotteryWorkshop.Domain.Interfaces;
using PotteryWorkshop.Infrastructure.Data;
using PotteryWorkshop.Infrastructure.Repositories;

namespace PotteryWorkshop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IWorkshopRepository, WorkshopRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
