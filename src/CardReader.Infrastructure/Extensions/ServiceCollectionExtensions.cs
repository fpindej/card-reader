using CardReader.Application;
using Microsoft.Extensions.DependencyInjection;

namespace CardReader.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRfidCardService, RfidCardService>();

        return services;
    }
}