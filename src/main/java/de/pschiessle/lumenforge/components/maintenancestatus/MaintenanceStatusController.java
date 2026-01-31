package de.pschiessle.lumenforge.components.maintenancestatus;

import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/maintenance-statuses")
public class MaintenanceStatusController {

    private final MaintenanceStatusService service;

    // GET /api/maintenance-statuses?page=0&size=20&sort=name,asc
    @GetMapping
    public ResponseEntity<Page<MaintenanceStatus>> getAll(
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(service.getAll(pageable));
    }

    // GET /api/maintenance-statuses/search?query=maint
    @GetMapping("/search")
    public ResponseEntity<Page<MaintenanceStatus>> search(
            @RequestParam String query,
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(service.search(query, pageable));
    }
}
