package de.pschiessle.lumenforge.components.device;

import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;

public record DeviceRequestDTO(
        String serialNumber,
        String name,
        String description,
        String photoUrl,
        Long vendorId,
        BigDecimal purchasePrice,
        LocalDate purchaseDate,
        List<Long> categoryIds,
        Long maintenanceStatusId,
        StockRequestDTO stockRequestDTO
) {}
