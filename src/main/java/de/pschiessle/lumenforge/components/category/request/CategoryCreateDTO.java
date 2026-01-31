package de.pschiessle.lumenforge.components.category.request;

import de.pschiessle.lumenforge.components.category.Category;
import de.pschiessle.lumenforge.components.general.IConvertableDTO;

import java.util.Optional;

public record CategoryCreateDTO(
        String name,
        String description
) implements IConvertableDTO<Category> {

    @Override
    public Category fromDTO() {
        if (name == null) {
            throw new IllegalArgumentException("Category Name should not be null");
        }

        var category = new Category();
        category.setName(name);
        category.setDescription(description);
        return category;
    }
}