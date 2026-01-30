package de.pschiessle.lumenforge.device.maintenancestatus;

import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import de.pschiessle.lumenforge.SenderApplication;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.test.context.ContextConfiguration;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
@ContextConfiguration(classes = SenderApplication.class)
class MaintenanceStatusRepositoryTest {

    @Autowired
    private MaintenanceStatusRepository maintenanceStatusRepository;

    @BeforeEach
    void clearDatabase() {
        maintenanceStatusRepository.deleteAll();
    }

    @Test
    void searchBySimilarityMatchesNameAndDescriptionAndOrdersByPrefix() {
        MaintenanceStatus online = new MaintenanceStatus();
        online.setName("Operational");
        online.setDescription("Operational and ready");
        maintenanceStatusRepository.save(online);

        MaintenanceStatus operating = new MaintenanceStatus();
        operating.setName("Operation Halt");
        operating.setDescription("Paused for service");
        maintenanceStatusRepository.save(operating);

        MaintenanceStatus service = new MaintenanceStatus();
        service.setName("Service Needed");
        service.setDescription("Needs service");
        maintenanceStatusRepository.save(service);

        MaintenanceStatus spare = new MaintenanceStatus();
        spare.setName("Spare");
        spare.setDescription("Stored in inventory");
        maintenanceStatusRepository.save(spare);

        Page<MaintenanceStatus> results = maintenanceStatusRepository.searchBySimilarity("oper", PageRequest.of(0, 10));
        assertThat(results.getContent())
                .containsExactly(operating, online);
    }

    @Test
    void searchBySimilarityReturnsEmptyForNoMatches() {
        Page<MaintenanceStatus> results = maintenanceStatusRepository.searchBySimilarity("missing", PageRequest.of(0, 10));
        assertThat(results.getContent()).isEmpty();
    }

    @Test
    void searchBySimilaritySupportsPaginationForMixedMatches() {
        for (int i = 1; i <= 9; i++) {
            MaintenanceStatus status = new MaintenanceStatus();
            status.setName("Service Stage " + i);
            status.setDescription("Service workflow step " + i);
            maintenanceStatusRepository.save(status);
        }

        MaintenanceStatus descriptionOnly = new MaintenanceStatus();
        descriptionOnly.setName("Operational");
        descriptionOnly.setDescription("service planning only");
        maintenanceStatusRepository.save(descriptionOnly);

        Page<MaintenanceStatus> firstPage = maintenanceStatusRepository.searchBySimilarity("service", PageRequest.of(0, 4));
        Page<MaintenanceStatus> secondPage = maintenanceStatusRepository.searchBySimilarity("service", PageRequest.of(1, 4));
        Page<MaintenanceStatus> thirdPage = maintenanceStatusRepository.searchBySimilarity("service", PageRequest.of(2, 4));

        assertThat(firstPage.getContent()).hasSize(4);
        assertThat(secondPage.getContent()).hasSize(4);
        assertThat(thirdPage.getContent()).hasSize(2);
        assertThat(firstPage.getTotalElements()).isEqualTo(10);
        assertThat(thirdPage.getContent())
                .allMatch(status -> status.getName().toLowerCase().contains("service")
                        || status.getDescription().toLowerCase().contains("service"));
    }
}