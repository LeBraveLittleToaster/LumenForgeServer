package de.pschiessle.artnet.sender.artnet;

import ch.bildspur.artnet.ArtNetClient;
import jakarta.annotation.PostConstruct;
import jakarta.annotation.PreDestroy;
import lombok.extern.slf4j.Slf4j;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.util.HashMap;

@Service
@Slf4j
public class ArtnetClient {

    ArtNetClient artnet = new ArtNetClient();

    private final DeviceService deviceService;

    public ArtnetClient(DeviceService deviceService) {
        this.deviceService = deviceService;
    }

    @PostConstruct
    public void start() {
        artnet.start();
    }

    public void sendDMX(byte[] data) {
        if (data == null) {
            return;
        }
        deviceService.getDevicesByIsActive(true).forEach((device) -> {
            artnet.unicastDmx(device.getArtnetUrl(), device.getArtnetSubnet(), device.getArtnetUniverse(), data);
        });
    }


    @PreDestroy
    public void shutdown() {
        try {
            artnet.stop();
        } catch (Exception ignored) {
        }
    }

}