package de.pschiessle.lumenforge.components.device.dto;

import de.pschiessle.lumenforge.components.category.Category;
import de.pschiessle.lumenforge.components.device.Device;
import de.pschiessle.lumenforge.components.general.IConvertableDTO;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.components.vendor.Vendor;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

public record DeviceCreateDTO(
        String serialNumber,
        String name,
        String description,
        String photoUrl,
        Vendor vendor,
        BigDecimal purchasePrice,
        LocalDate purchaseDate,
        List<Category> categories,
        MaintenanceStatus maintenanceStatus
) implements IConvertableDTO<Device> {



    @Override
    public Device fromDTO() {
        if (vendor == null){
            throw new IllegalArgumentException("Vendor should not be Null");
        }
        if(maintenanceStatus == null){
            throw new IllegalArgumentException("Maintenance Status should not be Null");
        }

        var device = new Device();
        device.setSerialNumber(serialNumber);
        device.setName(name);
        device.setDescription(description);
        device.setPhotoUrl(photoUrl);
        device.setVendor(vendor);
        device.setPurchasePrice(purchasePrice);
        device.setPurchaseDate(purchaseDate);
        device.setCategories(categories);
        device.setMaintenanceStatus(maintenanceStatus);

        return device;
    }
}
