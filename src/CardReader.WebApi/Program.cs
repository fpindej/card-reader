using CardReader.Infrastructure.Extensions;
using CardReader.Infrastructure.Persistence.Extensions;
using CardReader.WebApi.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

Log.Logger = CardReader.Logging.Extensions.LoggerConfigurationExtensions.ConfigureMinimalLogging(environmentName);

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    Log.Debug("Use Serilog");
    builder.Host.UseSerilog((context, _, loggerConfiguration) =>
    {
        CardReader.Logging.Extensions.LoggerConfigurationExtensions.SetupLogger(context.Configuration, loggerConfiguration);
    }, true);

    try
    {
        Log.Debug("Adding persistence services");
        builder.Services.AddPersistence(builder.Configuration);

        Log.Debug("ConfigureServices => Setting infrastructure layer");
        builder.Services.AddInfrastructure();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Failed to configure essential services or dependencies.");
        throw;
    }

    Log.Debug("Adding Controllers");
    builder.Services.AddControllers();

    Log.Debug("Adding Swagger for API documentation");
    builder.Services.AddSwaggerGen();

    Log.Debug("ConfigureServices => Setting AddHealthChecks");
    builder.Services.AddHealthChecks();

    Log.Debug("ConfigureServices => Setting AddEndpointsApiExplorer");
    builder.Services.AddEndpointsApiExplorer();

    Log.Debug("ConfigureServices => Setting AddSwaggerGen");
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

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

    if (app.Environment.IsDevelopment())
    {
        Log.Debug("Setting UseDeveloperExceptionPage");
        app.UseDeveloperExceptionPage();

        Log.Debug("Apply migrations to local database");
        app.ApplyMigrations();
    }

    Log.Debug("Setting cors => allow *");
    app.UseCors(b =>
    {
        b.SetIsOriginAllowed(_ => true);
        b.AllowAnyHeader();
        b.AllowAnyMethod();
    });

    Log.Debug("Setting UseSerilogRequestLogging");
    app.UseSerilogRequestLogging();

    Log.Debug("Setting UseMiddleware => ExceptionHandlingMiddleware");
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    Log.Debug("Setting UseHttpsRedirection");
    app.UseHttpsRedirection();

    Log.Debug("Setting UseRouting");
    app.UseRouting();


    Log.Debug("Setting endpoints => MapControllers");
    app.MapControllers();

    Log.Debug("Setting endpoints => MapHealthChecks");
    app.MapHealthChecks("/health");

    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down application");
    await Log.CloseAndFlushAsync();
}
