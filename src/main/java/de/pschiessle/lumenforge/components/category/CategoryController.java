package de.pschiessle.lumenforge.components.category;

import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/categories")
public class CategoryController {

    private final CategoryService service;

    // GET /api/categories?page=0&size=20&sort=name,asc
    @GetMapping
    public ResponseEntity<Page<Category>> getAll(
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(service.getAll(pageable));
    }

    // GET /api/categories/search?query=network
    @GetMapping("/search")
    public ResponseEntity<Page<Category>> search(
            @RequestParam String query,
            @PageableDefault(size = 20) Pageable pageable
    ) {
        return ResponseEntity.ok(service.search(query, pageable));
    }
}
