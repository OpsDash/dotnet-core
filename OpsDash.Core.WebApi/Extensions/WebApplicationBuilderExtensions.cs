using Asp.Versioning;
using OpsDash.Core.Auth;
using OpsDash.Core.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpsDash.Core.WebApi.ErrorHandlers;
using OpsDash.Core.WebApi.Options;

namespace OpsDash.Core.WebApi.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddWebApi(this WebApplicationBuilder builder, WebApiOptions options)
    {
        builder.Services.AddSingleton(options);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        if (options.ApiVersioningEnabled)
        {
            builder.Services
                .AddApiVersioning(apiOptions =>
                {
                    apiOptions.DefaultApiVersion = new(
                        majorVersion: ParseMajor(options.DefaultApiVersion),
                        minorVersion: ParseMinor(options.DefaultApiVersion));

                    apiOptions.AssumeDefaultVersionWhenUnspecified = true;
                    apiOptions.ReportApiVersions = true;
                })
                .AddMvc();
        }

        if (options.Auth.Enabled)
        {
            builder.Services.AddWebApiAuth(builder.Environment, options.Auth);
        }

        builder.Services.ConfigureOpenApi();
        
        //TODO: Use serilog and configure sinks
        builder.Logging.AddConsole();
        return builder;
    }
    
    private static int ParseMajor(string version)
        => int.TryParse(version.Split('.')[0], out var major) ? major : 1;

    private static int ParseMinor(string version)
    {
        var parts = version.Split('.');
        return parts.Length > 1 && int.TryParse(parts[1], out var minor) ? minor : 0;
    }
}