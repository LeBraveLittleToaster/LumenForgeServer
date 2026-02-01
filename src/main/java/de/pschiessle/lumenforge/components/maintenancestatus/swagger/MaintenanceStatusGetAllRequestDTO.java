package de.pschiessle.lumenforge.components.maintenancestatus.swagger;

public record MaintenanceStatusGetAllRequestDTO(
        Integer page,
        Integer size,
        String sort
) {}
