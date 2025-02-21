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

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        {

        }
    }
}
