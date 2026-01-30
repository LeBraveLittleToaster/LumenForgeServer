package de.pschiessle.lumenforge.config;

import de.pschiessle.lumenforge.device.Device;
import de.pschiessle.lumenforge.device.DeviceRequestDTO;
import de.pschiessle.lumenforge.device.DeviceServiceImpl;
import de.pschiessle.lumenforge.device.IDeviceService;
import de.pschiessle.lumenforge.device.category.Category;
import de.pschiessle.lumenforge.device.category.CategoryRequestDTO;
import de.pschiessle.lumenforge.device.category.CategoryService;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusRequestDTO;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatusService;
import de.pschiessle.lumenforge.device.vendor.Vendor;
import de.pschiessle.lumenforge.device.vendor.VendorRequestDTO;
import de.pschiessle.lumenforge.device.vendor.VendorService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.time.LocalDate;
import java.util.List;
import java.util.Objects;
import java.util.Optional;
import java.util.Random;
import java.util.stream.IntStream;

@Slf4j
@Component
@RequiredArgsConstructor
public class InitializeDatabaseService implements CommandLineRunner {


    public static final String INITIALIZE_DATABASE_STARTERDATA_LONG = "--init-with-starter-data";
    public static final String INITIALIZE_DATABASE_STARTERDATA_SHORT = "-s";

    private static final String CATEGORY_CSV_FILENAME = "/static/categories.csv";
    private static final String MAINTENANCE_STATUS_CSV_FILENAME = "/static/maintenance_status.csv";
    private static final String VENDOR_CSV_FILENAME = "/static/manufacturer.csv";


    private final CategoryService categoryService;
    private final VendorService vendorService;
    private final MaintenanceStatusService maintenanceStatusService;
    private final DeviceServiceImpl deviceService;

    @Override
    public void run(String... args) throws Exception {
        for(int i = 0; i < args.length; i++) {
            if(Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_SHORT) || Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_LONG)) {
                log.info("Found command to generate database starter data, proceeding with categories...");
                var cat = loadCategories();
                log.info("Found command to generate database starter data, proceeding with vendors...");
                var ven = loadVendors();
                log.info("Found command to generate database starter data, proceeding with maintenance status...");
                var mai = loadMaintenanceStatuses();
                log.info("Found command to generate database starter data, proceeding with devices...");
                generateDevices(500, cat, ven, mai);
                return;
            }
        }
        log.info("Found no command to generate database starter data");
    }

    private List<Device> generateDevices(
            int amount,
            List<Category> categories,
            List<Vendor> vendors,
            List<MaintenanceStatus> statuses
    ) {
        if (amount <= 0) return List.of();

        if (vendors == null || vendors.isEmpty()) {
            log.warn("Cannot generate devices: no vendors available.");
            return List.of();
        }
        if (statuses == null || statuses.isEmpty()) {
            log.warn("Cannot generate devices: no maintenance statuses available.");
            return List.of();
        }

        List<Category> safeCategories = (categories == null) ? List.of() : categories;

        var rnd = java.util.concurrent.ThreadLocalRandom.current();

        return IntStream.rangeClosed(1, amount)
                .mapToObj(i -> {
                    Vendor vendor = vendors.get(rnd.nextInt(vendors.size()));
                    MaintenanceStatus status = statuses.get(rnd.nextInt(statuses.size()));

                    // random category subset -> IDs
                    List<Long> categoryIds;
                    if (safeCategories.isEmpty()) {
                        categoryIds = List.of();
                    } else {
                        int max = Math.min(3, safeCategories.size());
                        int take = rnd.nextInt(0, max + 1);
                        if (take == 0) {
                            categoryIds = List.of();
                        } else {
                            var shuffled = new java.util.ArrayList<>(safeCategories);
                            java.util.Collections.shuffle(shuffled);
                            categoryIds = shuffled.subList(0, take).stream()
                                    .map(Category::getId)
                                    .toList();
                        }
                    }

                    String serial = "SN-" + java.util.UUID.randomUUID().toString().substring(0, 12).toUpperCase();

                    var price = BigDecimal.valueOf(rnd.nextLong(1_000, 200_000)).movePointLeft(2);
                    var purchaseDate = LocalDate.now().minusDays(rnd.nextLong(0, 365L * 5));

                    return new DeviceRequestDTO(
                            serial,
                            "Generated Device " + i,
                            "Auto-generated seed data",
                            null,
                            vendor.getId(),
                            price,
                            purchaseDate,
                            categoryIds,
                            status.getId()
                    );
                })
                .map(deviceService::create)
                .toList();
    }



    private List<Category> loadCategories() throws Exception {
        if (categoryService.getCount() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return List.of();
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(CATEGORY_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            return reader.lines()
                    .skip(1) // header
                    .map(String::trim)
                    .filter(line -> !line.isEmpty())
                    .map(line -> line.split(",", 2))
                    .map(values -> new CategoryRequestDTO(
                            values[0].trim(),
                            values.length > 1 ? values[1].trim() : null
                    ))
                    .map(dto -> {
                        try {
                            return categoryService.create(dto);
                        } catch (RuntimeException e) {
                            log.warn("Skipping category row due to error: {}", e.getMessage());
                            return null;
                        }
                    })
                    .filter(java.util.Objects::nonNull)
                    .toList();
        }
    }

    private List<Vendor> loadVendors() throws Exception {
        if (vendorService.getCount() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return List.of();
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(VENDOR_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            return reader.lines()
                    .skip(1) // header
                    .map(String::trim)
                    .filter(line -> !line.isEmpty())
                    .map(name -> new VendorRequestDTO(name))
                    .map(dto -> {
                        try {
                            return vendorService.create(dto);
                        } catch (RuntimeException e) {
                            log.warn("Skipping vendor row due to error: {}", e.getMessage());
                            return null;
                        }
                    })
                    .filter(java.util.Objects::nonNull)
                    .toList();
        }
    }


    private List<MaintenanceStatus> loadMaintenanceStatuses() throws Exception {
        if (maintenanceStatusService.getCount() > 0) {
            log.info("Found command, but one ore more databases are not empty...skipping import");
            return List.of();
        }

        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(MAINTENANCE_STATUS_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            return reader.lines()
                    .skip(1) // header
                    .map(String::trim)
                    .filter(line -> !line.isEmpty())
                    .map(line -> line.split(",", 2))
                    .map(values -> new MaintenanceStatusRequestDTO(
                            values[0].trim(),
                            values.length > 1 ? values[1].trim() : null
                    ))
                    .map(dto -> {
                        try {
                            return maintenanceStatusService.create(dto);
                        } catch (RuntimeException e) {
                            log.warn("Skipping maintenance status row due to error: {}", e.getMessage());
                            return null;
                        }
                    })
                    .filter(java.util.Objects::nonNull)
                    .toList();
        }
    }

}
