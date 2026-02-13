using LumenForgeServer.Rentals.Domain;
using NodaTime;

namespace LumenForgeServer.Billing.Domain;

public class Invoice
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public long InvoiceStatusId { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; } = null!;

    public string InvoiceNumber { get; set; } = null!;

    public decimal SubtotalAmount { get; set; } // >= 0
    public decimal TaxAmount { get; set; }      // >= 0
    public decimal TotalAmount { get; set; }    // >= 0

    public string CurrencyCode { get; set; } = null!; // ISO 4217

    public Instant GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public Instant? IssuedAt { get; set; }
    public Instant? DueAt { get; set; }
    public Instant? PaidAt { get; set; }

    public string? InvoiceDocumentUrl { get; set; }

    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
}
