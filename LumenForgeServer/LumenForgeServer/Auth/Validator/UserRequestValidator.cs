using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Common.Exceptions;

namespace LumenForgeServer.Auth.Validator;

public static class UserRequestValidator
{
    /// <summary>
    /// Validates a user creation payload.
    /// </summary>
    /// <param name="addUserDto">The payload to validate.</param>
    /// <exception cref="ValidationException">Thrown when required fields are missing or invalid.</exception>
    public static void ValidateAddUser(AddUserDto addUserDto)
    {
        if (string.IsNullOrWhiteSpace(addUserDto.userKcId))
        {
            throw new ValidationException("Failed to validate Add User Dto",
                errors: new Dictionary<string, string[]>
                {
                    { "keycloakId", ["Length must be greater than to 0"] },
                });
        }
    }

    public static void ValidateGetUser()
    {
        
    }

    public static void ValidateGetUserRoles()
    {
        
    }

    public static void ValidateDeleteUserByKcId()
    {
        
    }
}