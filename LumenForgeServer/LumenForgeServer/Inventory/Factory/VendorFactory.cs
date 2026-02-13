using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;

namespace LumenForgeServer.Inventory.Factory;

public static class VendorFactory
{
    public static Vendor Create(CreateVendorDto dto)
    {
        var dateNow = DateTime.UtcNow;
        return new Vendor
        {
            Name = dto.Name,
            Guid = Guid.NewGuid(),
            CreatedAt = dateNow,
            UpdatedAt = dateNow,
        };
    }
}