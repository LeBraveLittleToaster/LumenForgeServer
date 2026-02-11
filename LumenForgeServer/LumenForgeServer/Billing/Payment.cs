using System;
using RentalDomain.Common;

namespace RentalDomain.Billing;

public class Payment
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public long PaymentStatusId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = null!;

    public decimal Amount { get; set; } // > 0
    public string CurrencyCode { get; set; } = null!; // ISO 4217

    public PaymentMethod PaymentMethod { get; set; }
    public string? ProviderReference { get; set; }

    public DateTimeOffset? PaidAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
