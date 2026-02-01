package de.pschiessle.lumenforge.components.maintenancestatus.swagger;

import java.util.List;

public record MaintenanceStatusGetAllResponseDTO(
        List<MaintenanceStatusResponseDTO> content,
        Integer pageNumber,
        Integer pageSize,
        Long totalElements,
        Integer totalPages
) {}
