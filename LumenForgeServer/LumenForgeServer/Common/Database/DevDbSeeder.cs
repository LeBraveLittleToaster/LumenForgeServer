using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Service;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Service;
using NodaTime;

namespace LumenForgeServer.Common.Database;

/// <summary>
/// Utility to reset and seed the development database with sample data.
/// </summary>
public static class DevDbSeeder
{
    public static AddKcUserDto TEST_ADMIN_USER = new AddKcUserDto
    {
        Username = "InitialAdminuser",
        Password = "admin",
        Email = "admin@admin.de",
        FirstName = "Initial",
        LastName = "Admin",
        RealmRoles = ["REALM_ADMIN", "REALM_OWNER"]
    };
    public static string? TEST_ADMIN_USER_KC_ID = null;
    /// <summary>
    /// Deletes the database, recreates it, and inserts minimal seed data.
    /// </summary>
    /// <param name="serviceProvider">Service provider used to resolve scoped services.</param>
    /// <returns>A task that completes when seeding is finished.</returns>
    /// <remarks>
    /// This uses <c>EnsureDeletedAsync</c> and <c>EnsureCreatedAsync</c> rather than migrations.
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

        var kcService = scope.ServiceProvider.GetRequiredService<KcService>();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        

        var vendorService = scope.ServiceProvider.GetRequiredService<VendorService>();
        var categoryService = scope.ServiceProvider.GetRequiredService<CategoryService>();

        try
        {
            logger.LogWarning("Starting development database reset.");

            logger.LogInformation("Deleting database...");
            await db.Database.EnsureDeletedAsync();

            logger.LogInformation("Creating database without migrations...");
            await db.Database.EnsureCreatedAsync();

            logger.LogInformation("Seeding dummy data...");

            await kcService.DeleteUsersFromKeycloakByUsernamePrefix("test", CancellationToken.None);
            
            try
            {
                TEST_ADMIN_USER_KC_ID = await kcService.AddUserToKeycloak(TEST_ADMIN_USER, CancellationToken.None);
            }
            catch (UniqueConstraintException e)
            {
                await kcService.DeleteUserFromKeycloakByUsername(TEST_ADMIN_USER.Username, CancellationToken.None);
                TEST_ADMIN_USER_KC_ID = await kcService.AddUserToKeycloak(TEST_ADMIN_USER, CancellationToken.None);
            }
            
            await userService.AddUser(TEST_ADMIN_USER_KC_ID, CancellationToken.None);
            
            await vendorService.CreateVendor(new CreateVendorDto { Name = "Some Cool Vendor" }, CancellationToken.None);

            for (var i = 0; i < 10; i++)
            {
                await categoryService.CreateCategory(new CreateCategoryDto { Name = "Some Category " + i, Description = "Description " + i}, CancellationToken.None);
            }

            if (!db.MaintenanceStatuses.Any())
            {
                var now = SystemClock.Instance.GetCurrentInstant();
                db.MaintenanceStatuses.Add(new MaintenanceStatus
                {
                    Uuid = Guid.CreateVersion7(),
                    Name = "Operational",
                    Description = "Default maintenance status.",
                    CreatedAt = now,
                    UpdatedAt = now
                });
                await db.SaveChangesAsync();
                logger.LogInformation("Seeded default maintenance status.");
            }

            logger.LogInformation("Development database reset and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while resetting and seeding the development database.");
            throw;
        }
    }
}
