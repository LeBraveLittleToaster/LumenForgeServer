using System;
using System.Collections.Generic;
using RentalDomain.Maintenance;

namespace LumenForgeServer.Rentals.domain;

public class ChecklistItem
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long ChecklistId { get; set; }
    public Checklist Checklist { get; set; } = null!;

    public long RentalItemId { get; set; }
    public RentalItem RentalItem { get; set; } = null!;

    public decimal QuantityChecked { get; set; } // > 0
    public bool ConditionOk { get; set; }
    public string? ConditionNotes { get; set; }

    public decimal DamagedQuantity { get; set; } // >= 0
    public string? DamageSummary { get; set; }
    public string? DamageDescription { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
