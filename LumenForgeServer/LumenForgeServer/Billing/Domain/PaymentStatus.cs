using NodaTime;

namespace LumenForgeServer.Billing.Domain;

/// <summary>
/// Lookup entity describing the status of a payment.
/// </summary>
public class PaymentStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
}
