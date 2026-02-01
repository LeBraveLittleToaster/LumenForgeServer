package de.pschiessle.lumenforge.components.vendor.swagger;

public record VendorSearchRequestDTO(
        String query,
        Integer page,
        Integer size,
        String sort
) {}
