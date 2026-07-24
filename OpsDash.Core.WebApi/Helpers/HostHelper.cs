using Microsoft.Extensions.Configuration;

namespace OpsDash.Core.WebApi.Helpers;

public static class HostHelper
{
    public static bool IsLocalDevelopment(this IConfiguration configuration)
        => configuration["ASPNETCORE_ENVIRONMENT"] == "Development";
}