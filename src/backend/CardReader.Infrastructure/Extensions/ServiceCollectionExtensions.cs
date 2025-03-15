using CardReader.Application.Services;
using CardReader.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CardReader.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IAccessLogService, AccessLogService>();

        return services;
    }
}