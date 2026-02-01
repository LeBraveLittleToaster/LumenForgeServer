package de.pschiessle.lumenforge.components.device.swagger;

import de.pschiessle.lumenforge.components.device.dto.DeviceListWithStockDTO;

import java.util.List;

public record DeviceGetPageWithStockResponseDTO(
        List<DeviceListWithStockDTO> content,
        Integer pageNumber,
        Integer pageSize,
        Long totalElements,
        Integer totalPages
) {}
