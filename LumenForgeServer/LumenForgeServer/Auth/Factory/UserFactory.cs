using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.View;
using NodaTime;

namespace LumenForgeServer.Auth.Factory;

/// <summary>
/// Builds User domain objects from auth DTOs.
/// </summary>
public static class UserFactory
{
    /// <summary>
    /// Builds a <see cref="User"/> instance from a user creation payload.
    /// </summary>
    /// <param name="dto">Payload containing the Keycloak subject identifier.</param>
    /// <returns>A new user instance with joined timestamp set.</returns>
    public static User BuildUser(AddUserDto dto)
    {
        return new User
        {
            JoinedAt = SystemClock.Instance.GetCurrentInstant(),
            KeycloakUserId = dto.keycloakId
        };
    }
}
