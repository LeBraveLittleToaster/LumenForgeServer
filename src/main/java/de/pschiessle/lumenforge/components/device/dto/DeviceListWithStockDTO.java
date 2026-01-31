package de.pschiessle.lumenforge.components.device.dto;

import de.pschiessle.lumenforge.components.stock.StockUnitType;

import java.math.BigDecimal;
import java.util.UUID;

public record DeviceListWithStockDTO(
        UUID uuid, String name, String serialNumber,
        BigDecimal stockCount,
        StockUnitType stockUnitType,
        String vendorName
) {}