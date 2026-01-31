package de.pschiessle.lumenforge.components.stock.request;

import de.pschiessle.lumenforge.components.stock.StockUnitType;

import java.math.BigDecimal;

public record StockRequestDTO(StockUnitType stockUnitType, BigDecimal stockCount, boolean isFractional) {
}
