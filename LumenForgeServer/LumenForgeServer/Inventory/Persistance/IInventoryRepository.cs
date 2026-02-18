using LumenForgeServer.Inventory.Domain;

namespace LumenForgeServer.Inventory.Persistance;

public interface IInventoryRepository
{
    
    Task AddVendor(Vendor vendor, CancellationToken ct);
    Task<long?> TryGetVendorIdByUuidAsync(Guid vendorUuid, CancellationToken ct);
    
    
    Task<IReadOnlyList<long>> GetCategoryIdsByUuidsAsync(IReadOnlyCollection<Guid> uuids, CancellationToken ct);
    
    Task AddDeviceAsync(Device device, CancellationToken ct);
    

    Task AddCategory(Category category, CancellationToken ct);
}

