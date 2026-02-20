namespace LumenForgeServer.Auth.Dto;

public record AddGroupDto
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
}