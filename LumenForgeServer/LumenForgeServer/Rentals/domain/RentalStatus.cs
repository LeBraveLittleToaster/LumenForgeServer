using System;
using System.Collections.Generic;

namespace LumenForgeServer.Rentals.domain;

public class RentalStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Rental> Rentals { get; set; } = new();
}
