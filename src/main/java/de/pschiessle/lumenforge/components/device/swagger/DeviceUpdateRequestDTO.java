package de.pschiessle.lumenforge.components.device.swagger;

import de.pschiessle.lumenforge.components.device.DeviceRequestDTO;

public record DeviceUpdateRequestDTO(
        Long id,
        DeviceRequestDTO device
) {}
