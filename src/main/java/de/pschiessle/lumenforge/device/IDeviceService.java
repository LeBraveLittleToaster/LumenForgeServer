package de.pschiessle.lumenforge.device;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.List;
import java.util.UUID;

public interface IDeviceService {
    Page<Device> getPage(Pageable pageable);

    List<Device> getAll();
    Device getById(Long id);
    Device getByUuid(UUID uuid);

    Device create(DeviceRequestDTO request);
    Device update(Long id, DeviceRequestDTO request);
    public Page<Device> getPage(Pageable pageable, String q);

    void delete(Long id);
}
