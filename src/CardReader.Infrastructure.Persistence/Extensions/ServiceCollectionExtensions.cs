using CardReader.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardReader.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GymDoorDbContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            opt.UseNpgsql(connectionString);
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}