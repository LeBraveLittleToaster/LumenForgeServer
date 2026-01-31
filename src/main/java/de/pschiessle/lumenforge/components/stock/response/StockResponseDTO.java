package de.pschiessle.lumenforge.components.stock.response;

import de.pschiessle.lumenforge.components.stock.Stock;
import de.pschiessle.lumenforge.components.stock.StockUnitType;

import java.math.BigDecimal;
import java.util.UUID;

public record StockResponseDTO(
        UUID deviceUuid,
        UUID stockUuid,
        StockUnitType stockUnitType,
        BigDecimal stockCount,
        boolean isFractional
) {
    public static StockResponseDTO fromClass(Stock stock) {
        return new StockResponseDTO(
                stock.getUuid(),
                stock.getUuid(),
                stock.getStockUnitType(),
                stock.getStockCount(),
                stock.isFractional()
        );
    }
}
