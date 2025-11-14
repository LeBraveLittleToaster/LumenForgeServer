package de.pschiessle.artnet.sender.effects;

import com.fasterxml.jackson.databind.JsonNode;

public interface Effect {
    byte[] render(long time);
}
