package de.pschiessle.lumenforge.device.maintenancestatus;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Transactional
public class MaintenanceStatusService {

    private final MaintenanceStatusRepository repository;

    @Transactional(readOnly = true)
    public Page<MaintenanceStatus> getAll(Pageable pageable) {
        return repository.findAll(pageable);
    }

    @Transactional(readOnly = true)
    public Page<MaintenanceStatus> search(String query, Pageable pageable) {
        if (query == null || query.trim().isEmpty()) {
            return Page.empty(pageable);
        }
        return repository.searchBySimilarity(query.trim(), pageable);
    }

    public MaintenanceStatus create(MaintenanceStatusRequestDTO request) {
        MaintenanceStatus status = new MaintenanceStatus();
        applyRequest(status, request);
        return repository.save(status);
    }

    public MaintenanceStatus update(Long id, MaintenanceStatusRequestDTO request) {
        MaintenanceStatus status = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("MaintenanceStatus not found: id=" + id));
        applyRequest(status, request);
        return repository.save(status);
    }

    public void delete(Long id) {
        MaintenanceStatus status = repository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("MaintenanceStatus not found: id=" + id));
        repository.delete(status);
    }

    @Transactional(readOnly = true)
    public long getCount() {
        return repository.count();
    }

    private void applyRequest(MaintenanceStatus status, MaintenanceStatusRequestDTO request) {
        String name = normalizeRequired(request.name(), "MaintenanceStatus.name");
        status.setName(name);

        status.setDescription(normalizeOptional(request.description()));
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
