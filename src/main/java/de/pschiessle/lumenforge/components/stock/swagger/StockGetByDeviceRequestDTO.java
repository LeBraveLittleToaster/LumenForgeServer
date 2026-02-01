package de.pschiessle.lumenforge.components.stock.swagger;

import java.util.UUID;

public record StockGetByDeviceRequestDTO(
        UUID deviceUuid
) {}
