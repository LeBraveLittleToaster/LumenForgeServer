package de.pschiessle.lumenforge.components.device.swagger;

public record DeviceGetPageBasicRequestDTO(
        String q,
        Integer page,
        Integer size,
        String sort
) {}
