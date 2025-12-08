package de.pschiessle.artnet.sender.devices;

import de.pschiessle.artnet.sender.utils.TimeUtil;
import jakarta.annotation.PostConstruct;
import lombok.extern.slf4j.Slf4j;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@Transactional
@Slf4j
public class DeviceService {

    private final DeviceRepository deviceRepository;

    public DeviceService(DeviceRepository deviceRepository) {
        this.deviceRepository = deviceRepository;
    }


    public List<DeviceDTO> getAllDevices() {
        List<DeviceDTO> devices = new ArrayList<>();
        deviceRepository.findAll().forEach(devices::add);
        return devices;
    }

    public Optional<DeviceDTO> getDeviceById(Long id) {
        return deviceRepository.findById(id);
    }

    public Optional<DeviceDTO> getDeviceByUuid(String uuid) {
        return deviceRepository.findDeviceDTOByUuid(uuid);
    }

    public Optional<List<DeviceDTO>> getDeviceByIsDmxOTAV1Compatible(Boolean isDmxOTAV1Compatible) {
        return deviceRepository.findDeviceDTOByIsDmxOTAV1Compatible(isDmxOTAV1Compatible);
    }

    @Cacheable(value = "active_devices_cache")
    public List<DeviceDTO> getDevicesByIsActive(Boolean isActive) {
        return deviceRepository.findDeviceDTOSByIsActive(isActive).orElseGet(ArrayList::new);
    }

    public DeviceDTO createDevice(DeviceDTO deviceDTO) {
        LocalDateTime now = TimeUtil.getCurrentUtc();
        deviceDTO.setId(null);
        deviceDTO.setUuid(UUID.randomUUID().toString());
        deviceDTO.setCreatedAt(now);
        deviceDTO.setUpdatedAt(now);
        return deviceRepository.save(deviceDTO);
    }

    public Optional<DeviceDTO> updateDevice(Long id, DeviceDTO deviceDTO) {
        return deviceRepository.findById(id).map(existing -> {
            if(deviceDTO.getUuid() != null) existing.setUuid(deviceDTO.getUuid());
            if(deviceDTO.getIsActive() != null) existing.setIsActive(deviceDTO.getIsActive());
            if(deviceDTO.getName() != null) existing.setName(deviceDTO.getName());
            if(deviceDTO.getArtnetUrl() != null) existing.setArtnetUrl(deviceDTO.getArtnetUrl());
            if(deviceDTO.getArtnetPort() != null) existing.setArtnetPort(deviceDTO.getArtnetPort());
            if(deviceDTO.getArtnetSubnet() != null) existing.setArtnetSubnet(deviceDTO.getArtnetSubnet());
            if(deviceDTO.getArtnetUniverse() != null) existing.setArtnetUniverse(deviceDTO.getArtnetUniverse());
            existing.setUpdatedAt(TimeUtil.getCurrentUtc());
            return deviceRepository.save(existing);
        });
    }

    public void deleteDevice(Long id) {
        deviceRepository.deleteById(id);
    }

    @PostConstruct
    public void populateDatabase() {
        log.info("Populating database...");

        DeviceDTO localDeviceDTO = new DeviceDTO();
        localDeviceDTO.setName("My Local Device");
        localDeviceDTO.setIsActive(true);
        localDeviceDTO.setIsDmxOTAV1Compatible(true);
        localDeviceDTO.setArtnetUrl("esp32.local");
        localDeviceDTO.setArtnetUniverse(0);
        localDeviceDTO.setArtnetPort(6454);
        localDeviceDTO.setArtnetSubnet(0);
        createDevice(localDeviceDTO);

        for(int i = 0; i < 20; i++) {
            DeviceDTO deviceDTO = new DeviceDTO();
            deviceDTO.setName("Test Device" + i);
            deviceDTO.setIsActive(false);
            deviceDTO.setIsDmxOTAV1Compatible(true);
            deviceDTO.setArtnetUrl("localhost");
            deviceDTO.setArtnetUniverse(0);
            deviceDTO.setArtnetPort(6454);
            deviceDTO.setArtnetSubnet(0);
            createDevice(deviceDTO);
        }

        log.info("Populated database complete.");
    }
}
