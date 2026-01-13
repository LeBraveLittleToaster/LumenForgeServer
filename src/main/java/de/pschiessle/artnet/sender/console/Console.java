package de.pschiessle.artnet.sender.console;

import java.util.List;
import java.util.Optional;
import java.util.stream.IntStream;

public class Console {
    private Slider[] sliders;
    private List<DMXDevice> devices;

    public Console (int numberOfChannels) {
        generateChannelsAndRouting(numberOfChannels);
    }

    private void generateChannelsAndRouting(int numberOfChannels) {
        this.sliders = IntStream.range(0, numberOfChannels).mapToObj(Slider::new).toArray(Slider[]::new);
    }

    public Optional<Integer> setSliderValueIfPossible(int channelIndex, int value) {
        if(channelIndex < 0 || channelIndex >= this.sliders.length) {
            return Optional.empty();
        }
        return Optional.of(this.sliders[channelIndex].setValue(value));
    }

    public Slider[] getSliderClone() {
        return sliders.clone();
    }
}
