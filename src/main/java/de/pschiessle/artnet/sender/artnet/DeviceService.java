package de.pschiessle.artnet.sender.artnet;

import de.pschiessle.artnet.sender.utils.TimeUtil;
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
            existing.setUuid(deviceDTO.getUuid());
            existing.setIsActive(deviceDTO.getIsActive());
            existing.setName(deviceDTO.getName());
            existing.setArtnetUrl(deviceDTO.getArtnetUrl());
            existing.setArtnetPort(deviceDTO.getArtnetPort());
            existing.setArtnetSubnet(deviceDTO.getArtnetSubnet());
            existing.setArtnetUniverse(deviceDTO.getArtnetUniverse());
            existing.setUpdatedAt(TimeUtil.getCurrentUtc());
            return deviceRepository.save(existing);
        });
    }

    public void deleteDevice(Long id) {
        deviceRepository.deleteById(id);
    }
}
