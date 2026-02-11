using System;
using System.Collections.Generic;
using RentalDomain.Billing;

namespace LumenForgeServer.Rentals.domain;

public class Rental
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalStatusId { get; set; }
    public RentalStatus RentalStatus { get; set; } = null!;

    // Keycloak user id
    public string CustomerUserId { get; set; } = null!;

    public string? RequestTitle { get; set; }
    public string? RequestDescription { get; set; }
    public DateTimeOffset? RequestedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? PickupAt { get; set; }
    public DateTimeOffset? DropoffAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset? InvoicedAt { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public DateTimeOffset? ReportedAt { get; set; }

    public string? AssignedByUserId { get; set; }
    public DateTimeOffset? AssignedAt { get; set; }

    public string? PickupProcessedByUserId { get; set; }
    public string? DropoffProcessedByUserId { get; set; }
    public string? CompletedByUserId { get; set; }

    public bool IsScrapped { get; set; }
    public DateTimeOffset? ScrappedAt { get; set; }
    public string? ScrappedByUserId { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public List<RentalItem> Items { get; set; } = new();
    public List<Checklist> Checklists { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();

    public RentalReport RentalReport { get; set; } = null!;
}
