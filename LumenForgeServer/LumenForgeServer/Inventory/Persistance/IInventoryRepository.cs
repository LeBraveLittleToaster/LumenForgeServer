using LumenForgeServer.Inventory.Domain;

namespace LumenForgeServer.Inventory.Persistance;

/// <summary>
/// Persistence contract for inventory entities.
/// </summary>
public interface IInventoryRepository
{
    
    /// <summary>
    /// Adds a vendor to the persistence store.
    /// </summary>
    /// <param name="vendor">Vendor entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    Task AddVendor(Vendor vendor, CancellationToken ct);
    /// <summary>
    /// Resolves the internal vendor id for a vendor UUID.
    /// </summary>
    /// <param name="vendorUuid">Vendor UUID to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The vendor id if found; otherwise <c>null</c>.</returns>
    Task<long?> TryGetVendorIdByUuidAsync(Guid vendorUuid, CancellationToken ct);
    
    
    /// <summary>
    /// Resolves internal category ids for a set of category UUIDs.
    /// </summary>
    /// <param name="uuids">Category UUIDs to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of internal category ids that were found.</returns>
    Task<IReadOnlyList<long>> GetCategoryIdsByUuidsAsync(IReadOnlyCollection<Guid> uuids, CancellationToken ct);
    
    /// <summary>
    /// Adds a device aggregate to the persistence store.
    /// </summary>
    /// <param name="device">Device entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    Task AddDeviceAsync(Device device, CancellationToken ct);
    

    /// <summary>
    /// Adds a category to the persistence store.
    /// </summary>
    /// <param name="category">Category entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    Task AddCategory(Category category, CancellationToken ct);

    /// <summary>
    /// Persists pending changes to the underlying data store.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Thrown when persistence fails.</exception>
    Task SaveChangesAsync(CancellationToken ct);
}
