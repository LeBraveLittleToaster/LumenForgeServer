package de.pschiessle.artnet.sender.console.realtime.effectfuncs;

public abstract class EffectFunc {


    public EffectFunc() {

    }

    public byte[] runEffect(double level, long currentMillis, long deltaMillis){
        return new byte[512];
    }

    protected byte[] clampToDMXRange(byte[] dmxValues) {
        for(byte i = 0; i < dmxValues.length; i++){
            dmxValues[i] = (byte) Math.clamp(dmxValues[i], 0, 255);
        }
        return dmxValues;
    }
}
