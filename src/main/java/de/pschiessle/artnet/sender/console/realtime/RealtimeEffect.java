package de.pschiessle.artnet.sender.console.realtime;


import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenDef;
import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenParam;
import de.pschiessle.artnet.sender.console.realtime.effectfuncs.EffectFunc;
import de.pschiessle.artnet.sender.console.realtime.effectfuncs.SinusEffectFunc;

import java.util.Optional;

public class RealtimeEffect {

    private EffectFunc effectFunc;

    private RealtimeEffect (EffectFunc effectFunc) {
        this.effectFunc = effectFunc;
    }

    public int[] loop(long currentMillis){
        return effectFunc.runEffect(currentMillis, new int[512]);
    }

    public static Optional<RealtimeEffect> build(EffectGenDef effectGenDef) {
        Optional<? extends EffectFunc> effectFunc = switch (effectGenDef.getEffectGenType()) {
            case SINUS -> getSinusEffectFunction(effectGenDef);
            default ->  Optional.empty();
        };
        return effectFunc.map(RealtimeEffect::new);
    }

    private static Optional<SinusEffectFunc> getSinusEffectFunction(EffectGenDef effectGenDef) {
        Optional<Float> amplitudeScale = effectGenDef.getEffectGenParams().stream()
                .filter((effectGenParam -> effectGenParam.getKey().equals(SinusEffectFunc.EFFECT_FUNC_KEY_AMP_SCALE)))
                .map(EffectGenParam::getValue)
                .findFirst();
        Optional<Float> timeOffset = effectGenDef.getEffectGenParams().stream()
                .filter((effectGenParam -> effectGenParam.getKey().equals(SinusEffectFunc.EFFECT_FUNC_KEY_TIME_OFFSET)))
                .map(EffectGenParam::getValue)
                .findFirst();
        if(timeOffset.isEmpty() || amplitudeScale.isEmpty()) return Optional.empty();
        return Optional.of(new SinusEffectFunc(0, amplitudeScale.get(), timeOffset.get()));
    }
}
