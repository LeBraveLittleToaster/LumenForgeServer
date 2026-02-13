using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Rentals.Domain;
using NodaTime;

namespace LumenForgeServer.Maintenance.Domain;

public class MaintenanceBacklog
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long StockId { get; set; }
    public Stock Stock { get; set; } = null!;

    public long? RentalItemId { get; set; }
    public RentalItem? RentalItem { get; set; }

    public long? ChecklistItemId { get; set; }
    public ChecklistItem? ChecklistItem { get; set; }

    public long MaintenanceBacklogStatusId { get; set; }
    public MaintenanceBacklogStatus MaintenanceBacklogStatus { get; set; } = null!;

    public decimal QuantityAffected { get; set; } // > 0
    public string IssueSummary { get; set; } = null!;
    public string? IssueDescription { get; set; }

    public Instant ReportedAt { get; set; }
    public Instant? ResolvedAt { get; set; }

    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }
}
