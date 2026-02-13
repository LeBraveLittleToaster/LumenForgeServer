using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using NodaTime;

namespace LumenForgeServer.Inventory.Factory;

public static class VendorFactory
{
    public static Vendor Create(CreateVendorDto dto)
    {
        var dateNow = new Instant();
        var guid = Guid.CreateVersion7();
        Console.WriteLine(guid.ToString());
        return new Vendor
        {
            Name = dto.Name + guid,
            Guid = guid,
            CreatedAt = dateNow,
            UpdatedAt = dateNow,
        };
    }
}