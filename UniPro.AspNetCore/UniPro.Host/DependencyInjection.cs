using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
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

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = OnTokenValidated,
                    OnMessageReceived = OnMessageReceived,
                };
            });

        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build())
            .AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "admin"))
            .AddPolicy("TeacherPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "teacher"))
            .AddPolicy("StudentPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "student"));

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

    private static Task OnTokenValidated(TokenValidatedContext context)
    {
        if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity) return Task.CompletedTask;

        var roleClaim = claimsIdentity.FindFirst("st-role");

        if (roleClaim is null) return Task.CompletedTask;

        var json = JsonDocument.Parse(roleClaim.Value);

        if (!json.RootElement.TryGetProperty("v", out var rolesArray)) return Task.CompletedTask;

        foreach (var role in rolesArray.EnumerateArray())
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
        }

        return Task.CompletedTask;
    }

    private static Task OnMessageReceived(MessageReceivedContext context)
    {
        if (context.Request.Cookies.ContainsKey("sAccessToken"))
        {
            context.Token = context.Request.Cookies["sAccessToken"];
        }

        return Task.CompletedTask;
    }
}