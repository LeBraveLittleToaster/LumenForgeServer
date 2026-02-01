package de.pschiessle.lumenforge.components.stock.swagger;

import java.math.BigDecimal;
import java.util.UUID;

public record StockPatchCountRequestDTO(
        UUID deviceUuid,
        BigDecimal stockCount
) {}
