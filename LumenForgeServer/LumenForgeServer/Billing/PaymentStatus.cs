using System;
using System.Collections.Generic;

namespace RentalDomain.Billing;

public class PaymentStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
}
