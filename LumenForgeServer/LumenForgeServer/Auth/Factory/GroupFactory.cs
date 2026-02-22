using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using NodaTime;

namespace LumenForgeServer.Auth.Factory;

public static class GroupFactory
{
    public static Group BuildGroup(AddGroupDto dto)
    {
        var dateNow = SystemClock.Instance.GetCurrentInstant();
        return new Group
        {
            Guid = Guid.CreateVersion7(),
            CreatedAt = dateNow,
            UpdatedAt =  dateNow,
            Name = dto.Name,
            Description = dto.Description,
        };
    }
}