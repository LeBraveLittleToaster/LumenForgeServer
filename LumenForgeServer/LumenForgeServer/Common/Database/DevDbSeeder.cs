using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Service;

namespace LumenForgeServer.Common.Database;

/// <summary>
/// Utility to reset and seed the development database with sample data.
/// </summary>
public static class DevDbSeeder
{
    /// <summary>
    /// Deletes the database, recreates it, and inserts minimal seed data.
    /// </summary>
    /// <param name="serviceProvider">Service provider used to resolve scoped services.</param>
    /// <returns>A task that completes when seeding is finished.</returns>
    /// <remarks>
    /// This uses <c>EnsureDeletedAsync</c> and <c>EnsureCreatedAsync</c> rather than migrations.
    /// The current implementation does not await the vendor creation task.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required services are missing from the service provider.
    /// </exception>
    /// <exception cref="Exception">
    /// Re-throws any exception that occurs during delete, create, or seed operations.
    /// </exception>
    public static async Task DeleteAndSeedDbAsync(IServiceProvider serviceProvider)
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

            logger.LogInformation("Creating database without migrations...");
            await db.Database.EnsureCreatedAsync();

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
