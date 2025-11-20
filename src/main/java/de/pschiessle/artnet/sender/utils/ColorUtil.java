package de.pschiessle.artnet.sender.utils;

import java.awt.*;

public class ColorUtil {
    public static Triple<Integer, Integer, Integer> hsvToRgb(float hue, float saturation, float value) {

        Color rgb = Color.getHSBColor(hue / 360f, saturation, value);

        return new Triple<>(rgb.getRed(), rgb.getGreen(), rgb.getBlue());
    }
}
