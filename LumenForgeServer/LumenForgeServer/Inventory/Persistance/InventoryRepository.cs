using LumenForgeServer.Common.Database;
using LumenForgeServer.Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Inventory.Persistance;

public sealed class InventoryRepository(AppDbContext _db) : IInventoryRepository
{
    public Task AddVendor(Vendor vendor, CancellationToken ct)
    {
        return _db.Vendors.AddAsync(vendor, ct).AsTask();
    }
    
    public async Task<long?> TryGetVendorIdByUuidAsync(Guid vendorUuid, CancellationToken ct)
    {
        return await _db.Vendors
            .Where(v => v.Guid == vendorUuid)
            .Select(v => (long?)v.Id)
            .SingleOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<long>> GetCategoryIdsByUuidsAsync(
        IReadOnlyCollection<Guid> uuids,
        CancellationToken ct)
    {
        if (uuids.Count == 0) return Array.Empty<long>();

        return await _db.Categories
            .Where(c => uuids.Contains(c.Guid))
            .Select(c => c.Id)
            .ToListAsync(ct);
    }

    public Task AddDeviceAsync(Device device, CancellationToken ct)
        => _db.Devices.AddAsync(device, ct).AsTask();
    
    public Task AddCategory(Category category, CancellationToken ct)
    {
        return _db.Categories.AddAsync(category, ct).AsTask();
    }
}