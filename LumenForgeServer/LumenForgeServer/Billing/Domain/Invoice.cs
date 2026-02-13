using LumenForgeServer.Rentals.Domain;

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

    public DateTime GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public DateTime? IssuedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public string? InvoiceDocumentUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
}
