using Banking.Authentication.Constants;
using Banking.Authentication.CurrentUser;
using Banking.Authentication.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Banking.Authentication.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankingAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser.CurrentUser>();

        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException("Keycloak configuration is missing.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = jwtOptions.Authority;
                options.Audience = jwtOptions.Audience;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var identity = context.Principal?.Identity as ClaimsIdentity;

                        var realmAccessClaim = context.Principal?
                            .FindFirst("realm_access")?
                            .Value;

                        if (identity is not null &&
                            !string.IsNullOrWhiteSpace(realmAccessClaim))
                        {
                            using var document = JsonDocument.Parse(realmAccessClaim);

                            if (document.RootElement.TryGetProperty("roles", out var roles))
                            {
                                foreach (var role in roles.EnumerateArray())
                                {
                                    var roleName = role.GetString();

                                    if (!string.IsNullOrWhiteSpace(roleName))
                                    {
                                        identity.AddClaim(
                                            new Claim(ClaimTypes.Role, roleName));
                                    }
                                }
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthorizationPolicies.Authenticated,
                policy => policy.RequireAuthenticatedUser());

            options.AddPolicy(
                AuthorizationPolicies.Admin,
                policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}