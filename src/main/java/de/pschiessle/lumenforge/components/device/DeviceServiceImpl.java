package de.pschiessle.lumenforge.components.device;

import de.pschiessle.lumenforge.components.category.CategoryRepository;
import de.pschiessle.lumenforge.components.device.dto.DeviceListDTO;
import de.pschiessle.lumenforge.components.device.dto.DeviceListWithStockDTO;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatusRepository;
import de.pschiessle.lumenforge.components.stock.Stock;
import de.pschiessle.lumenforge.components.stock.StockRepository;
import de.pschiessle.lumenforge.components.vendor.Vendor;
import de.pschiessle.lumenforge.components.vendor.VendorRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.UUID;

@Primary
@Service
@RequiredArgsConstructor
@Transactional
public class DeviceServiceImpl implements IDeviceService {

    private final DeviceRepository deviceRepository;
    private final VendorRepository vendorRepository;
    private final CategoryRepository categoryRepository;
    private final MaintenanceStatusRepository maintenanceStatusRepository;
    private final StockRepository stockRepository;
    private final DeviceAssembler deviceAssembler;

    @Override
    @Transactional(readOnly = true)
    public List<Device> getAll() {
        return deviceRepository.findAll();
    }

    @Override
    @Transactional(readOnly = true)
    public Device getById(Long id) {
        return deviceRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Device not found: id=" + id));
    }

    @Override
    @Transactional(readOnly = true)
    public Device getByUuid(UUID uuid) {
        return deviceRepository.findByUuid(uuid)
                .orElseThrow(() -> new EntityNotFoundException("Device not found: uuid=" + uuid));
    }

    @Override
    public Device create(DeviceRequestDTO request) {
        Device device = new Device();
        Stock stock = new Stock();
        deviceAssembler.applyCreate(device, stock, request);
        return deviceRepository.save(device);
    }

    @Override
    public Device update(Long id, DeviceRequestDTO request) {
        Device device = getById(id);
        applyRequest(device, request);
        return deviceRepository.save(device);
    }

    @Override
    public void delete(Long id) {
        Device device = getById(id);
        deviceRepository.delete(device);
    }

    @Transactional(readOnly = true)
    @Override
    public Page<DeviceListWithStockDTO> getPageWithStock(Pageable pageable, String q) {
        String needle = (q == null) ? "" : q.trim();
        return deviceRepository.findPageWithStock(needle, pageable);
    }


    @Override
    @Transactional(readOnly = true)
    public Page<DeviceListDTO> getPage(Pageable pageable, String q) {
        String needle = (q == null) ? "" : q.trim();
        return deviceRepository.findPageAsListDTO(needle, pageable);
    }


    private void applyRequest(Device device, DeviceRequestDTO request) {
        if (request.vendorId() == null) {
            throw new IllegalArgumentException("Device.vendorId must not be null");
        }
        if (request.maintenanceStatusId() == null) {
            throw new IllegalArgumentException("Device.maintenanceStatusId must not be null");
        }

        device.setSerialNumber(request.serialNumber());
        device.setName(request.name());
        device.setDescription(request.description());
        device.setPhotoUrl(request.photoUrl());
        device.setPurchasePrice(request.purchasePrice());
        device.setPurchaseDate(request.purchaseDate());

        Vendor vendor = vendorRepository.findById(request.vendorId())
                .orElseThrow(() -> new EntityNotFoundException("Vendor not found: id=" + request.vendorId()));
        device.setVendor(vendor);

        MaintenanceStatus ms = maintenanceStatusRepository.findById(request.maintenanceStatusId())
                .orElseThrow(() -> new EntityNotFoundException("MaintenanceStatus not found: id=" + request.maintenanceStatusId()));
        device.setMaintenanceStatus(ms);

        device.setCategories(
                request.categoryIds() != null ? categoryRepository.findAllById(request.categoryIds()) : List.of()
        );
    }

}
