package de.pschiessle.artnet.sender.console.realtime;

import de.pschiessle.artnet.sender.console.realtime.effectfuncs.EffectFunc;
import lombok.Setter;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.List;

public class RealtimeChannel {
    private final int id;
    @Setter
    private double level = 100;
    private final List<EffectFunc> chain = new ArrayList<>();

    public RealtimeChannel(int id) {
        this.id = id; }

    public void addEffect(EffectFunc fx) {
        chain.add(fx); }

    public byte[] tick(long now, long dt) {
        byte[] dmxOut = new byte[512];
        for (var fx : chain){
            var v = fx.runEffect(level, now, dt);
            System.arraycopy(v, 0, dmxOut, 0, v.length);
        }
        return dmxOut;
    }
}