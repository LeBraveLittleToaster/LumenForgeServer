package de.pschiessle.lumenforge.config;

import de.pschiessle.lumenforge.device.category.Category;
import de.pschiessle.lumenforge.device.category.CategoryRepository;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusRepository;
import de.pschiessle.lumenforge.device.vendor.Vendor;
import de.pschiessle.lumenforge.device.vendor.VendorRepository;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.Objects;

@Slf4j
@Component
@RequiredArgsConstructor
public class InitializeDatabaseService implements CommandLineRunner {


    public static final String INITIALIZE_DATABASE_STARTERDATA_LONG = "--init-with-starter-data";
    public static final String INITIALIZE_DATABASE_STARTERDATA_SHORT = "-s";

    private static final String CATEGORY_CSV_FILENAME = "/static/categories.csv";
    private static final String MAINTENANCE_STATUS_CSV_FILENAME = "/static/maintenance_status.csv";
    private static final String VENDOR_CSV_FILENAME = "/static/manufacturer.csv";


    private final CategoryRepository categoryRepository;
    private final VendorRepository vendorRepository;
    private final MaintenanceStatusRepository maintenanceStatusRepository;

    @Override
    public void run(String... args) throws Exception {
        for(int i = 0; i < args.length; i++) {
            if(Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_SHORT) || Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_LONG)) {
                log.info("Found command to generate database starter data, proceeding with categories...");
                loadCategories();
                log.info("Found command to generate database starter data, proceeding with vendors...");
                loadVendors();
                log.info("Found command to generate database starter data, proceeding with maintenance status...");
                loadMaintenanceStatuses();
                return;
            }
        }
        log.info("Found no command to generate database starter data");
    }

    private void loadCategories() throws Exception {
        if (categoryRepository.count() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return;
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(CATEGORY_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            reader.lines()
                    .skip(1) // header
                    .map(line -> line.split(",", 2))
                    .forEach(values -> {
                        Category category = new Category();
                        category.setName(values[0].trim());
                        category.setDescription(values.length > 1 ? values[1].trim() : null);
                        categoryRepository.save(category);
                    });
        }
    }

    private void loadVendors() throws Exception {
        if (vendorRepository.count() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return;
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(VENDOR_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            reader.lines()
                    .skip(1) // skip header
                    .map(String::trim)
                    .filter(line -> !line.isEmpty())
                    .forEach(name -> {
                        Vendor vendor = new Vendor();
                        vendor.setName(name);
                        vendorRepository.save(vendor);
                    });
        }
    }

    private void loadMaintenanceStatuses() throws Exception {
        if (maintenanceStatusRepository.count() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return;
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(MAINTENANCE_STATUS_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            reader.lines()
                    .skip(1) // header
                    .map(line -> line.split(",", 2))
                    .forEach(values -> {
                        MaintenanceStatus status = new MaintenanceStatus();
                        status.setName(values[0].trim());
                        status.setDescription(values.length > 1 ? values[1].trim() : null);
                        maintenanceStatusRepository.save(status);
                    });
        }
    }
}
