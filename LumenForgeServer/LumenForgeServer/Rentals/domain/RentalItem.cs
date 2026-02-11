using System;
using System.Collections.Generic;
using RentalDomain.Inventory;
using RentalDomain.Maintenance;

namespace LumenForgeServer.Rentals.domain;

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
    public DateTimeOffset? ApprovedAt { get; set; }

    // nullable (customer or provider) - Keycloak user id
    public string? ApprovedByUserId { get; set; }

    public string? ConditionNotes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<ChecklistItem> ChecklistItems { get; set; } = new();
    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
