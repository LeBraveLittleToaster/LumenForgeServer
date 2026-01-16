package de.pschiessle.artnet.sender.console.realtime;


import de.pschiessle.artnet.sender.console.artnet.ArtnetClient;
import org.springframework.stereotype.Service;

@Service
public class ConsoleService {

    private final Console console = new Console(1);
    private final ArtnetClient client;

    public ConsoleService(ArtnetClient client) {
        this.client = client;
    }

    public void setLevel(int channelId, double level) {
        console.getChannelByIdIfPresent(channelId)
                .ifPresent(realtimeChannel -> realtimeChannel.setLevel(level));
    }

    public void tick(long now, long dt) {
        console.tick(client, now, dt);
    }
}