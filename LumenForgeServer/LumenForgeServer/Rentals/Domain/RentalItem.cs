using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Maintenance.Domain;

namespace LumenForgeServer.Rentals.Domain;

public class RentalItem
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public long StockId { get; set; }
    public Stock Stock { get; set; } = null!;

    public decimal Quantity { get; set; } // > 0

    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // nullable (customer or provider) - Keycloak user id
    public string? ApprovedByUserId { get; set; }

    public string? ConditionNotes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<ChecklistItem> ChecklistItems { get; set; } = new();
    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
