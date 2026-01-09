package de.pschiessle.artnet.sender.console;

public record ValueUpdateMessage<T>(String clientId, T value) {
}
