using CardReader.Infrastructure.Extensions;
using CardReader.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace CardReader.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Log.Debug("ConfigureServices => Setting AddControllers");
        services.AddControllers();

        Log.Debug("ConfigureServices => Setting AddHealthChecks");
        services.AddHealthChecks();

        Log.Debug("ConfigureServices => Setting AddEndpointsApiExplorer");
        services.AddEndpointsApiExplorer();

        Log.Debug("ConfigureServices => Setting AddSwaggerGen");
        services.AddSwaggerGen();
        
        Log.Debug("ConfigureServices => Setting infrastructure layer");
        services.AddInfrastructure();

        Log.Debug("ConfigureServices => Setting database persistence layer");
        services.AddPersistence(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            Log.Debug("Setting UseSwagger");
            app.UseSwagger();

            Log.Debug("Setting UseSwaggerUI");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardReader API V1");
                c.DocumentTitle = "Card Reader API Documentation";
                c.RoutePrefix = "swagger";
            });

            Log.Debug("Setting UseRedoc");
            app.UseReDoc(c =>
            {
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "Card Reader API Documentation";
                c.RoutePrefix = "redoc";
            });
            
            if (env.IsDevelopment())
            {
                Log.Debug("Setting UseDeveloperExceptionPage");
                app.UseDeveloperExceptionPage();
            }

            Log.Debug("Setting cors => allow *");
            app.UseCors(builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            Log.Debug("Setting UseSerilogRequestLogging");
            app.UseSerilogRequestLogging();

            Log.Debug("Setting UseHttpsRedirection");
            app.UseHttpsRedirection();

            Log.Debug("Setting UseRouting");
            app.UseRouting();

            Log.Debug("Setting UseEndpoints");
            app.UseEndpoints(endpoints =>
            {
                Log.Debug("Setting endpoints => MapControllers");
                endpoints.MapControllers();

                Log.Debug("Setting endpoints => MapHealthChecks");
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}