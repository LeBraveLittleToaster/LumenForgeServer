using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using NodaTime;

namespace LumenForgeServer.Inventory.Factory;

/// <summary>
/// Factory methods for building vendor domain entities.
/// </summary>
public static class VendorFactory
{
    /// <summary>
    /// Creates a <see cref="Vendor"/> entity from a creation payload.
    /// </summary>
    /// <param name="dto">Payload containing vendor details.</param>
    /// <returns>A new <see cref="Vendor"/> entity.</returns>
    internal static Vendor Create(CreateVendorDto dto)
    {
        var dateNow = SystemClock.Instance.GetCurrentInstant();
        return new Vendor
        {
            Name = dto.Name,
            Guid = Guid.CreateVersion7(),
            CreatedAt = dateNow,
            UpdatedAt = dateNow,
        };
    }
}
