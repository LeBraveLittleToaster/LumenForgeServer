package de.pschiessle.artnet.sender.effects;

import com.fasterxml.jackson.databind.JsonNode;

import java.util.Arrays;

import static de.pschiessle.artnet.sender.effects.EffectManager.DMX_UNIVERSE_SIZE;

public class PulseEffect implements Effect {

    private final double speed;
    private final double offset;
    private final int lightCount;
    private double intensity = 255;

    public PulseEffect(double speed, double offset, int lightCount) {
        this.speed = speed;
        this.offset = offset;
        this.lightCount = Math.max(Math.min(lightCount, DMX_UNIVERSE_SIZE), 0);
    }

    public byte[] render(long time) {

        byte[] dmxOut = new byte[DMX_UNIVERSE_SIZE];
        for(int segmentId = 0; segmentId < lightCount; segmentId++){
            dmxOut[segmentId * 5] = (byte) intensity;
            for(int channelOffset = 1; channelOffset < 5; channelOffset++){
                int i = segmentId * 5 + channelOffset;
                dmxOut[i] = (byte) (Math.round(Math.sin(((time * 0.001) + (offset * segmentId)) * speed) * 127) + 127);
            }
        }
        return dmxOut;
    }
}
