using Microsoft.Extensions.Configuration;
using OpsDash.Core.WebApi.Options;

namespace OpsDash.Core.WebApi.Helpers;

public static class WebApiHelper
{
    public static WebApiOptions Bind(
        this WebApiOptions options,
        IConfiguration configuration)
    {
        const string defaultSectionName = "WebApi";
        configuration
            .GetSection(defaultSectionName)
            .Bind(options);

        return options;
    }
}