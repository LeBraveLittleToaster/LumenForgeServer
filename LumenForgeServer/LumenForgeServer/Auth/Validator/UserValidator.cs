using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Common.Exceptions;

namespace LumenForgeServer.Auth.Validator;

/// <summary>
/// Validates auth-related DTOs and throws ValidationException on errors.
/// </summary>
public static class UserValidator
{
    /// <summary>
    /// Validates a user creation payload.
    /// </summary>
    /// <param name="addUserDto">The payload to validate.</param>
    /// <exception cref="ValidationException">Thrown when required fields are missing or invalid.</exception>
    public static void ValidateAddUser(AddUserDto addUserDto)
    {
        if (addUserDto.keycloakId.Length is 0)
        {
            throw new ValidationException("Failed to validate Add User Dto",
                errors: new Dictionary<string, string[]>
                {
                    { "keycloakId", ["Length must be greater than to 0"] },
                });
        }
    }

    /// <summary>
    /// Validates a group assignment payload.
    /// </summary>
    /// <param name="dto">The payload to validate.</param>
    /// <exception cref="ValidationException">Thrown when required fields are missing or invalid.</exception>
    public static void ValidateAssignUserToGroup(AssignUserToGroupDto dto)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.keycloakId.Length is not 0)
        {
            errors.Add("keycloakId", ["Length must be greater than to 0"]);
        }

        if (dto.groupGuid.ToString().Length is not 0)
        {
            errors.Add("groupGuid", ["Length must be greater than to 0"]);
        }

        if (errors.Count > 0)
        {
            throw new ValidationException("Failed to validate Assign User to Group Dto", errors);    
        }
    }
}
