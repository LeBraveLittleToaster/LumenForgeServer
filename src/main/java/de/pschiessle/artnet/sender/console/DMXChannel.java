package de.pschiessle.artnet.sender.console;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@AllArgsConstructor
@Entity
public class DMXChannel {
    @Id
    private Long id;

    @Column(name = "dmx_address", nullable = false)
    private int address;
    @Column(name = "dmx_Universe", nullable = false)
    private int universe;

    @Column(name = "dmx_channel_type", nullable = false)
    private DMXChannelType dmxChannelType;
}
