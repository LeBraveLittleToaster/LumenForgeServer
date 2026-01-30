package de.pschiessle.lumenforge.device;

import de.pschiessle.lumenforge.SenderApplication;
import de.pschiessle.lumenforge.device.category.CategoryRepository;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusRepository;
import de.pschiessle.lumenforge.device.vendor.Vendor;
import de.pschiessle.lumenforge.device.vendor.VendorRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.test.context.ContextConfiguration;

import java.util.List;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
@ContextConfiguration(classes = SenderApplication.class)
class DeviceRepositoryTest {

    @Autowired
    private DeviceRepository deviceRepository;

    @Autowired
    private VendorRepository vendorRepository;

    @Autowired
    private MaintenanceStatusRepository maintenanceStatusRepository;

    @Autowired
    private CategoryRepository categoryRepository;

    @BeforeEach
    void clearDatabase() {
        deviceRepository.deleteAll();
        categoryRepository.deleteAll();
        maintenanceStatusRepository.deleteAll();
        vendorRepository.deleteAll();
    }

    @Test
    void findByUuidAndExistsByUuidReturnExpectedResults() {
        Vendor vendor = vendorRepository.save(buildVendor("Acme"));
        MaintenanceStatus status = maintenanceStatusRepository.save(buildStatus("Operational"));
        Device device = deviceRepository.save(new DeviceBuilder(vendor, status).build("Laser Cutter", "SN-001"));

        assertThat(deviceRepository.findByUuid(device.getUuid()))
                .contains(device);
        assertThat(deviceRepository.existsByUuid(device.getUuid()))
                .isTrue();
        assertThat(deviceRepository.findByUuid(UUID.randomUUID()))
                .isEmpty();
        assertThat(deviceRepository.existsByUuid(UUID.randomUUID()))
                .isFalse();
    }

    @Test
    void findBySerialNumberAndExistsBySerialNumberReturnExpectedResults() {
        Vendor vendor = vendorRepository.save(buildVendor("Omega"));
        MaintenanceStatus status = maintenanceStatusRepository.save(buildStatus("Serviced"));
        Device device = deviceRepository.save(new DeviceBuilder(vendor, status).build("Printer", "SER-777"));

        assertThat(deviceRepository.findBySerialNumber("SER-777"))
                .contains(device);
        assertThat(deviceRepository.existsBySerialNumber("SER-777"))
                .isTrue();
        assertThat(deviceRepository.findBySerialNumber("missing"))
                .isEmpty();
        assertThat(deviceRepository.existsBySerialNumber("missing"))
                .isFalse();
    }

    @Test
    void searchByNameOrSerialNumberHandlesCombinedMatchesAndEdgeCases() {
        Vendor vendor = vendorRepository.save(buildVendor("Nova"));
        MaintenanceStatus status = maintenanceStatusRepository.save(buildStatus("Online"));

        DeviceBuilder builder = new DeviceBuilder(vendor, status);

        Device laser = deviceRepository.save(builder.build("Laser Cutter", "SN-001"));
        Device printer = deviceRepository.save(builder.build("Printer", "LC-777"));
        Device scanner = deviceRepository.save(builder.build("Scanner", "SN-XYZ"));
        Device press = deviceRepository.save(builder.build("Press", "PR-500"));

        Page<Device> byName = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("laser", "laser", PageRequest.of(0, 10, Sort.by(Sort.Direction.ASC,"id")));
        assertThat(byName.getContent())
                .containsExactly(laser);

        Page<Device> bySerial = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("lc", "lc", PageRequest.of(0, 10, Sort.by(Sort.Direction.ASC,"id")));
        assertThat(bySerial.getContent())
                .contains(printer);

        Page<Device> combined = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("sn", "sn", PageRequest.of(0, 10, Sort.by(Sort.Direction.ASC,"id")));
        assertThat(combined.getContent())
                .containsExactlyInAnyOrder(laser, scanner);

        Page<Device> bySerialPrefix = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("pr", "pr", PageRequest.of(0, 10, Sort.by(Sort.Direction.ASC,"id")));
        assertThat(bySerialPrefix.getContent())
                .containsExactly(printer,press);

        Page<Device> empty = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("missing", "missing", PageRequest.of(0, 10, Sort.by(Sort.Direction.ASC,"id")));
        assertThat(empty.getContent())
                .isEmpty();
    }

    @Test
    void searchByNameOrSerialNumberPaginatesAcrossLargeResultSet() {
        Vendor vendor = vendorRepository.save(buildVendor("Atlas"));
        MaintenanceStatus status = maintenanceStatusRepository.save(buildStatus("Available"));
        DeviceBuilder builder = new DeviceBuilder(vendor, status);

        for (int i = 1; i <= 12; i++) {
            deviceRepository.save(builder.build("Laser Rig " + i, "LR-" + i));
        }
        deviceRepository.save(builder.build("Controller", "LAS-CTRL"));
        deviceRepository.save(builder.build("Scanner", "SCAN-100"));

        Page<Device> firstPage = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("las", "las", PageRequest.of(0, 5));
        Page<Device> secondPage = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("las", "las", PageRequest.of(1, 5));
        Page<Device> thirdPage = deviceRepository
                .findByNameContainingIgnoreCaseOrSerialNumberContainingIgnoreCase("las", "las", PageRequest.of(2, 5));

        assertThat(firstPage.getContent()).hasSize(5);
        assertThat(secondPage.getContent()).hasSize(5);
        assertThat(thirdPage.getContent()).hasSize(3);
        assertThat(firstPage.getTotalElements()).isEqualTo(13);
        assertThat(thirdPage.getContent())
                .allMatch(device -> device.getName().toLowerCase().contains("las")
                        || device.getSerialNumber().toLowerCase().contains("las"));
    }

    private Vendor buildVendor(String name) {
        Vendor vendor = new Vendor();
        vendor.setName(name);
        return vendor;
    }

    private MaintenanceStatus buildStatus(String name) {
        MaintenanceStatus status = new MaintenanceStatus();
        status.setName(name);
        return status;
    }

    private record DeviceBuilder(Vendor vendor, MaintenanceStatus status) {

        private Device build(String name, String serialNumber) {
                Device device = new Device();
                device.setName(name);
                device.setSerialNumber(serialNumber);
                device.setVendor(vendor);
                device.setMaintenanceStatus(status);
                device.setCategories(List.of());
                return device;
            }
        }
}