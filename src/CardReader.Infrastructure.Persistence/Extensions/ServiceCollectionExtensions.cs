using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GymDoorDbContext>((sp, opt) =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            opt.UseNpgsql(connectionString);
            
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<GymDoorDbContext>();
            logger.LogInformation("Opening database connection: {connectionString}", connectionString);
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();
        services.AddScoped<IAccessLogService, AccessLogService>();

        return services;
    }
}
