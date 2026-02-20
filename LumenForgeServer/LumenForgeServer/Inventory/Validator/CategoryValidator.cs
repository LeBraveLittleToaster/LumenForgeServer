using LumenForgeServer.Inventory.Dto.Create;

namespace LumenForgeServer.Inventory.Validator;

/// <summary>
/// Validation helpers for inventory categories.
/// </summary>
public static class CategoryValidator
{
    /// <summary>
    /// Validates a category creation payload.
    /// </summary>
    /// <param name="dto">Payload to validate.</param>
    /// <returns><c>true</c> when the payload is valid; otherwise <c>false</c>.</returns>
    public static bool ValidateCreateCategoryDto(CreateCategoryDTO dto)
    {
        return dto.Name.Length is >= 1 and <= 100;
    }
}
