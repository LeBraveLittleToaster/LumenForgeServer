using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Common.Exceptions;

namespace LumenForgeServer.Auth.Validator;

/// <summary>
/// Validates auth-related DTOs and throws ValidationException on errors.
/// </summary>
public static class GroupRequestValidator
{
    

    /// <summary>
    /// Validates a group assignment payload.
    /// </summary>
    /// <param name="dto">The payload to validate.</param>
    /// <exception cref="ValidationException">Thrown when required fields are missing or invalid.</exception>
    public static void ValidateAssignUserToGroupRequest(
        string groupGuidStr,
        AssignUserToGroupDto dto,
        out Guid groupGuid)
    {
        var errors = new Dictionary<string, string[]>();
        groupGuid = Guid.Empty;

        if (string.IsNullOrWhiteSpace(dto.userKcId))
        {
            errors.Add("keycloakId", ["Length must be greater than 0"]);
        }

        if (string.IsNullOrWhiteSpace(groupGuidStr) ||
            !Guid.TryParse(groupGuidStr, out var parsedGroupGuid))
        {
            errors.Add("groupGuid", ["Must be a valid non-empty GUID"]);
        }
        else
        {
            groupGuid = parsedGroupGuid;
        }

        if (errors.Count > 0)
        {
            Console.WriteLine(errors.ToString());
            throw new ValidationException(
                "Failed to validate Assign User to Group Dto",
                errors);
        }
    }

    public static void ValidateAddGroup(AddGroupDto dto)
    {
        var errors = new Dictionary<string, string[]>();
        
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            errors.Add("name", ["Length must be greater than 0"]);
        }

        if (string.IsNullOrWhiteSpace(dto.Description) ||
            dto.Description.Length < 10)
        {
            errors.Add("description", ["Must be more than 10 characters"]);
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(
                "Failed to validate Add Group Dto",
                errors);
        }
        
    }

    public static void ValidateGetGroup(string groupGuidStr, out Guid groupGuid)
    {
        var errors = new Dictionary<string, string[]>();
        groupGuid = Guid.Empty;
        
        if (string.IsNullOrWhiteSpace(groupGuidStr))
        {
            errors.Add("groupGuidStr", ["Length must be greater than 0"]);
        }

        try
        {
            groupGuid = Guid.Parse(groupGuidStr);
        }
        catch (Exception e)
        {
            errors.Add("groupGuid", ["Must be a valid non-empty GUID"]);
        }
        
        if (errors.Count > 0)
        {
            throw new ValidationException(
                "Failed to validate Get Group",
                errors);
        }
    }

    public static void ValidateDeleteGroup(string groupGuidStr, out Guid groupGuid)
    {
        var errors = new Dictionary<string, string[]>();
        groupGuid = Guid.Empty;
        if (string.IsNullOrWhiteSpace(groupGuidStr))
        {
            errors.Add("groupGuidStr", ["Must be a valid non-empty GUID"]);
            throw new ValidationException(
                "Failed to validate Delete Group", errors);
        }
        try
        {
            groupGuid = Guid.Parse(groupGuidStr);
        }
        catch (Exception e)
        {
            errors.Add("groupGuid", ["Must be a valid non-empty GUID"]);
            throw new ValidationException(
                "Failed to validate Group Guid str",errors);
        }
    
    }
}
