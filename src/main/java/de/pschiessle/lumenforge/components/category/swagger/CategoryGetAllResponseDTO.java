package de.pschiessle.lumenforge.components.category.swagger;

import java.util.List;

public record CategoryGetAllResponseDTO(
        List<CategoryResponseDTO> content,
        Integer pageNumber,
        Integer pageSize,
        Long totalElements,
        Integer totalPages
) {}
