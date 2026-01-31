package de.pschiessle.lumenforge.config;

import de.pschiessle.lumenforge.components.category.Category;
import de.pschiessle.lumenforge.components.category.CategoryRequestDTO;
import de.pschiessle.lumenforge.components.category.CategoryService;
import de.pschiessle.lumenforge.components.device.Device;
import de.pschiessle.lumenforge.components.device.DeviceRequestDTO;
import de.pschiessle.lumenforge.components.device.DeviceServiceImpl;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatusRequestDTO;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatusService;
import de.pschiessle.lumenforge.components.stock.StockServiceImpl;
import de.pschiessle.lumenforge.components.stock.StockUnitType;
import de.pschiessle.lumenforge.components.stock.request.StockRequestDTO;
import de.pschiessle.lumenforge.components.vendor.Vendor;
import de.pschiessle.lumenforge.components.vendor.VendorRequestDTO;
import de.pschiessle.lumenforge.components.vendor.VendorService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.time.LocalDate;
import java.util.List;
import java.util.Objects;

@Slf4j
@Component
@RequiredArgsConstructor
public class InitializeDatabaseService implements CommandLineRunner {


    public static final String INITIALIZE_DATABASE_STARTERDATA_LONG = "--init-with-starter-data";
    public static final String INITIALIZE_DATABASE_STARTERDATA_SHORT = "-s";

    public static final String CATEGORY_CSV_FILENAME = "/static/categories.csv";
    public static final String MAINTENANCE_STATUS_CSV_FILENAME = "/static/maintenance_status.csv";
    public static final String VENDOR_CSV_FILENAME = "/static/manufacturer.csv";
    public static final String DEVICE_NAMES_CSV_FILENAME = "/static/device_names.csv";


    private final CategoryService categoryService;
    private final VendorService vendorService;
    private final MaintenanceStatusService maintenanceStatusService;
    private final DeviceServiceImpl deviceService;
    private final StockServiceImpl stockService;

    @Override
    public void run(String... args) throws Exception {
        for (int i = 0; i < args.length; i++) {
            if (Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_SHORT) || Objects.equals(args[0], INITIALIZE_DATABASE_STARTERDATA_LONG)) {
                populateWithDummyData(categoryService, vendorService,maintenanceStatusService,deviceService,stockService);
                return;
            }
        }
        log.info("Found no command to generate database starter data");
    }

    public static void populateWithDummyData(CategoryService categoryService, VendorService vendorService, MaintenanceStatusService maintenanceStatusService, DeviceServiceImpl deviceService, StockServiceImpl stockService) throws Exception{
        log.info("Found command to generate database starter data, proceeding with categories...");
        var cat = InitializeDatabaseService.loadCategories(categoryService);
        log.info("Found command to generate database starter data, proceeding with vendors...");
        var ven = InitializeDatabaseService.loadVendors(vendorService);
        log.info("Found command to generate database starter data, proceeding with maintenance status...");
        var mai = InitializeDatabaseService.loadMaintenanceStatuses(maintenanceStatusService);
        log.info("Found command to generate database starter data, proceeding with devices...");
        InitializeDatabaseService.generateDevices(deviceService, stockService, cat, ven, mai);
    }

    public static List<Device> generateDevices(
            DeviceServiceImpl deviceService,
            StockServiceImpl stockService,
            List<Category> categories,
            List<Vendor> vendors,
            List<MaintenanceStatus> statuses
    ) throws IOException {
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
        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(
                        new ClassPathResource(DEVICE_NAMES_CSV_FILENAME).getInputStream(),
                        StandardCharsets.UTF_8))) {

            return reader.lines()
                    .skip(1) // header
                    .map(String::trim)
                    .filter(line -> !line.isEmpty())
                    .map(line -> line.split(",", 4))
                    .map(values -> {
                        Vendor vendor = vendors.get(rnd.nextInt(vendors.size()));
                        MaintenanceStatus status = statuses.get(rnd.nextInt(statuses.size()));

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

                        var purchaseDate = LocalDate.now().minusDays(rnd.nextLong(0, 365L * 5));

                        return new DeviceRequestDTO(
                                values[1],
                                values[0],
                                values[2],
                                null,
                                vendor.getId(),
                                BigDecimal.valueOf(Long.parseLong(values[3])),
                                purchaseDate,
                                categoryIds,
                                status.getId(),
                                new StockRequestDTO(StockUnitType.PIECES, new BigDecimal(rnd.nextInt(1, 20)), false)
                        );
                    })
                    .map(dto -> {
                        try {
                            return deviceService.create(dto);
                        } catch (RuntimeException e) {
                            log.warn("Skipping device row due to error: {}", e.getMessage());
                            return null;
                        }
                    })
                    .filter(java.util.Objects::nonNull)
                    .toList();
        }

    }


    public static List<Category> loadCategories(
            CategoryService categoryService
    ) throws Exception {
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

    public static List<Vendor> loadVendors(VendorService vendorService) throws Exception {
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


    public static List<MaintenanceStatus> loadMaintenanceStatuses(
            MaintenanceStatusService maintenanceStatusService
    ) throws Exception {
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
