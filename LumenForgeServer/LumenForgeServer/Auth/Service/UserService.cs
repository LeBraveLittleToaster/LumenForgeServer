using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Validator;
using LumenForgeServer.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Service;


public class UserService(IUserRepository userRepository)
{

    public async Task<User?> AddUser(AddDtos addDtos, CancellationToken ct) 
    {
        var user = new User
        {
            KeycloakUserId = addDtos.keycloakId
        };
        UserValidator.ValidateAddUser(addDtos);
        await userRepository.AddUserAsync(user, ct);
        return user;
    }

    public async Task AssignUserToGroup(AssignUserToGroupDto dto, CancellationToken ct)
    {
        UserValidator.ValidateAssignUserToGroup(dto);
        
        await userRepository.AssignUserToGroupAsync(dto.assigneeKeycloakId, dto.keycloakId, dto.groupGuid, ct);
    }
    
    public async Task<HashSet<Role>> GetRolesForKeycloakId(string keycloakId, CancellationToken ct)
    {
        return await userRepository.GetRolesForKeycloakIdAsync(keycloakId,ct);
    }
}