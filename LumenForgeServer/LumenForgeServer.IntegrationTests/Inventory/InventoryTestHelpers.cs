using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Fixtures;
using LumenForgeServer.IntegrationTests.TestSupport;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Dto.View;

namespace LumenForgeServer.IntegrationTests.Inventory;

/// <summary>
/// Shared helper methods for inventory integration tests.
/// </summary>
internal static class InventoryTestHelpers
{
    
    public static HttpClient CreateAnonymousClient(AuthFixture fixture)
    {
        return new HttpClient();
    }

    public static Task<TestAppClient> CreateAdminClientAsync(AuthFixture fixture)
    {
        //return fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        return null;
    }

    public static Task<TestAppClient> CreateNonAdminClientAsync(AuthFixture fixture)
    {
        return null;
        /*
        var guid = Guid.NewGuid().ToString("N");
        return fixture.CreateNewTestUserClientAsync(new TestUserInfo(
            Username: "InventoryNoAdmin" + guid,
            Password: "Password" + guid,
            Email: "inventory-no-admin-" + guid + "@test.de",
            FirstName: "Inventory",
            LastName: "NoAdmin",
            Groups: [],
            RealmRoles: []), CancellationToken.None);
            */
    }

    public static async Task<CategoryView> CreateCategoryAsync(TestAppClient kcClient, string? name = null)
    {
        return null;
        /*
        var categoryName = name ?? $"Category-{Guid.NewGuid()}";
        var response = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/inventory/categories", new CreateCategoryDto
        {
            Name = categoryName,
            Description = "Category description " + Guid.NewGuid()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return await DeserializeResponseAsync<CategoryView>(response);
        */
    }

    public static async Task<VendorView> CreateVendorAsync(TestAppClient kcClient, string? name = null)
    {
        var vendorName = name ?? $"Vendor-{Guid.NewGuid()}";
        var response = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/inventory/vendors", new CreateVendorDto
        {
            Name = vendorName
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return await DeserializeResponseAsync<VendorView>(response);
    }

    public static async Task<DeviceView> CreateDeviceAsync(
        TestAppClient kcClient,
        Guid vendorGuid,
        IReadOnlyCollection<Guid>? categoryGuids = null,
        string? serialNumber = null)
    {
        var response = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/inventory/devices", new CreateDeviceDto
        {
            SerialNumber = serialNumber ?? $"SN-{Guid.NewGuid()}",
            Name = "Device " + Guid.NewGuid(),
            Description = "Device Description " + Guid.NewGuid(),
            PhotoUrl = "https://example.com/device.jpg",
            VendorGuid = vendorGuid,
            PurchasePrice = 1234.56m,
            PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow.Date),
            Stock = new CreateStockDto
            {
                StockCount = 5,
                StockUnitType = StockUnitType.UNIT
            },
            Parameters =
            [
                new CreateDeviceParameterDto
                {
                    Key = "manual_url",
                    Value = "https://example.com/manual.pdf"
                }
            ],
            CategoryGuids = categoryGuids?.ToList() ?? []
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return await DeserializeResponseAsync<DeviceView>(response);
    }

    public static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        var payload = JsonSerializer.Deserialize<T>(body, Json.GetJsonSerializerOptions());
        payload.Should().NotBeNull();
        return payload!;
    }
}
