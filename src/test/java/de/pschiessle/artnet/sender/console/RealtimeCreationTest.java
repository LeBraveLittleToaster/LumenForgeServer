package de.pschiessle.artnet.sender.console;

import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenDef;
import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenParam;
import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenType;
import de.pschiessle.artnet.sender.console.realtime.RealtimeEffect;
import de.pschiessle.artnet.sender.console.realtime.effectfuncs.SinusEffectFunc;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.Arrays;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertTrue;

@SpringBootTest
public class RealtimeCreationTest {

    @Test
    public void createSinusFuncTest() throws InterruptedException {
        List<EffectGenParam> paramList = List.of(
                new EffectGenParam(1L, SinusEffectFunc.EFFECT_FUNC_KEY_AMP_SCALE, 1),
                new EffectGenParam(2L, SinusEffectFunc.EFFECT_FUNC_KEY_TIME_OFFSET, 0)
        );
        EffectGenDef effectGenDef = new EffectGenDef(0L, EffectGenType.SINUS, paramList);
        var realtimeEffect = RealtimeEffect.build(effectGenDef);

        assertTrue(realtimeEffect.isEmpty());

        for(int iteration = 0; iteration < 60; iteration++){
            System.out.println(Arrays.toString(realtimeEffect.get().loop(System.currentTimeMillis())));
            Thread.sleep(100);
        }

    }
}
