package de.pschiessle.lumenforge.components.device;

import de.pschiessle.lumenforge.components.device.dto.DeviceListDTO;
import de.pschiessle.lumenforge.components.device.dto.DeviceListWithStockDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.UUID;

public interface IDeviceService {


    List<Device> getAll();
    Device getById(Long id);
    Device getByUuid(UUID uuid);

    Device create(DeviceRequestDTO request);
    Device update(Long id, DeviceRequestDTO request);

    @Transactional(readOnly = true)
    Page<DeviceListWithStockDTO> getPageWithStock(Pageable pageable, String q);

    Page<DeviceListDTO> getPage(Pageable pageable, String q);

    void delete(Long id);
}
