package de.pschiessle.lumenforge.device;

import de.pschiessle.lumenforge.device.category.CategoryRepository;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusRepository;
import de.pschiessle.lumenforge.device.vendor.Vendor;
import de.pschiessle.lumenforge.device.vendor.VendorRepository;
import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.UUID;

@Service
@RequiredArgsConstructor
@Transactional
public class DeviceServiceImpl implements IDeviceService {

    private final DeviceRepository deviceRepository;
    private final VendorRepository vendorRepository;
    private final CategoryRepository categoryRepository;
    private final MaintenanceStatusRepository maintenanceStatusRepository;

    @Override
    @Transactional(readOnly = true)
    public Page<Device> getPage(Pageable pageable) {
        return deviceRepository.findAll(pageable);
    }

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
        applyRequest(device, request);
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

    private void applyRequest(Device device, DeviceRequestDTO request) {
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

        if (request.categoryIds() != null) {
            device.setCategories(categoryRepository.findAllById(request.categoryIds()));
        } else {
            device.setCategories(List.of());
        }
    }
}
