package de.pschiessle.artnet.sender.artnet;

import ch.bildspur.artnet.ArtNetClient;
import jakarta.annotation.PostConstruct;
import jakarta.annotation.PreDestroy;
import org.springframework.stereotype.Service;

import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.ScheduledExecutorService;

import static de.pschiessle.artnet.sender.effects.EffectManager.DMX_UNIVERSE_SIZE;

@Service
public class ArtnetClient {

    ArtNetClient artnet = new ArtNetClient();



    @PostConstruct
    public void start() {
        // Start ArtNet client once
        artnet.start();
    }

    /**
     * Public API: enqueue a DMX frame.
     * Any length is accepted; it will be normalized to 512 bytes.
     */
    public void sendDMX(byte[] data) {
        if (data == null) {
            return;
        }
        artnet.unicastDmx("localhost", 0, 0, data);
        artnet.unicastDmx("192.168.178.99", 0, 0, data);
    }


    @PreDestroy
    public void shutdown() {
        try {
            artnet.stop();
        } catch (Exception ignored) {
        }
    }

}