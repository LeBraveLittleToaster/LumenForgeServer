package de.pschiessle.artnet.sender.console;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
public class ConsoleCreationTest {

    @Test
    public void creationTest(){
        var console = new Console(8);

        for(Channel channel : console.getChannelsClone()) {
            System.out.println(channel.channelIndex + " Value " + channel.getValue());
        }
    }
}
