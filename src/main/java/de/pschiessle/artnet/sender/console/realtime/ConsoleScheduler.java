package de.pschiessle.artnet.sender.console.realtime;

import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

@Component
public class ConsoleScheduler {
    private final ConsoleService consoleService;
    private long last = System.currentTimeMillis();

    public ConsoleScheduler(ConsoleService consoleService) {
        this.consoleService = consoleService;
    }

    @Scheduled(fixedRate = 500)
    public void run() {
        long now = System.currentTimeMillis();
        consoleService.tick(now, now - last);
        last = now;
    }
}