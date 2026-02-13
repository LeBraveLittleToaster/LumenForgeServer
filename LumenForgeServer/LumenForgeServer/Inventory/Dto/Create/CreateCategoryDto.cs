namespace LumenForgeServer.Inventory.Dto.Create;

public record CreateCategoryDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}