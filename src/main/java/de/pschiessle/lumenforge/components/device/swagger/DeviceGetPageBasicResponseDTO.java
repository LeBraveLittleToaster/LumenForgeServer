package de.pschiessle.lumenforge.components.device.swagger;

import de.pschiessle.lumenforge.components.device.dto.DeviceListDTO;

import java.util.List;

public record DeviceGetPageBasicResponseDTO(
        List<DeviceListDTO> content,
        Integer pageNumber,
        Integer pageSize,
        Long totalElements,
        Integer totalPages
) {}
