package de.pschiessle.artnet.sender.web;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@AllArgsConstructor
@Data
public class DmxConfigForm {
    private String deviceUUID;
    private String url;
    private Integer dmxAddress;
    private Integer dmxMode;
    private Integer dmxUniverse;
    private Integer receiverType;
}
