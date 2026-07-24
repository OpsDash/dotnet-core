namespace OpsDash.Core.Auth;

public static class ForwardedIdentityDefaults
{
    public const string AuthenticationScheme = "ForwardedIdentity";
    
    public const string UserIdHeader = "X-User-Id";
    public const string UserNameHeader = "X-User-Name";
    public const string EmailHeader = "X-User-Email";
    public const string RolesHeader = "X-User-Roles";
}