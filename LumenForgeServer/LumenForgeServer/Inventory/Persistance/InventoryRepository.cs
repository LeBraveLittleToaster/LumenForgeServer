using LumenForgeServer.Common.Database;
using LumenForgeServer.Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Inventory.Persistance;

/// <summary>
/// EF Core-backed repository for inventory entities.
/// </summary>
public sealed class InventoryRepository(AppDbContext _db) : IInventoryRepository
{
    /// <summary>
    /// Adds a vendor to the persistence store.
    /// </summary>
    /// <param name="vendor">Vendor entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    public Task AddVendor(Vendor vendor, CancellationToken ct)
    {
        return _db.Vendors.AddAsync(vendor, ct).AsTask();
    }
    
    /// <summary>
    /// Resolves the internal vendor id for a vendor UUID.
    /// </summary>
    /// <param name="vendorUuid">Vendor UUID to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The vendor id if found; otherwise <c>null</c>.</returns>
    public async Task<long?> TryGetVendorIdByUuidAsync(Guid vendorUuid, CancellationToken ct)
    {
        return await _db.Vendors
            .Where(v => v.Guid == vendorUuid)
            .Select(v => (long?)v.Id)
            .SingleOrDefaultAsync(ct);
    }

    /// <summary>
    /// Resolves internal category ids for a set of category UUIDs.
    /// </summary>
    /// <param name="uuids">Category UUIDs to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of internal category ids that were found.</returns>
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

    /// <summary>
    /// Adds a device aggregate to the persistence store.
    /// </summary>
    /// <param name="device">Device entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    public Task AddDeviceAsync(Device device, CancellationToken ct)
        => _db.Devices.AddAsync(device, ct).AsTask();
    
    /// <summary>
    /// Adds a category to the persistence store.
    /// </summary>
    /// <param name="category">Category entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    public Task AddCategory(Category category, CancellationToken ct)
    {
        return _db.Categories.AddAsync(category, ct).AsTask();
    }

    /// <summary>
    /// Persists pending changes to the underlying data store.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="DbUpdateException">Thrown when persistence fails.</exception>
    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
