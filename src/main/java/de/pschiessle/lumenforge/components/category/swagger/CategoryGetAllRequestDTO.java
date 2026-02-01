package de.pschiessle.lumenforge.components.category.swagger;

public record CategoryGetAllRequestDTO(
        Integer page,
        Integer size,
        String sort
) {}
