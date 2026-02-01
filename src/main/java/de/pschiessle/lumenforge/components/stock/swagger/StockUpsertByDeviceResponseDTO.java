package de.pschiessle.lumenforge.components.stock.swagger;

import de.pschiessle.lumenforge.components.stock.response.StockResponseDTO;

public record StockUpsertByDeviceResponseDTO(
        StockResponseDTO stock
) {}
