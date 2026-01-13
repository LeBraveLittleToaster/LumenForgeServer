package de.pschiessle.artnet.sender.console;

public class Slider {

    public static int CHANNEL_MAX_VALUE = 255;

    public final int channelIndex;
    private int value = 0;

    public Slider(int channelIndex){
        this.channelIndex = channelIndex;
    }

    public synchronized int getValue() {
        return value;
    }

    public synchronized int setValue(int value) {
        if (value < 0 || value > CHANNEL_MAX_VALUE) {
            throw new IllegalArgumentException("Value must be between 1 and " + CHANNEL_MAX_VALUE);
        }
        this.value = value;
        return this.value;
    }
}
