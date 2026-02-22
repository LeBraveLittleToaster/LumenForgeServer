using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Auth.Factory;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Validator;
using LumenForgeServer.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Auth.Service;

/// <summary>
/// Application service for group-related auth operations.
/// </summary>
public class GroupService(IAuthRepository authRepository)
{

    /// <summary>
    /// Resolves a group by group guid.
    /// </summary>
    /// <param name="guid">Group guid to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The group object.</returns>
    /// <exception cref="NotFoundException">Thrown when the group cannot be found.</exception>
    public async Task<GroupView> GetGroupByGuid(Guid guid, CancellationToken ct)
    {
        var group = await authRepository.GetGroupByGuidAsync(guid, ct);
        return group == null ? throw new NotFoundException("Group not found") : GroupView.FromEntity(group);
    }
    
    /// <summary>
    /// Resolves a group id by group guid.
    /// </summary>
    /// <param name="guid">Group guid to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The internal group id.</returns>
    /// <exception cref="NotFoundException">Thrown when the group cannot be found.</exception>
    public async Task<long> GetGroupIdByGuid(Guid guid, CancellationToken ct)
    {
        var groupId = await authRepository.GetGroupIdByGuidAsync(guid, ct);
        return groupId;
    }

    /// <summary>
    /// Creates a group record from a payload.
    /// </summary>
    /// <param name="addGroupDto">Payload containing the name and description.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created group.</returns>
    /// <exception cref="ValidationException">Thrown when the payload fails validation.</exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Thrown when persistence fails.</exception>
    public async Task<GroupView> AddGroup(AddGroupDto dto, CancellationToken ct)
    {
        var group = GroupFactory.BuildGroup(dto);

        try
        {
            await authRepository.AddGroupAsync(group, ct);
            await authRepository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e)
        {
            throw new UniqueConstraintException(e.Message, e);
        }
        
        
        return GroupView.FromEntity(group);
    }
}
