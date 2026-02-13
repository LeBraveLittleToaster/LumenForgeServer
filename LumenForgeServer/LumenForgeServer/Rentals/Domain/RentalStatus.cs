using NodaTime;

namespace LumenForgeServer.Rentals.Domain;

public class RentalStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<Rental> Rentals { get; set; } = new();
}
