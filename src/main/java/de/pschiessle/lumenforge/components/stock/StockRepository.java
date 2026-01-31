package de.pschiessle.lumenforge.components.stock;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.Optional;
import java.util.UUID;

public interface StockRepository extends JpaRepository<Stock, Long> {
    Optional<Stock> findByDevice_Uuid(UUID deviceUuid);

    boolean existsByDevice_Uuid(UUID deviceUuid);

    void deleteByDevice_Uuid(UUID deviceUuid);

    @Query("select s.id from Stock s where s.device.uuid = :deviceUuid")
    Optional<Long> findIdByDeviceUuid(UUID deviceUuid);
}
