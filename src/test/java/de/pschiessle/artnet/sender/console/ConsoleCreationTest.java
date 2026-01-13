package de.pschiessle.artnet.sender.console;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
public class ConsoleCreationTest {

    @Test
    public void createConsoleTest() {
        Console console = new Console(8);

        for(Slider sliderClone : console.getSliderClone()) {
            System.out.println(sliderClone);
        }
    }
}
