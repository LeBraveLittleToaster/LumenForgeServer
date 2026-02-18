using System.Security.Claims;
using System.Text.Json;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Domain.Session;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Service;
using LumenForgeServer.Common.Database;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.Inventory.Controller;
using LumenForgeServer.Inventory.Persistance;
using LumenForgeServer.Inventory.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace LumenForgeServer.Common;

public static class DiRegistration
{
    public static void RegisterDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DB_NAME"), o => o.UseNodaTime());
        });
    }

    public static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<InventoryService>();
        builder.Services.AddScoped<UserService>();
    }

    public static void RegisterExceptionHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(configure =>
        {
            configure.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });
        builder.Services.AddExceptionHandler<UniqueConstraintExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void RegisterSwagger(WebApplicationBuilder builder, string keycloakAuthority, string keycloakClientId)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "LumenForge API",
                Version = "v0.0.1"
            });

            options.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect scope" },
                            { "profile", "User profile" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(nameof(SecuritySchemeType.OAuth2), doc),
                    []
                }
            });
        });
    }

    public static void AddAuthenticationJwt(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IKeycloakUser, KeycloakUser>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Keycloak:Issuer"]!;
                options.Audience = builder.Configuration["Keycloak:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Keycloak:Issuer"],
                    RoleClaimType = "roles",
                    NameClaimType = "preferred_username"
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var identity = ctx.Principal?.Identity as ClaimsIdentity;
                        if (identity is null) return;

                        var keycloakUserId = ctx.Principal?.FindFirst("sub")?.Value;
                        if (keycloakUserId is null) return;

                        AddKeycloakRoles(identity);

                        var userService = ctx.HttpContext.RequestServices.GetRequiredService<UserService>();
                        var roles = await userService.GetRolesForKeycloakId(keycloakUserId, CancellationToken.None);

                        identity.AddClaims(
                            roles.Select(x => new Claim(ClaimTypes.Role, x.ToString())).ToList());
                    }
                };
                options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
            });
        builder.Services.AddAuthorization();
    }

    private static void AddKeycloakRoles(ClaimsIdentity identity)
    {
        var realmAccess = identity.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess) || !realmAccess.TrimStart().StartsWith("{")) return;
        using var doc = JsonDocument.Parse(realmAccess);
        if (!doc.RootElement.TryGetProperty("roles", out var roles) ||
            roles.ValueKind != JsonValueKind.Array) return;
        foreach (var r in roles.EnumerateArray().Select(x => x.GetString())
                     .Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, r!));
        }
    }

    public static void AddAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Administrator", p => p.RequireRole("REALM_ADMIN"))
                .AddPolicy("OwnerOnly", p => p.RequireRole("REALM_OWNER"));
        });
    }
}