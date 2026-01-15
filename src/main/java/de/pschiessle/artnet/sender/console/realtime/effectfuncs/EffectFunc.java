package de.pschiessle.artnet.sender.console.realtime.effectfuncs;

public abstract class EffectFunc {

    private int dmxAddress = 0;

    public EffectFunc(int dmxAddress) {
        this.dmxAddress = dmxAddress;
    }

    protected int getDmxAddress() {
        return this.dmxAddress;
    }

    public int[] runEffect(long currentTimeMillis, int[] dmxValues){
        return new int[512];
    }

    protected int[] clampToDMXRange(int[] dmxValues) {
        for(int i = 0; i < dmxValues.length; i++){
            dmxValues[i] = Math.clamp(dmxValues[i], 0, 255);
        }
        return dmxValues;
    }
}
