package de.pschiessle.lumenforge.components.category;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Transactional
public class CategoryService {

    private final CategoryRepository repository;

    @Transactional(readOnly = true)
    public Page<Category> getAll(Pageable pageable) {
        return repository.findAll(pageable);
    }

    @Transactional(readOnly = true)
    public Page<Category> search(String query, Pageable pageable) {
        if (query == null || query.trim().isEmpty()) {
            return Page.empty(pageable);
        }
        return repository.searchBySimilarity(query.trim(), pageable);
    }

    public Category create(CategoryRequestDTO request) {
        Category category = new Category();
        applyRequest(category, request);
        return repository.save(category);
    }

    public Category update(Long id, CategoryRequestDTO request) {
        Category category = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Category not found: id=" + id));
        applyRequest(category, request);
        return repository.save(category);
    }

    public void delete(Long id) {
        Category category = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Category not found: id=" + id));
        repository.delete(category);
    }

    @Transactional(readOnly = true)
    public long getCount() {
        return repository.count();
    }

    private void applyRequest(Category category, CategoryRequestDTO request) {
        String name = normalizeRequired(request.name(), "Category.name");
        category.setName(name);

        category.setDescription(normalizeOptional(request.description()));
    }

    private String normalizeRequired(String value, String field) {
        if (value == null || value.trim().isEmpty()) {
            throw new IllegalArgumentException(field + " must not be blank");
        }
        return value.trim();
    }

    private String normalizeOptional(String value) {
        if (value == null) return null;
        String t = value.trim();
        return t.isEmpty() ? null : t;
    }
}
