namespace LumenForgeServer.Auth.Dto;

public record AssignUserToGroupDto(string? assigneeKeycloakId, string keycloakId, Guid groupGuid)
{
}