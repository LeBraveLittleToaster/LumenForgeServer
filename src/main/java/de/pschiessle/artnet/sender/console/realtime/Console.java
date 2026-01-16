package de.pschiessle.artnet.sender.console.realtime;

import de.pschiessle.artnet.sender.console.artnet.ArtnetClient;
import de.pschiessle.artnet.sender.console.artnet.ArtnetFrame;
import de.pschiessle.artnet.sender.console.realtime.effectfuncs.SinusEffectFunc;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

public class Console {
    private final Map<Integer, RealtimeChannel> channels = new HashMap<>();

    public Console(int channelCount) {
        for(int i = 0; i < channelCount; i++) {
            var channel = new RealtimeChannel(i);
            setChannelEffectChain(channel);
            channels.put(i, channel);
        }
    }

    private void setChannelEffectChain(RealtimeChannel realtimeChannel) {
        realtimeChannel.addEffect(new SinusEffectFunc(1, 0));
    }

    public void tick(ArtnetClient client, long now, long dt) {
        for (var ch : channels.values()) {
            var dmxValues = ch.tick(now, dt);
            client.broadcastFrames(List.of(new ArtnetFrame("", 0,0,dmxValues)));
        }
    }

    public Optional<RealtimeChannel> getChannelByIdIfPresent(int channelId) {
        return Optional.ofNullable(channels.getOrDefault(channelId, null));
    }
}
