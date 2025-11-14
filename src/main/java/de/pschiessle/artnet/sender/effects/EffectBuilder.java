package de.pschiessle.artnet.sender.effects;

import com.fasterxml.jackson.databind.JsonNode;

import java.util.Optional;

public class EffectBuilder {

    private static Optional<Effect> createPulseEffect(JsonNode jsonNode){
        try {
            double speed = jsonNode.get("speed").asDouble();
            double offset = jsonNode.get("offset").asDouble();
            int lightCount = jsonNode.get("ledCount").asInt();
            return Optional.of(new PulseEffect(speed, offset, lightCount));
        }catch (Exception e) {
            System.out.println("PULSE: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    private static Optional<Effect> createRunningEffect(JsonNode jsonNode){
        System.out.println("NODE DATA:");
        System.out.println(jsonNode);
        try {
            double speed = jsonNode.get("speed").asDouble();
            double offset = jsonNode.get("offset").asDouble();
            int lightCount = jsonNode.get("ledCount").asInt();
            return Optional.of(new RunningEffect(speed, offset, lightCount));
        }catch (Exception e) {
            System.out.println("RUNNING: Failed to obtain speed, offset and lightCount");
            return Optional.empty();
        }
    }

    static Optional<Effect> createEffectFromTypeAndJson(EffectType effectType, JsonNode jsonNode){
        return switch (effectType){
            case PULSE -> createPulseEffect(jsonNode);
            case RUNNING -> createRunningEffect(jsonNode);
            default -> Optional.empty();
        };
    }
}
