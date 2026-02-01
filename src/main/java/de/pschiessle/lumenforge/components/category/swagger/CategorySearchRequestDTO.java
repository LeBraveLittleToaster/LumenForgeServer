package de.pschiessle.lumenforge.components.category.swagger;

public record CategorySearchRequestDTO(
        String query,
        Integer page,
        Integer size,
        String sort
) {}
