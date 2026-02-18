using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;

namespace LumenForgeServer.Inventory.Factory;

public static class DeviceFactory
{
    internal static Device Create(CreateDeviceDto dto, long vendorId, IReadOnlyList<long> categoryIds)
    {
        return new Device
        {
            SerialNumber = dto.SerialNumber,
            DeviceName = dto.Name,
            DeviceDescription = dto.Description,
            VendorId = vendorId,
            PurchaseDate = dto.PurchaseDate,
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