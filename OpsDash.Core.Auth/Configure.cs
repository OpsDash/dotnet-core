using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace OpsDash.Core.Auth;

public static class Configure
{
    public const string LocalScheme = "Local";
    
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWebApiAuth(IWebHostEnvironment env,
            AuthOptions auth
        )
        {
            if (!auth.Enabled)
            {
                return services;
            }

            if (env.IsDevelopment())
            {
                return services.AddLocalAuth(auth);
            }
            
            return auth.Mode switch
            {
                AuthMode.ForwardedIdentity => services.AddForwardedIdentityAuth(),
                _ => services.AddJwtAuth(auth)
            };
        }
        
        private IServiceCollection AddForwardedIdentityAuth()
        {
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = ForwardedIdentityDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = ForwardedIdentityDefaults.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, ForwardedIdentityHandler>(
                    ForwardedIdentityDefaults.AuthenticationScheme, _ => { });

            services.AddAuthorization();
            return services;
        }

        private IServiceCollection AddJwtAuth(AuthOptions auth)
        {
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = CreateTokenValidationParameters(auth);
                });

            services.AddAuthorization();
            return services;
        }
        
        private IServiceCollection AddLocalAuth(AuthOptions auth)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = LocalScheme;
                    options.DefaultChallengeScheme = LocalScheme;
                })
                .AddPolicyScheme(LocalScheme, "JWT or forwarded identity", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.ContainsKey(ForwardedIdentityDefaults.UserIdHeader))
                        {
                            return ForwardedIdentityDefaults.AuthenticationScheme;
                        }
                        
                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                })
                .AddScheme<AuthenticationSchemeOptions, ForwardedIdentityHandler>(
                    ForwardedIdentityDefaults.AuthenticationScheme, _ => { })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = CreateTokenValidationParameters(auth);
                });
            
            services.AddAuthorization();
            
            return services;
        }
    }
    
    

    public static TokenValidationParameters CreateTokenValidationParameters(AuthOptions options)
        => new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
        };
}