package de.pschiessle.artnet.sender.console;

import de.pschiessle.artnet.sender.console.artnet.ArtnetClient;
import de.pschiessle.artnet.sender.console.realtime.Console;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

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
