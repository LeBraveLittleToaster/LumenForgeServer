package de.pschiessle.lumenforge.console;

import de.pschiessle.device.sender.console.artnet.ArtnetClient;
import de.pschiessle.device.sender.console.realtime.Console;
import org.junit.jupiter.api.Test;

public class ConsoleCreationTest {

    @Test
    public void creationTest() throws InterruptedException {

        var console = new Console(1);

        var client = new ArtnetClient();
        var ts = System.currentTimeMillis();

        for(int i = 0; i < 60; i++){
            var now = System.currentTimeMillis();
            var delta = now - ts;
            console.tick(client, ts, delta);
            ts = now;
            Thread.sleep(100);
        }

    }
}
