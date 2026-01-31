package de.pschiessle.lumenforge.device.vendor;

import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/vendors")
public class VendorController {

    private final VendorService vendorService;

    // GET /api/vendors?page=0&size=20&sort=name,asc
    @GetMapping
    public ResponseEntity<Page<Vendor>> getVendors(
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(vendorService.getAll(pageable));
    }

    // GET /api/vendors/search?query=acm&page=0&size=20
    @GetMapping("/search")
    public ResponseEntity<Page<Vendor>> searchVendors(
            @RequestParam("query") String query,
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(vendorService.search(query, pageable));
    }
}
