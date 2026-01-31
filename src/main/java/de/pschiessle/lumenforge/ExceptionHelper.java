package de.pschiessle.lumenforge;

public class ExceptionHelper {
    public static <T> T require(T value, String msg) {
        if (value == null) throw new IllegalArgumentException(msg);
        return value;
    }
}
