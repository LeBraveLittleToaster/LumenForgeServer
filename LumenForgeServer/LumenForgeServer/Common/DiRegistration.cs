using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.Common.Persistance;
using LumenForgeServer.Inventory.Controller;
using LumenForgeServer.Inventory.Persistance;
using LumenForgeServer.Inventory.Service;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Common;

public static class DiRegistration
{
    public static void RegisterDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DB_NAME"), o => o.UseNodaTime()));
    }

    public static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
    }

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<InventoryService>();
    }


    public static void RegisterControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<InventoryController>();
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
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}