using LumenForgeServer.Common;
using LumenForgeServer.Common.Persistance;

var builder = WebApplication.CreateBuilder(args);

DiRegistration.RegisterDbContext(builder);
DiRegistration.RegisterRepositories(builder);
DiRegistration.RegisterServices(builder);
DiRegistration.RegisterControllers(builder);


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await DevDbSeeder.ResetAndSeedAsync(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();