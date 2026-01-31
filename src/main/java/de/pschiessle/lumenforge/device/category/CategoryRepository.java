package de.pschiessle.lumenforge.device.category;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface CategoryRepository extends JpaRepository<Category, Long> {

    @Query("""
        select c from Category c
        where
          lower(c.name) like lower(concat('%', :q, '%'))
          or lower(c.description) like lower(concat('%', :q, '%'))
        order by
          case when lower(c.name) like lower(concat(:q, '%')) then 0 else 1 end,
          c.name asc
    """)
    Page<Category> searchBySimilarity(
            @Param("q") String query,
            Pageable pageable
    );
}
