using System;
using System.Collections.Generic;
using RentalDomain.Common;

namespace LumenForgeServer.Rentals.domain;

public class Checklist
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public ChecklistType ChecklistType { get; set; }

    // dropoff references pickup
    public long? SourceChecklistId { get; set; }
    public Checklist? SourceChecklist { get; set; }
    public List<Checklist> DerivedChecklists { get; set; } = new();

    public DateTimeOffset GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public DateTimeOffset? SignedAt { get; set; }
    public string? SignedByUserId { get; set; } // Keycloak user id

    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<ChecklistItem> Items { get; set; } = new();
}
