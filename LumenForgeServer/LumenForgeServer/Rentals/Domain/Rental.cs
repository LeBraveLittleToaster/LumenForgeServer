using LumenForgeServer.Billing.Domain;

namespace LumenForgeServer.Rentals.Domain;

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
    public DateTime? RequestedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? PickupAt { get; set; }
    public DateTime? DropoffAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? InvoicedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? ReportedAt { get; set; }

    public string? AssignedByUserId { get; set; }
    public DateTime? AssignedAt { get; set; }

    public string? PickupProcessedByUserId { get; set; }
    public string? DropoffProcessedByUserId { get; set; }
    public string? CompletedByUserId { get; set; }

    public bool IsScrapped { get; set; }
    public DateTime? ScrappedAt { get; set; }
    public string? ScrappedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<RentalItem> Items { get; set; } = new();
    public List<Checklist> Checklists { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();

    public RentalReport RentalReport { get; set; } = null!;
}
