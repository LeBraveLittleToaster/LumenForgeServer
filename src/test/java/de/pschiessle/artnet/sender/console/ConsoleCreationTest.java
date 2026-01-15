package de.pschiessle.artnet.sender.console;

import de.pschiessle.artnet.sender.console.persistent.Channel;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
public class ConsoleCreationTest {

    @Test
    public void creationTest(){

        var console = new Console(8);


    }
}
