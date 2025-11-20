package de.pschiessle.artnet.sender.effects;

import de.pschiessle.artnet.sender.utils.ColorUtil;

import static de.pschiessle.artnet.sender.effects.EffectManager.DMX_UNIVERSE_SIZE;

public class ColorRampEffect implements Effect {

    private final double speed;
    private final int lightCount;
    private double intensity = 255;

    public ColorRampEffect(double speed, int lightCount) {
        this.speed = speed;
        this.lightCount = Math.max(Math.min(lightCount, DMX_UNIVERSE_SIZE), 0);
    }

    @Override
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
        float fadeValueColor = (float) Math.round(fade * 360); // 0..360

        for (int segmentId = 0; segmentId < lightCount; segmentId++) {
            int base = segmentId * 5;

            if (segmentId == activeSegment) {
                var rgb = ColorUtil.hsvToRgb(fadeValueColor, 1f,1f);
                dmxOut[base] = (byte) intensity;
                dmxOut[base + 1] = (byte) rgb.x.intValue();
                dmxOut[base + 2] = (byte) rgb.y.intValue();
                dmxOut[base + 3] = (byte) rgb.z.intValue();
                dmxOut[base + 1] = (byte) 0;

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
