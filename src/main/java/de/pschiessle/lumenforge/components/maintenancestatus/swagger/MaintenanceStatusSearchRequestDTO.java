package de.pschiessle.lumenforge.components.maintenancestatus.swagger;

public record MaintenanceStatusSearchRequestDTO(
        String query,
        Integer page,
        Integer size,
        String sort
) {}
