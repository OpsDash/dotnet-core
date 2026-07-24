using OpsDash.Core.WebApi.Extensions;
using OpsDash.Core.WebApi.Helpers;
using Microsoft.AspNetCore.Builder;
using OpsDash.Core.WebApi.Options;

namespace OpsDash.Core.WebApi;

public static class ServiceHost
{
    public static async Task Run<TConfiguration>(string[] args)
        where TConfiguration : WebApiApplicationConfiguration, new()
    {
        var appConfiguration = new TConfiguration();
        
        var builder = WebApplication.CreateBuilder(args);
        appConfiguration.Initialize(builder.Configuration);

        var options = new WebApiOptions();
        options.Bind(builder.Configuration);
        
        appConfiguration.ConfigureWebApi(options);
        
        builder.AddWebApi(options);
        
        appConfiguration.ConfigureHost(builder);
        appConfiguration.ConfigureServices(builder.Services);
        var app = builder.Build();

        app.UseWebApi(options);
        appConfiguration.Configure(app);

        await app.RunStartupTasks();
        await app.RunAsync();
    }
}