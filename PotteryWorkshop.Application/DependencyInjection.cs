using Microsoft.Extensions.DependencyInjection;

namespace PotteryWorkshop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR will be added here when we add more CQRS handlers
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        return services;
    }
}
