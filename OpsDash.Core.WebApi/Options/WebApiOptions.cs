using OpsDash.Core.Auth;

namespace OpsDash.Core.WebApi.Options;

public sealed record WebApiOptions
{
    public const string Section = "WebApi";
    public string ServiceName { get; set; } = string.Empty;
    public bool ApiVersioningEnabled { get; private set; } = true;
    public string DefaultApiVersion { get; private set; } = "1.0";
    public AuthOptions Auth { get; } = new();
}