package de.pschiessle.lumenforge.components.stock.request;

import de.pschiessle.lumenforge.components.general.IConvertableDTO;
import de.pschiessle.lumenforge.components.stock.Stock;
import de.pschiessle.lumenforge.components.stock.StockUnitType;

import java.math.BigDecimal;
import java.util.Optional;

public record StockCreateDTO(StockUnitType stockUnitType, BigDecimal stockCount, boolean isFractional) implements IConvertableDTO<Stock> {
    @Override
    public Stock fromDTO() {
        if (stockUnitType == null) {
            throw new IllegalArgumentException("stockUnitType must not be null");
        }
        if (stockCount == null) {
            throw new IllegalArgumentException("stockCount must not be null");
        }

        var stock = new Stock();
        stock.setStockCount(stockCount);
        stock.setStockUnitType(stockUnitType);
        stock.setFractional(isFractional);
        return stock;
    }
}
