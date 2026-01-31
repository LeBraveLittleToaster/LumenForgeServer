package de.pschiessle.lumenforge.device.vendor;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Transactional
public class VendorService {

    private final VendorRepository repository;

    @Transactional(readOnly = true)
    public Page<Vendor> getAll(Pageable pageable) {
        return repository.findAll(pageable);
    }

    @Transactional(readOnly = true)
    public Page<Vendor> search(String query, Pageable pageable) {
        if (query == null || query.trim().isEmpty()) {
            return Page.empty(pageable);
        }
        return repository.searchByNameSimilarity(query.trim(), pageable);
    }

    public Vendor create(VendorRequestDTO request) {
        Vendor vendor = new Vendor();
        applyRequest(vendor, request);
        return repository.save(vendor);
    }

    public Vendor update(Long id, VendorRequestDTO request) {
        Vendor vendor = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Vendor not found: id=" + id));
        applyRequest(vendor, request);
        return repository.save(vendor);
    }

    public void delete(Long id) {
        Vendor vendor = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Vendor not found: id=" + id));
        repository.delete(vendor);
    }

    @Transactional(readOnly = true)
    public long getCount() {
        return repository.count();
    }

    private void applyRequest(Vendor vendor, VendorRequestDTO request) {
        String name = normalizeRequired(request.name(), "Vendor.name");
        vendor.setName(name);
    }

    private String normalizeRequired(String value, String field) {
        if (value == null || value.trim().isEmpty()) {
            throw new IllegalArgumentException(field + " must not be blank");
        }
        return value.trim();
    }
}
