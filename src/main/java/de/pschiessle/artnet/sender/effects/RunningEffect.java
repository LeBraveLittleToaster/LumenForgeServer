package de.pschiessle.artnet.sender.effects;

import static de.pschiessle.artnet.sender.effects.EffectManager.DMX_UNIVERSE_SIZE;

public class RunningEffect implements Effect{
    private final double speed;
    private final int lightCount;
    private double intensity = 255;


    public RunningEffect(double speed, double offset, int lightCount) {
        this.speed = speed;
        this.lightCount = Math.max(Math.min(lightCount, DMX_UNIVERSE_SIZE), 0);
    }

    public byte[] render(long time) {

        byte[] dmxOut = new byte[DMX_UNIVERSE_SIZE];

        double baseCycleMs = 1000.0;
        double cycleMs = baseCycleMs / speed;

        long stepIndex = (long) (time / cycleMs);
        int activeSegment = (int) (stepIndex % lightCount);

        double localT = (time % (long) cycleMs) / cycleMs;

        double fade = Math.sin(localT * Math.PI);
        if (fade < 0) fade = 0;

        int fadeValue = (int) Math.round(fade * 255); // 0..255

        for (int segmentId = 0; segmentId < lightCount; segmentId++) {
            int base = segmentId * 5;

            if (segmentId == activeSegment) {
                dmxOut[base] = (byte) intensity;
                for (int channelOffset = 1; channelOffset < 5; channelOffset++) {
                    dmxOut[base + channelOffset] = (byte) fadeValue;
                }
            } else {
                dmxOut[base] = 0;
                for (int channelOffset = 1; channelOffset < 5; channelOffset++) {
                    dmxOut[base + channelOffset] = 0;
                }
            }
        }
        return dmxOut;
    }
}
