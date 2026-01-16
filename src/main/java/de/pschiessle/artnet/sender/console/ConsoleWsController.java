package de.pschiessle.artnet.sender.console;

import de.pschiessle.artnet.sender.console.realtime.ConsoleService;
import org.springframework.messaging.handler.annotation.DestinationVariable;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Controller;

@Controller
public class ConsoleWsController {

    private final ConsoleService consoleService;
    private final SimpMessagingTemplate messagingTemplate;

    public ConsoleWsController(ConsoleService consoleService,
                               SimpMessagingTemplate messagingTemplate) {
        this.consoleService = consoleService;
        this.messagingTemplate = messagingTemplate;
    }

    @MessageMapping("/slider/set/{channelId}")
    public void setSliderValue(@DestinationVariable int channelId, ValueUpdateMessage<Integer> value) {
        /*
        Optional<Integer> updatedValueOpt = consoleService.setChannelValue(channelId, value.value());
        updatedValueOpt
                .ifPresent(updated ->
                        messagingTemplate.convertAndSend("/topic/slider/set/" + channelId, new ValueUpdateResponse<>(value.clientId(), updated))
                );

         */
    }
}
