package de.pschiessle.lumenforge.device.vendor;

import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Transactional(readOnly = true)
public class VendorService {

    private final VendorRepository vendorRepository;

    public Page<Vendor> getVendors(Pageable pageable) {
        return vendorRepository.findAll(pageable);
    }

    public Page<Vendor> searchVendorsBySimilarity(String query, Pageable pageable) {
        if (query == null) {
            return Page.empty(pageable);
        }
        String q = query.trim();
        if (q.isEmpty()) {
            return Page.empty(pageable);
        }
        return vendorRepository.searchByNameSimilarity(q, pageable);
    }
}