package de.pschiessle.lumenforge.device;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

public interface DeviceRepository extends JpaRepository<Device, Long> {
    Page<Device> findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase(
            String name,
            String serialNumber,
            Pageable pageable
    );

    Optional<Device> findByUuid(UUID uuid);
    Optional<Device> findBySerialNumber(String serialNumber);

    boolean existsByUuid(UUID uuid);
    boolean existsBySerialNumber(String serialNumber);

}