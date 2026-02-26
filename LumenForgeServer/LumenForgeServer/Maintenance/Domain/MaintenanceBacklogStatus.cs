using NodaTime;

namespace LumenForgeServer.Maintenance.Domain;

/// <summary>
/// Lookup entity describing workflow status for a maintenance backlog entry.
/// </summary>
public class MaintenanceBacklogStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
