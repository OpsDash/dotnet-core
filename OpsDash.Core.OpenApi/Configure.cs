using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace OpsDash.Core.OpenApi;

public static class Configure
{
    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }
    
    public static WebApplication MapWebApiOpenApi(this WebApplication app, string title)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(opt =>
        {
            opt
                .WithTitle(title)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        
        return app;
    }
}