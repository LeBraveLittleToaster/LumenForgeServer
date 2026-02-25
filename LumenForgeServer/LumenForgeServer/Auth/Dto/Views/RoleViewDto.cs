using LumenForgeServer.Auth.Domain;

namespace LumenForgeServer.Auth.Dto.Views;

/// <summary>
/// Represents an application role for API responses.
/// </summary>
public sealed record RoleViewDto
{
    public required string Name { get; init; }
    public required int Value { get; init; }

    public static RoleViewDto FromRole(Role role)
    {
        return new RoleViewDto
        {
            Name = role.ToString(),
            Value = (int)role
        };
    }
}
