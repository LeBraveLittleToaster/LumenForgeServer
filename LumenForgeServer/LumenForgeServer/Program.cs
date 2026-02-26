using LumenForgeServer.Common;
using LumenForgeServer.Common.Database;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

var keycloakAuthority = builder.Configuration["Keycloak:Authority"]!;
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;




DiRegistration.RegisterMemoryCache(builder);
DiRegistration.AddAuthenticationJwt(builder);
DiRegistration.RegisterDbContext(builder);
DiRegistration.RegisterRepositories(builder);
DiRegistration.RegisterServices(builder);
DiRegistration.RegisterSwagger(builder, keycloakAuthority, keycloakClientId);

DiRegistration.RegisterExceptionHandler(builder);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await DevDbSeeder.DeleteAndSeedDbAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(keycloakClientId);
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace LumenForgeServer
{
    /// <summary>
    /// Marker type used by integration tests to reference the API entry assembly.
    /// </summary>
    public partial class Program { }
}
