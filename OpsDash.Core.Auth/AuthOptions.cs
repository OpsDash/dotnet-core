namespace OpsDash.Core.Auth;

public enum AuthMode
{
    /// <summary>Validate bearer JWT.</summary>
    /// <example>gateway / public edge.</example>
    Jwt = 0,
    /// <summary>Trust identity headers from the gateway.</summary>
    /// <example>backend services.</example>
    ForwardedIdentity = 1
}

public sealed class AuthOptions
{
    public const string Section = "WebApi:Auth";
    public bool Enabled { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 60;
    public AuthMode Mode { get; set; } = AuthMode.Jwt;
}