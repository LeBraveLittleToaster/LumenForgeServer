package de.pschiessle.artnet.sender.console;


import de.pschiessle.artnet.sender.console.persistent.Channel;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class ConsoleService {

    private Console console;

    public ConsoleService(Console console) {
        this.console = console;
    }

}
