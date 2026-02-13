using LumenForgeServer.Inventory.Dto.Create;

namespace LumenForgeServer.Inventory.Validator;

public static class CategoryValidator
{
    public static bool ValidateCreateCategoryDto(CreateCategoryDTO dto)
    {
        return dto.Name.Length is >= 1 and <= 100;
    }
}