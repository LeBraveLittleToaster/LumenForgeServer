package de.pschiessle.lumenforge.components.stock.swagger;

import java.util.UUID;

public record StockDeleteByDeviceRequestDTO(
        UUID deviceUuid
) {}
