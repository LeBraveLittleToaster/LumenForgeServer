using System.Reflection.Metadata;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Factory;
using LumenForgeServer.Inventory.Persistance;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Inventory.Service;

public class InventoryService(IInventoryRepository repository)
{
    public async Task<Device> CreateDeviceAsync(CreateDeviceDto dto, CancellationToken ct)
    {
        var vendorId = await repository.TryGetVendorIdByUuidAsync(dto.vendorUuid, ct);
        if (vendorId is null)
            throw new NotFoundException($"Vendor '{dto.vendorUuid}' not found.");

        var categoryIds = await repository.GetCategoryIdsByUuidsAsync(dto.CategoriesUuids, ct);
        if (categoryIds.Count != dto.CategoriesUuids.Count)
        {
            throw new NotFoundException("One or more categories were not found.");
        }

        var device = DeviceFactory.Create(dto, vendorId.Value, categoryIds);

        await repository.AddDeviceAsync(device, ct);

        try
        {
            await repository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("A unique constraint was violated.", ex);
        }

        return device;
    }

    public async Task<Vendor> AddVendor(CreateVendorDto dto, CancellationToken ct)
    {
        var vendor = VendorFactory.Create(dto);
        await repository.AddVendor(vendor, ct);
        await repository.SaveChangesAsync(ct);
        return vendor;
    }

    public async Task<Category> AddCategory(CreateCategoryDTO dto, CancellationToken ct)
    {
        var category = CategoryFactory.Create(dto);
        try
        {
            await repository.AddCategory(category, ct);
            await repository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            throw new UniqueConstraintException("A >Category< unique constraint was violated", ex);
        }
        
        return category;
    }
}


