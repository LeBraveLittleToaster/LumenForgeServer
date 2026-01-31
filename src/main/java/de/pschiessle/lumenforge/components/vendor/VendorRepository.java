package de.pschiessle.lumenforge.components.vendor;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface VendorRepository extends JpaRepository<Vendor, Long> {
    // Simple similarity search using case-insensitive LIKE.
    // Orders vendors that start with the query higher than those that merely contain it.
    @Query("""
        select v from Vendor v
        where lower(v.name) like lower(concat('%', :q, '%'))
        order by
          case when lower(v.name) like lower(concat(:q, '%')) then 0 else 1 end,
          length(v.name) asc,
          v.name asc
    """)
    Page<Vendor> searchByNameSimilarity(@Param("q") String q, Pageable pageable);
}

