package de.pschiessle.artnet.sender.utils;

import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.Calendar;

public class TimeUtil {
    public static LocalDateTime getCurrentUtc() {
        return LocalDateTime.now(ZoneId.of("UTC"));
    }
}
