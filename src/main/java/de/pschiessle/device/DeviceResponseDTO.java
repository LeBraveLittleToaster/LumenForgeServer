package de.pschiessle.device;

import de.pschiessle.device.category.Category;

import java.math.BigDecimal;
import java.time.Instant;
import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

public record DeviceResponseDTO(
        Long id,
        UUID uuid,
        String serialNumber,
        String name,
        String description,
        String photoUrl,
        Long vendorId,
        BigDecimal purchasePrice,
        LocalDate purchaseDate,
        List<Long> categoryIds,
        Long maintenanceStatusId,
        Instant createDate,
        Instant updateDate
) {
    public static DeviceResponseDTO from(Device d) {
        return new DeviceResponseDTO(
                d.getId(),
                d.getUuid(),
                d.getSerialNumber(),
                d.getName(),
                d.getDescription(),
                d.getPhotoUrl(),
                d.getVendor() != null ? d.getVendor().getId() : null,
                d.getPurchasePrice(),
                d.getPurchaseDate(),
                d.getCategories() != null ? d.getCategories().stream().map(Category::getId).toList() : List.of(),
                d.getMaintenanceStatus() != null ? d.getMaintenanceStatus().getId() : null,
                d.getCreateDate(),
                d.getUpdateDate()
        );
    }
}
