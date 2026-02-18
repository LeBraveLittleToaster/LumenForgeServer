using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.View;
using NodaTime;

namespace LumenForgeServer.Auth.Factory;

public static class UserFactory
{
    public static User BuildUser(AddUserDto dto)
    {
        return new User
        {
            JoinedAt = SystemClock.Instance.GetCurrentInstant(),
            KeycloakUserId = dto.keycloakId
        };
    }
}