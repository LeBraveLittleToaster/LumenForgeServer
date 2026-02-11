using System;
using System.Collections.Generic;
using LumenForgeServer.Rentals.domain;

namespace RentalDomain.Billing;

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

    public DateTimeOffset GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public DateTimeOffset? IssuedAt { get; set; }
    public DateTimeOffset? DueAt { get; set; }
    public DateTimeOffset? PaidAt { get; set; }

    public string? InvoiceDocumentUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
}
