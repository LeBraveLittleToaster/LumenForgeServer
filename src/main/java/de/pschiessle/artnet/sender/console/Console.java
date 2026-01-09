package de.pschiessle.artnet.sender.console;

import java.util.Optional;
import java.util.stream.IntStream;

public class Console {
    private Channel[] channels;
    private ChannelToDmxRouting channelToDmxRouting;

    public Console (int numberOfChannels) {
        generateChannelsAndRouting(numberOfChannels);
    }

    private void generateChannelsAndRouting(int numberOfChannels) {
        this.channels = IntStream.range(0, numberOfChannels).mapToObj(Channel::new).toArray(Channel[]::new);
    }

    public Optional<Integer> setChannelValueIfPossible(int channelIndex, int value) {
        if(channelIndex < 0 || channelIndex >= this.channels.length) {
            return Optional.empty();
        }
        return Optional.of(this.channels[channelIndex].setValue(value));
    }

    public Channel[] getChannelsClone() {
        return channels.clone();
    }
}
