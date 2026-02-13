using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Dto.View;
using NodaTime;

namespace LumenForgeServer.Inventory.Factory;

public static class CategoryFactory
{
    public static Category Create(CreateCategoryDTO dto)
    {
        var now = new Instant();
        return new Category
        {
            CreatedAt = now,
            UpdatedAt = now,
            Guid = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
        };
    }

    public static CategoryViewDto FromCategory(Category category)
    {
        return new CategoryViewDto(category.Guid, category.Name, category.Description);
    }
}
