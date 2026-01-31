package de.pschiessle.lumenforge.device;

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
        Long maintenanceStatusId
) {}
