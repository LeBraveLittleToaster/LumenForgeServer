package de.pschiessle.lumenforge.components.stock.swagger;

import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;

import java.util.UUID;

public record StockUpsertByDeviceRequestDTO(
        UUID deviceUuid,
        StockRequestDTO stock
) {}
