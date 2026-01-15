package de.pschiessle.artnet.sender.console.realtime.effectfuncs;

public class SinusEffectFunc extends EffectFunc {

    public static String EFFECT_FUNC_KEY_AMP_SCALE = "SINUS_AMP_SCALE";
    public static String EFFECT_FUNC_KEY_TIME_OFFSET = "SINUS_AMP_SCALE";

    private float amplitudeScale = 1;
    private float timeOffset = 0;

    public SinusEffectFunc(int dmxAddress, float amplitudeScale, float timeOffset) {
        super(dmxAddress);
        this.amplitudeScale = amplitudeScale;
        this.timeOffset = timeOffset;
    }


    @Override
    public int[] runEffect(long currentTimeMillis, int[] dmxvalues) {
        dmxvalues[getDmxAddress()] = (int) (Math.sin(currentTimeMillis + timeOffset ) * 255 * amplitudeScale);
        return clampToDMXRange(dmxvalues);
    }
}
