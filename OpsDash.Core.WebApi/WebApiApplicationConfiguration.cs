using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpsDash.Core.WebApi.Options;

namespace OpsDash.Core.WebApi;

public abstract class WebApiApplicationConfiguration
{
    public IConfiguration Configuration { get; private set; } = null!;
    
    internal void Initialize(IConfiguration configuration)
        => Configuration = configuration;
    
    /// <summary>Platform setup.</summary>
    /// <example>auth, observability, etc.</example>
    public virtual void ConfigureWebApi(WebApiOptions options) {}
    
    /// <summary>Host setup.</summary>
    /// <example>Aspire, Orleans, etc.</example>
    /// <remarks>This runs before the platform pipeline.</remarks>
    public virtual void ConfigureHost(WebApplicationBuilder builder) {}
    
    /// <summary> Domain specific dependency injections.</summary>
    /// <example>repositories, services, clients, etc.</example>
    public virtual void ConfigureServices(IServiceCollection services) {}

    /// <summary> App-specific middleware/endpoints.</summary>
    /// <remarks>This runs after the platform pipeline.</remarks>
    public virtual void Configure(WebApplication app) {}
}