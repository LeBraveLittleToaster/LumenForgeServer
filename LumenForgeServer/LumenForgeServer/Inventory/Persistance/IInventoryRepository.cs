using LumenForgeServer.Inventory.Domain;

namespace LumenForgeServer.Inventory.Persistance;

public interface IInventoryRepository
{
    // Vendor
    Task AddVendor(Vendor vendor, CancellationToken ct);
    Task<long?> TryGetVendorIdByUuidAsync(Guid vendorUuid, CancellationToken ct);
    
    // Category
    Task<IReadOnlyList<long>> GetCategoryIdsByUuidsAsync(IReadOnlyCollection<Guid> uuids, CancellationToken ct);

    // Device
    Task AddDeviceAsync(Device device, CancellationToken ct);
    
    // General
    Task SaveChangesAsync(CancellationToken ct);

    Task AddCategory(Category category, CancellationToken ct);
}

