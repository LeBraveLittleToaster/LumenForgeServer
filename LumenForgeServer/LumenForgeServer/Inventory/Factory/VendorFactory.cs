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
    /// <remarks>
    /// The current implementation appends a generated GUID to the vendor name and writes it to stdout.
    /// </remarks>
    internal static Vendor Create(CreateVendorDto dto)
    {
        var dateNow = SystemClock.Instance.GetCurrentInstant();
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
