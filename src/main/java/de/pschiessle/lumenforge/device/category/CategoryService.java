package de.pschiessle.lumenforge.device.category;

import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Transactional(readOnly = true)
public class CategoryService {

    private final CategoryRepository repository;

    public Page<Category> getAll(Pageable pageable) {
        return repository.findAll(pageable);
    }

    public Page<Category> search(String query, Pageable pageable) {
        if (query == null || query.trim().isEmpty()) {
            return Page.empty(pageable);
        }
        return repository.searchBySimilarity(query.trim(), pageable);
    }
}
