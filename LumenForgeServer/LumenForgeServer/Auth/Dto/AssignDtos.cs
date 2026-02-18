namespace LumenForgeServer.Auth.Dto;

public record AssignUserToGroupDto
{
    public string? assigneeKeycloakId;
    public required string keycloakId;
    public Guid groupGuid;
}