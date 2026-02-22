using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Dto.View;
using NodaTime;

namespace LumenForgeServer.Inventory.Factory;

/// <summary>
/// Factory methods for building category domain and view models.
/// </summary>
public static class CategoryFactory
{
    /// <summary>
    /// Creates a <see cref="Category"/> entity from a creation payload.
    /// </summary>
    /// <param name="dto">Payload containing category details.</param>
    /// <returns>A new <see cref="Category"/> entity.</returns>
    internal static Category Create(CreateCategoryDTO dto)
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        return new Category
        {
            CreatedAt = now,
            UpdatedAt = now,
            Guid = Guid.CreateVersion7(),
            Name = dto.Name,
            Description = dto.Description,
        };
    }

    /// <summary>
    /// Builds a view model from a category entity.
    /// </summary>
    /// <param name="category">Category entity to transform.</param>
    /// <returns>A <see cref="CategoryViews"/> containing public fields.</returns>
    internal static CategoryViews FromCategory(Category category)
    {
        return new CategoryViews
        {
            Guid = category.Guid,
            Name = category.Name,
            Description = category.Description
        };
    }
}
