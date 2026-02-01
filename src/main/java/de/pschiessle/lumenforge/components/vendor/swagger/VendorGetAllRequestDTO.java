package de.pschiessle.lumenforge.components.vendor.swagger;

public record VendorGetAllRequestDTO(
        Integer page,
        Integer size,
        String sort
) {}
