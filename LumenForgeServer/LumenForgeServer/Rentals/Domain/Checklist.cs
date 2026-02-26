using LumenForgeServer.Common;
using NodaTime;

namespace LumenForgeServer.Rentals.Domain;

/// <summary>
/// Represents a pickup or dropoff checklist associated with a rental.
/// </summary>
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

    public Instant GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public Instant? SignedAt { get; set; }
    public string? SignedByUserId { get; set; } // Keycloak user id

    public string? Notes { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<ChecklistItem> Items { get; set; } = new();
}
