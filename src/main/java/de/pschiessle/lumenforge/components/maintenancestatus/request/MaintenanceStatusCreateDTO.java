package de.pschiessle.lumenforge.components.maintenancestatus.request;

import de.pschiessle.lumenforge.components.general.IConvertableDTO;
import de.pschiessle.lumenforge.components.maintenancestatus.MaintenanceStatus;

import java.util.Optional;

public record MaintenanceStatusCreateDTO(
        String name,
        String description
) implements IConvertableDTO<MaintenanceStatus> {

    @Override
    public MaintenanceStatus fromDTO() {
        if (name == null) {
           throw new IllegalArgumentException("MaintenanceStatus Name should not be Null");
        }

        var status = new MaintenanceStatus();
        status.setName(name);
        status.setDescription(description);
        return status;
    }
}