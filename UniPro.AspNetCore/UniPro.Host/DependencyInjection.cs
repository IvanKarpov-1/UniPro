using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace UniPro.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(configurePolicy:
                policy =>
                {
                    policy
                        .WithOrigins(configuration.GetSection("CorsOrigins").Get<string[]>() ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"http://{configuration["Jwt:Domain"]}";
                options.Audience = configuration["Jwt:Audience"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"http://{configuration["Jwt:Domain"]}/api/auth",
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(LoadRsaKey(configuration)),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidateLifetime = true
                };
            });
        
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }

    private static RSA LoadRsaKey(IConfiguration configuration)
    {
        var rsa = RSA.Create();
        var rsaPublicKeyPath = configuration.GetSection("Jwt")["RsaPublicKeyPath"];

        if (!File.Exists(rsaPublicKeyPath))
        {
            throw new FileNotFoundException("RSA key file not found", rsaPublicKeyPath);
        }
        
        var pem = File.ReadAllText(rsaPublicKeyPath);
        
        if (string.IsNullOrEmpty(pem))
        {
            throw new ApplicationException("RSA key is empty");
        }
        
        rsa.ImportFromPem(pem.ToCharArray());
        return rsa;
    }
}