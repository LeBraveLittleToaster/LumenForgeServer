package de.pschiessle.artnet.sender.console.artnet;

public record ArtnetFrame(String url, int subnet, int universe, byte[] dmxData) {
}
