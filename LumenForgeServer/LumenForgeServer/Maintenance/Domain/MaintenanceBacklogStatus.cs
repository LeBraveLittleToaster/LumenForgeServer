namespace LumenForgeServer.Maintenance.Domain;

public class MaintenanceBacklogStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
