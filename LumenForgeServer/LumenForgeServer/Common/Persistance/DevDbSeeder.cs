using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Factory;
using LumenForgeServer.Inventory.Service;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Common.Persistance;

public static class DevDbSeeder
{
    public static async Task ResetAndSeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DevDbSeeder");
        ;

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var inventoryService = scope.ServiceProvider.GetRequiredService<InventoryService>();

        try
        {
            logger.LogWarning("Starting development database reset.");

            logger.LogInformation("Deleting database...");
            await db.Database.EnsureDeletedAsync();

            logger.LogInformation("Applying migrations...");
            await db.Database.MigrateAsync();

            logger.LogInformation("Seeding dummy data...");


            var vendor = inventoryService
                .AddVendor(new CreateVendorDto { Name = "Some Cool Vendor" }, CancellationToken.None);
            logger.LogInformation("Created Vendor: {vendor}", vendor);
            logger.LogInformation("Development database reset and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while resetting and seeding the development database.");
            throw;
        }
    }
}