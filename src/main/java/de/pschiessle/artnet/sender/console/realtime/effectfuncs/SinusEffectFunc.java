package de.pschiessle.artnet.sender.console.realtime.effectfuncs;

public class SinusEffectFunc extends EffectFunc {

    public static String EFFECT_FUNC_KEY_AMP_SCALE = "SINUS_AMP_SCALE";
    public static String EFFECT_FUNC_KEY_TIME_OFFSET = "SINUS_AMP_SCALE";

    private float frequency = 1;
    private float timeOffset = 0;

    public SinusEffectFunc(float frequency, float timeOffset) {
        this.frequency = frequency;
        this.timeOffset = timeOffset;
    }


    @Override
    public byte[] runEffect(double level, long currentMillis, long deltaMillis) {
        double t = currentMillis * 1e-3;
        var sinusV = (Math.sin(t / frequency + timeOffset) + 1) / 2;
        var out = sinusV * (level / 100d) * 255d;
        var outV = new byte[]{(byte) (out)};
        System.out.println(out + " | " + sinusV + " | " + (Math.sin(currentMillis) * 255 * frequency * (level / 100d)));

        return outV;
    }
}
