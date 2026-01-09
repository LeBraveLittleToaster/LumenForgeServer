package de.pschiessle.artnet.sender.console;

import org.springframework.context.event.EventListener;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.messaging.SessionConnectedEvent;

import java.util.Arrays;

@Component
public class WsConnectListener {

    private final SimpMessagingTemplate messagingTemplate;
    private final ConsoleService consoleService;

    public WsConnectListener(SimpMessagingTemplate messagingTemplate,
                             ConsoleService consoleService) {
        this.messagingTemplate = messagingTemplate;
        this.consoleService = consoleService;
    }

    @EventListener
    public void onConnect(SessionConnectedEvent event) {
        Arrays.stream(consoleService.getChannelSnapshot())
                .forEach(channel -> messagingTemplate.convertAndSend(
                        "/topic/slider/set/" + channel.channelIndex,
                        channel.getValue()
                ));

    }
}
