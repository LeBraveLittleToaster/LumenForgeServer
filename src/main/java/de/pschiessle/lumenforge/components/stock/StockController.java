package de.pschiessle.lumenforge.components.stock;

import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;
import de.pschiessle.lumenforge.components.stock.response.StockResponseDTO;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.util.UUID;

@RestController
@RequestMapping("/api/stocks")
@RequiredArgsConstructor
public class StockController {

    private final StockServiceImpl stockService;

    @GetMapping("/device/{deviceUuid}")
    public StockResponseDTO getByDeviceUuid(@PathVariable UUID deviceUuid) {
        return stockService.getByDeviceUuid(deviceUuid);
    }

    @PutMapping("/device/{deviceUuid}")
    public StockResponseDTO upsertByDeviceUuid(
            @PathVariable UUID deviceUuid,
            @RequestBody StockRequestDTO request
    ) {
        return stockService.upsertByDeviceUuid(deviceUuid, request);
    }

    public record StockCountPatchDTO(BigDecimal stockCount) {}

    @PatchMapping("/device/{deviceUuid}/count")
    public StockResponseDTO patchCount(
            @PathVariable UUID deviceUuid,
            @RequestBody StockCountPatchDTO request
    ) {
        if (request == null || request.stockCount() == null) {
            throw new IllegalArgumentException("stockCount must not be null");
        }
        return stockService.patchCountByDeviceUuid(deviceUuid, request.stockCount());
    }

    @DeleteMapping("/device/{deviceUuid}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deleteByDeviceUuid(@PathVariable UUID deviceUuid) {
        stockService.deleteByDeviceUuid(deviceUuid);
    }
}
