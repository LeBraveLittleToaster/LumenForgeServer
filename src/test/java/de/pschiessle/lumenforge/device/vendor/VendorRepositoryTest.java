package de.pschiessle.lumenforge.device.vendor;

import de.pschiessle.lumenforge.components.vendor.Vendor;
import de.pschiessle.lumenforge.components.vendor.VendorRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import de.pschiessle.lumenforge.LumenForgeApplication;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.test.context.ContextConfiguration;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
@ContextConfiguration(classes = LumenForgeApplication.class)
class VendorRepositoryTest {

    @Autowired
    private VendorRepository vendorRepository;

    @BeforeEach
    void clearDatabase() {
        vendorRepository.deleteAll();
    }

    @Test
    void searchByNameSimilarityOrdersByPrefixThenLength() {
        Vendor prefixMatch = new Vendor();
        prefixMatch.setName("Acme");
        vendorRepository.save(prefixMatch);

        Vendor prefixLonger = new Vendor();
        prefixLonger.setName("Acme Tools");
        vendorRepository.save(prefixLonger);

        Vendor containsMatch = new Vendor();
        containsMatch.setName("Mega Acme Systems");
        vendorRepository.save(containsMatch);

        Vendor shorterPrefix = new Vendor();
        shorterPrefix.setName("Ac");
        vendorRepository.save(shorterPrefix);

        Page<Vendor> results = vendorRepository.searchByNameSimilarity("ac", PageRequest.of(0, 10));
        assertThat(results.getContent())
                .containsExactly(shorterPrefix, prefixMatch, prefixLonger, containsMatch);
    }

    @Test
    void searchByNameSimilarityReturnsEmptyWhenNoResults() {
        Page<Vendor> results = vendorRepository.searchByNameSimilarity("missing", PageRequest.of(0, 10));
        assertThat(results.getContent()).isEmpty();
    }

    @Test
    void searchByNameSimilarityPaginatesAcrossLargeResultSet() {
        for (int i = 1; i <= 12; i++) {
            Vendor vendor = new Vendor();
            vendor.setName("Acme Supplier " + i);
            vendorRepository.save(vendor);
        }

        Vendor contains = new Vendor();
        contains.setName("Mega Acme Corporation");
        vendorRepository.save(contains);

        Page<Vendor> firstPage = vendorRepository.searchByNameSimilarity("acme", PageRequest.of(0, 5));
        Page<Vendor> secondPage = vendorRepository.searchByNameSimilarity("acme", PageRequest.of(1, 5));
        Page<Vendor> thirdPage = vendorRepository.searchByNameSimilarity("acme", PageRequest.of(2, 5));

        assertThat(firstPage.getContent()).hasSize(5);
        assertThat(secondPage.getContent()).hasSize(5);
        assertThat(thirdPage.getContent()).hasSize(3);
        assertThat(firstPage.getTotalElements()).isEqualTo(13);
        assertThat(thirdPage.getContent())
                .allMatch(vendor -> vendor.getName().toLowerCase().contains("acme"));
    }
}