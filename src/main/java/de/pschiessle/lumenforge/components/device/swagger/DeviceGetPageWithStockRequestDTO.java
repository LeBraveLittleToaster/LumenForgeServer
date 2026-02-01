package de.pschiessle.lumenforge.components.device.swagger;

public record DeviceGetPageWithStockRequestDTO(
        String q,
        Integer page,
        Integer size,
        String sort
) {}
