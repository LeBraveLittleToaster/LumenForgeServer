package de.pschiessle.lumenforge.components.stock;

import de.pschiessle.lumenforge.components.device.Device;
import de.pschiessle.lumenforge.components.device.DeviceRepository;
import de.pschiessle.lumenforge.components.stock.request.StockCreateDTO;
import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;
import de.pschiessle.lumenforge.components.stock.response.StockResponseDTO;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.UUID;

import static de.pschiessle.lumenforge.ExceptionHelper.require;

@Service
@RequiredArgsConstructor
@Transactional
public class StockServiceImpl implements IStockService {

    private final StockRepository stockRepository;
    private final DeviceRepository deviceRepository;

    @Override
    @Transactional(readOnly = true)
    public StockResponseDTO getByDeviceUuid(UUID deviceUuid) {
        Stock stock = stockRepository.findByDevice_Uuid(deviceUuid)
                .orElseThrow(() -> new EntityNotFoundException("Stock not found for deviceUuid=" + deviceUuid));
        return toDto(stock);
    }

    @Override
    public StockResponseDTO upsertByDeviceUuid(UUID deviceUuid, StockRequestDTO request) {
        require(request.stockUnitType(), "stockUnitType must not be null");
        require(request.stockCount(), "stockCount must not be null");

        validateCount(request.stockCount(), request.isFractional());

        Stock stock = stockRepository.findByDevice_Uuid(deviceUuid).orElseGet(() -> {
            Stock s = new Stock();
            s.setDevice(deviceRefByUuid(deviceUuid));
            return s;
        });

        stock.setStockUnitType(request.stockUnitType());
        stock.setStockCount(request.stockCount());
        stock.setFractional(request.isFractional());

        Stock saved = stockRepository.save(stock);
        return toDto(saved);
    }

    @Override
    public StockResponseDTO patchCountByDeviceUuid(UUID deviceUuid, BigDecimal newCount) {
        require(newCount, "newCount must not be null");

        Stock stock = stockRepository.findByDevice_Uuid(deviceUuid)
                .orElseThrow(() -> new EntityNotFoundException("Stock not found for deviceUuid=" + deviceUuid));

        validateCount(newCount, stock.isFractional());

        stock.setStockCount(newCount);
        return toDto(stockRepository.save(stock));
    }

    @Override
    public void deleteByDeviceUuid(UUID deviceUuid) {
        if (!stockRepository.existsByDevice_Uuid(deviceUuid)) {
            throw new EntityNotFoundException("Stock not found for deviceUuid=" + deviceUuid);
        }
        stockRepository.deleteByDevice_Uuid(deviceUuid);
    }

    private Device deviceRefByUuid(UUID deviceUuid) {
        Long deviceId = deviceRepository.findIdByUuid(deviceUuid)
                .orElseThrow(() -> new EntityNotFoundException("Device not found: uuid=" + deviceUuid));
        return deviceRepository.getReferenceById(deviceId);
    }

    private static void validateCount(BigDecimal count, boolean fractionalAllowed) {
        if (count.compareTo(BigDecimal.ZERO) < 0) {
            throw new IllegalArgumentException("stockCount must be >= 0");
        }
        if (!fractionalAllowed) {
            // 1.0 should be allowed, so strip trailing zeros before checking scale.
            if (count.stripTrailingZeros().scale() > 0) {
                throw new IllegalArgumentException("Fractional stockCount not allowed");
            }
        }
    }

    private static StockResponseDTO toDto(Stock stock) {
        return new StockResponseDTO(
                stock.getDevice().getUuid(),
                stock.getUuid(),
                stock.getStockUnitType(),
                stock.getStockCount(),
                stock.isFractional()
        );
    }
}
