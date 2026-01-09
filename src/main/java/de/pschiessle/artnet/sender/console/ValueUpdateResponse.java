package de.pschiessle.artnet.sender.console;

import org.springframework.messaging.core.MessagePostProcessor;

public record ValueUpdateResponse<T>(String clientId, Integer value) {
}
