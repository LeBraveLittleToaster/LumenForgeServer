using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;

namespace LumenForgeServer.Inventory.Factory;

/// <summary>
/// Factory methods for creating device aggregates.
/// </summary>
public static class DeviceFactory
{
    /// <summary>
    /// Creates a device aggregate from a creation payload and resolved ids.
    /// </summary>
    /// <param name="dto">Payload containing device details.</param>
    /// <param name="vendorId">Resolved internal vendor id.</param>
    /// <param name="categoryIds">Resolved internal category ids.</param>
    /// <returns>A new <see cref="Device"/> aggregate with stock, parameters, and categories.</returns>
    internal static Device Create(CreateDeviceDto dto, long vendorId, IReadOnlyList<long> categoryIds)
    {
        return new Device
        {
            SerialNumber = dto.SerialNumber,
            DeviceName = dto.Name,
            DeviceDescription = dto.Description,
            VendorId = vendorId,
            PurchaseDate = dto.PurchaseDate,
            Guid = Guid.CreateVersion7(),
            Stock = new Stock
            {
                StockCount = dto.Stock.StockCount,
                UnitStockType = dto.Stock.StockUnitType,
            },
            Parameters = dto.Parameters.Select(p => new DeviceParameter
            {
                ParamKey = p.Key,
                Value = p.Value
            }).ToList(),
            DeviceCategories = categoryIds.Select(id => new DeviceCategory
            {
                CategoryId = id
            }).ToList()
        };
    }
}
