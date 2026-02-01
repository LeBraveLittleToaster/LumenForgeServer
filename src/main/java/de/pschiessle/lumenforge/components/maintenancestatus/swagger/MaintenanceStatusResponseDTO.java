package de.pschiessle.lumenforge.components.maintenancestatus.swagger;

public record MaintenanceStatusResponseDTO(
        Long id,
        String uuid,
        String name,
        String description
) {}
