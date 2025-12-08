package de.pschiessle.artnet.sender.devices;

import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface DeviceRepository extends CrudRepository<DeviceDTO, Long> {
    Optional<DeviceDTO> findDeviceDTOByUuid(String uuid);
    Optional<List<DeviceDTO>> findDeviceDTOSByIsActive(Boolean isActive);
    Optional<List<DeviceDTO>> findDeviceDTOByIsDmxOTAV1Compatible(Boolean isDmxOTAV1Compatible);
}
