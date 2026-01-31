package de.pschiessle.lumenforge.components.device.dto;

import java.util.UUID;

public record DeviceListDTO(UUID uuid, String name, String serialNumber) {}