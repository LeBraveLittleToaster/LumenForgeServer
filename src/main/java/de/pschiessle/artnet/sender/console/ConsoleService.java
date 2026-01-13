package de.pschiessle.artnet.sender.console;


import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class ConsoleService {

    private Console console;

    public ConsoleService(Console console) {
        this.console = console;
    }

    public synchronized Optional<Integer> setChannelValue(int channelIndex, int value) {
        return console.setSliderValueIfPossible(channelIndex, value);
    }

    public Slider[] getChannelSnapshot() {
        return console.getSliderClone();
    }
}
