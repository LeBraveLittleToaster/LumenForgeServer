package de.pschiessle.artnet.sender.web;

import com.fasterxml.jackson.databind.JsonNode;
import de.pschiessle.artnet.sender.effects.EffectManager;
import de.pschiessle.artnet.sender.effects.EffectType;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.awt.*;

@RestController()
@RequestMapping("/effects")
public class EffectEndpoint {


    private final EffectManager effectManager;

    public EffectEndpoint(EffectManager effectManager) {
        this.effectManager = effectManager;
    }

    @PostMapping(value = "/set/{id}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    void setEffectFromId(@PathVariable("id") String effectId, @RequestBody JsonNode options) {
        try {
            effectManager.setEffect(EffectType.values()[Integer.parseInt(effectId)], options);
            System.out.println("Setting EffectId=" + effectId + " | options=" + options.toPrettyString() );
        } catch (Exception _) {
            System.out.println("Failed to parse EffectType");
        }
    }
}
