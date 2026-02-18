using LumenForgeServer.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LumenForgeServer.Common.Database;


var builder = WebApplication.CreateBuilder(args);

var keycloakAuthority = builder.Configuration["Keycloak:Authority"]!;
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;

DiRegistration.AddAuthenticationJwt(builder);

DiRegistration.RegisterDbContext(builder);
DiRegistration.RegisterRepositories(builder);
DiRegistration.RegisterServices(builder);
DiRegistration.RegisterSwagger(builder, keycloakAuthority, keycloakClientId);

DiRegistration.RegisterExceptionHandler(builder);

builder.Services.AddControllers();

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