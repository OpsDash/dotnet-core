using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace OpsDash.Core.Aspire;

public static class Configure
{
    public static WebApplicationBuilder ConfigureAspire(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            return builder;
        }

        builder.AddAspireServiceDefaults();

        return builder;
    }
}