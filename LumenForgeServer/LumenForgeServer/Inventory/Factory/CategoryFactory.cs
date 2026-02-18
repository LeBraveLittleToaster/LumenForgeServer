using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Dto.View;
using NodaTime;

namespace LumenForgeServer.Inventory.Factory;

public static class CategoryFactory
{
    internal static Category Create(CreateCategoryDTO dto)
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        return new Category
        {
            CreatedAt = now,
            UpdatedAt = now,
            Guid = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
        };
    }

    internal static CategoryViewDto FromCategory(Category category)
    {
        return new CategoryViewDto
        {
            Guid = category.Guid,
            Name = category.Name,
            Description = category.Description
        };
    }
}
