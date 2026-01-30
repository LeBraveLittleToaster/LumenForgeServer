package de.pschiessle.lumenforge.device.maintenancestatus;

public record MaintenanceStatusRequestDTO(
        String name,
        String description
) {}