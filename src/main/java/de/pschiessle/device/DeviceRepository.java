package de.pschiessle.device;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface DeviceRepository extends JpaRepository<Device, Long> {
    Optional<Device> findByUuid(UUID uuid);
    Optional<Device> findBySerialNumber(String serialNumber);

    boolean existsByUuid(UUID uuid);
    boolean existsBySerialNumber(String serialNumber);

}