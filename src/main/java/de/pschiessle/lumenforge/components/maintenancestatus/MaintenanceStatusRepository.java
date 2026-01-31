package de.pschiessle.lumenforge.components.maintenancestatus;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface MaintenanceStatusRepository
        extends JpaRepository<MaintenanceStatus, Long> {

    @Query("""
        select m from MaintenanceStatus m
        where
          lower(m.name) like lower(concat('%', :q, '%'))
          or lower(m.description) like lower(concat('%', :q, '%'))
        order by
          case when lower(m.name) like lower(concat(:q, '%')) then 0 else 1 end,
          m.name asc
    """)
    Page<MaintenanceStatus> searchBySimilarity(
            @Param("q") String query,
            Pageable pageable
    );
}
