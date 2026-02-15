using LumenForgeServer.Common.Auth;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.Common.Persistance;
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
    }

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<InventoryService>();
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

    public static void AddAuthenticationJWT(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Keycloak:Issuer"]!;
                options.Audience = builder.Configuration["Keycloak:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Keycloak:Issuer"],
                    RoleClaimType = "roles",
                    NameClaimType = "preferred_username"
                };

                options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

            });
        builder.Services.AddAuthorization();
    }

    public static void addAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireRole("REALM_ADMIN"));
                options.AddPolicy("OwnerOnly", p => p.RequireRole("REALM_OWNER"));
            });
        });
    }
}