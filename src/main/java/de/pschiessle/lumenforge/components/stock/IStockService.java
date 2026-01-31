package de.pschiessle.lumenforge.components.stock;

import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;
import de.pschiessle.lumenforge.components.stock.response.StockResponseDTO;

import java.util.UUID;

public interface IStockService {
    StockResponseDTO getByDeviceUuid(UUID deviceUuid);
    StockResponseDTO upsertByDeviceUuid(UUID deviceUuid, StockRequestDTO request);
    StockResponseDTO patchCountByDeviceUuid(UUID deviceUuid, java.math.BigDecimal newCount);
    void deleteByDeviceUuid(UUID deviceUuid);
}