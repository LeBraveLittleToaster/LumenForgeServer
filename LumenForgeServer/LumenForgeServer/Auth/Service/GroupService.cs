using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Factory;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Validator;
using LumenForgeServer.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Service;


public class GroupService(IAuthRepository authRepository) : ControllerBase
{

    public async Task<long> GetGroupByGuid(Guid guid, CancellationToken ct)
    {
        var groupId = await authRepository.GetGroupIdByGuidAsync(guid, ct);
        return groupId;
    }

    public async Task<User?> AddUser(AddUserDto addUserDto, CancellationToken ct)
    {
        var user = UserFactory.BuildUser(addUserDto);
        
        UserValidator.ValidateAddUser(addUserDto);
        
        await authRepository.AddUserAsync(user, ct);
        await authRepository.SaveChangesAsync(ct);
        
        return user;
    }

    public async Task AssignUserToGroup(AssignUserToGroupDto dto, CancellationToken ct)
    {
        UserValidator.ValidateAssignUserToGroup(dto);
        
        await authRepository.AssignUserToGroupAsync(dto.assigneeKeycloakId, dto.keycloakId, dto.groupGuid, ct);
        await authRepository.SaveChangesAsync(ct);
    }
    
    public async Task<HashSet<Role>> GetRolesForKeycloakId(string keycloakId, CancellationToken ct)
    {
        return await authRepository.GetRolesForKeycloakIdAsync(keycloakId,ct);
    }
}