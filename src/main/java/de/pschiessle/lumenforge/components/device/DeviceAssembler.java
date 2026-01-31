package de.pschiessle.lumenforge.components.device;

import de.pschiessle.lumenforge.components.category.CategoryRepository;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatusRepository;
import de.pschiessle.lumenforge.components.stock.Stock;
import de.pschiessle.lumenforge.components.stock.StockRepository;
import de.pschiessle.lumenforge.components.vendor.VendorRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Component;

import java.util.List;

import static de.pschiessle.lumenforge.ExceptionHelper.require;

@Component
@RequiredArgsConstructor
public class DeviceAssembler {
    private final VendorRepository vendorRepository;
    private final CategoryRepository categoryRepository;
    private final MaintenanceStatusRepository maintenanceStatusRepository;
    private final StockRepository stockRepository;

    public void applyCreate(Device device, Stock stock, DeviceRequestDTO request) {
        require(request.vendorId(), "Device.vendorId must not be null");
        require(request.maintenanceStatusId(), "Device.maintenanceStatusId must not be null");
        require(request.stockRequestDTO(), "Device.createStockDTO is not present");

        device.setSerialNumber(request.serialNumber());
        device.setName(request.name());
        device.setDescription(request.description());
        device.setPhotoUrl(request.photoUrl());
        device.setPurchasePrice(request.purchasePrice());
        device.setPurchaseDate(request.purchaseDate());

        device.setVendor(vendorRepository.getReferenceById(request.vendorId()));
        device.setMaintenanceStatus(maintenanceStatusRepository.getReferenceById(request.maintenanceStatusId()));
        device.setCategories(request.categoryIds() == null ? List.of() : categoryRepository.findAllById(request.categoryIds()));

        stock.setDevice(device);
        stock.setFractional(request.stockRequestDTO().isFractional());
        stock.setStockUnitType(request.stockRequestDTO().stockUnitType());
        stock.setStockCount(request.stockRequestDTO().stockCount());
        stockRepository.save(stock);
    }
}
