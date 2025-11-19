package de.pschiessle.artnet.sender.effects;

import static de.pschiessle.artnet.sender.effects.EffectManager.DMX_UNIVERSE_SIZE;

public class StaticColorEffect implements Effect {

    private final double dimmer;
    private final int r;
    private final int g;
    private final int b;
    private final int w;
    private final int lightCount;

    public StaticColorEffect(double dimmer, int r, int g, int b, int w, int lightCount) {
        this.dimmer = dimmer;
        this.r = r;
        this.g = g;
        this.b = b;
        this.w = w;
        this.lightCount = Math.max(Math.min(lightCount, DMX_UNIVERSE_SIZE), 0);
    }

    public byte[] render(long time) {

        byte[] dmxOut = new byte[DMX_UNIVERSE_SIZE];
        for(int segmentId = 0; segmentId < lightCount; segmentId++){
            dmxOut[segmentId * 5] = (byte) Math.clamp(dimmer, 0, 255);
            dmxOut[(segmentId * 5) + 1] = (byte) r;
            dmxOut[(segmentId * 5) + 2] = (byte) g;
            dmxOut[(segmentId * 5) + 3] = (byte) b;
            dmxOut[(segmentId * 5) + 4] = (byte) w;
        }
        return dmxOut;
    }
}
