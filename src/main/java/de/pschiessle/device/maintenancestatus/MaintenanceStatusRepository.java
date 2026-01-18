package de.pschiessle.device.maintenancestatus;

import org.springframework.data.jpa.repository.JpaRepository;

public interface MaintenanceStatusRepository extends JpaRepository<MaintenanceStatus, Long> {}
