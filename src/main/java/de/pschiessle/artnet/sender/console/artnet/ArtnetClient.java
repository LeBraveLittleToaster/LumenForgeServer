package de.pschiessle.artnet.sender.console.artnet;

import ch.bildspur.artnet.ArtNetClient;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ArtnetClient {

    private ArtNetClient client;

    public ArtnetClient() {
        this.client = new ArtNetClient();
        this.client.start();
    }

    public void broadcastFrames(List<ArtnetFrame> artnetFrames) {
        for(ArtnetFrame frame : artnetFrames) {
            client.broadcastDmx(frame.subnet(), frame.universe(), frame.dmxData());
        }
    }

    public void unicastFrames(List<ArtnetFrame> artnetFrames) {
        for(ArtnetFrame frame : artnetFrames) {
            client.unicastDmx(frame.url(), frame.subnet(), frame.universe(), frame.dmxData());
        }
    }
}
