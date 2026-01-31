package de.pschiessle.lumenforge.components.device;

import de.pschiessle.lumenforge.components.device.dto.DeviceListDTO;
import de.pschiessle.lumenforge.components.device.dto.DeviceListWithStockDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.Optional;
import java.util.UUID;

public interface DeviceRepository extends JpaRepository<Device, Long> {
    @Query("""
        select new de.pschiessle.lumenforge.components.device.dto.DeviceListWithStockDTO(
            d.uuid, d.name, d.serialNumber,
            s.stockCount, s.stockUnitType,d.vendor.name as vendorName
        )
        from Device d
        left join Stock s on s.device.id = d.id
        where (:q is null or :q = '' 
               or lower(d.name) like lower(concat('%', :q, '%'))
               or lower(d.serialNumber) like lower(concat('%', :q, '%')))
        """)
    Page<DeviceListWithStockDTO> findPageWithStock(@Param("q") String q, Pageable pageable);

    @Query("""
    select new de.pschiessle.lumenforge.components.device.dto.DeviceListDTO(
        d.uuid, d.name, d.serialNumber
    )
    from Device d
    where (:q is null or :q = ''
           or lower(d.name) like lower(concat('%', :q, '%'))
           or lower(d.serialNumber) like lower(concat('%', :q, '%')))
    """)
    Page<DeviceListDTO> findPageAsListDTO(@Param("q") String q, Pageable pageable);

    Page<Device> findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase(
            String name,
            String serialNumber,
            Pageable pageable
    );

    Optional<Device> findByUuid(UUID uuid);
    Optional<Device> findBySerialNumber(String serialNumber);

    @Query("select d.id from Device d where d.uuid = :uuid")
    Optional<Long> findIdByUuid(UUID uuid);

    boolean existsByUuid(UUID uuid);
    boolean existsBySerialNumber(String serialNumber);

}