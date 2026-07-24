using OpsDash.Core.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OpsDash.Core.WebApi.Options;

namespace OpsDash.Core.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApi(this WebApplication app, WebApiOptions options)
    { 
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();

        if (options.Auth.Enabled)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.MapWebApiOpenApi(options.ServiceName);
        
        app.MapControllers();

        return app;
    }
}