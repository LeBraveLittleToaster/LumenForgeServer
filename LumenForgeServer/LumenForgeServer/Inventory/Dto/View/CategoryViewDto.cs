namespace LumenForgeServer.Inventory.Dto.View;

public record CategoryViewDto
{
    public Guid Guid;
    public required string Name;
    public string? Description;
}