package de.pschiessle.artnet.sender.effects;

import com.fasterxml.jackson.databind.JsonNode;
import lombok.extern.slf4j.Slf4j;

import java.util.Optional;

@Slf4j
public class EffectBuilder {

    private static Optional<Effect> createPulseEffect(JsonNode jsonNode){
        try {
            double speed = jsonNode.get("speed").asDouble();
            double offset = jsonNode.get("offset").asDouble();
            int lightCount = jsonNode.get("ledCount").asInt();
            return Optional.of(new PulseEffect(speed, offset, lightCount));
        }catch (Exception e) {
            log.info("PULSE: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    private static Optional<Effect> createRunningEffect(JsonNode jsonNode){
        log.info("NODE DATA:");
        log.info(String.valueOf(jsonNode));
        try {
            double speed = jsonNode.get("speed").asDouble();
            double offset = jsonNode.get("offset").asDouble();
            int lightCount = jsonNode.get("ledCount").asInt();
            return Optional.of(new RunningEffect(speed, offset, lightCount));
        }catch (Exception e) {
            log.info("RUNNING: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    private static Optional<Effect> createStrobeEffect(JsonNode jsonNode){
        log.info("NODE DATA:");
        log.info(String.valueOf(jsonNode));
        try {
            var ticksOn = jsonNode.get("ticksOn").asInt();
            var ticksOff = jsonNode.get("ticksOff").asInt();
            int lightCount = jsonNode.get("ledCount").asInt();
            return Optional.of(new StrobeEffect(ticksOn, ticksOff, lightCount));
        }catch (Exception e) {
            log.info("STROBE: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    private static Optional<Effect> createColorEffect(JsonNode jsonNode){
        log.info("NODE DATA:");
        log.info(String.valueOf(jsonNode));
        try {
            var dimmer = jsonNode.get("dimmer").asInt();
            int lightCount = jsonNode.get("ledCount").asInt();
            int r = jsonNode.get("r").asInt();
            int g = jsonNode.get("g").asInt();
            int b = jsonNode.get("b").asInt();
            int w = jsonNode.get("w").asInt();
            return Optional.of(new StaticColorEffect(dimmer,r,g,b,w, lightCount));
        }catch (Exception e) {
            log.info("STROBE: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    static Optional<Effect> createEffectFromTypeAndJson(EffectType effectType, JsonNode jsonNode){
        return switch (effectType){
            case PULSE -> createPulseEffect(jsonNode);
            case RUNNING -> createRunningEffect(jsonNode);
            case STROBE -> createStrobeEffect(jsonNode);
            case STATIC_COLOR -> createColorEffect(jsonNode);
            default -> Optional.empty();
        };
    }
}
