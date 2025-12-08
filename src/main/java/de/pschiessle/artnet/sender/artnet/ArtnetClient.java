package de.pschiessle.artnet.sender.artnet;

import ch.bildspur.artnet.ArtNetClient;
import de.pschiessle.artnet.sender.devices.DeviceService;
import jakarta.annotation.PostConstruct;
import jakarta.annotation.PreDestroy;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

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
        /*
        artnet.unicastDmx("localhost", 0,0, data);
        artnet.unicastDmx("192.168.178.99", 0,0, data);
        */
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