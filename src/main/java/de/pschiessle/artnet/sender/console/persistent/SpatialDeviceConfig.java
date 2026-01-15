package de.pschiessle.artnet.sender.console.persistent;

import de.pschiessle.artnet.sender.console.DMXDevice;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.OneToOne;
import lombok.Data;

@Data
@Entity
public class SpatialDeviceConfig {
    @Id
    private Long id;

    @OneToOne(cascade = CascadeType.PERSIST)
    private DMXDevice dmxDevice;

    @OneToOne(cascade = CascadeType.ALL)
    private Coordinate lightPosition;

    @OneToOne(cascade = CascadeType.ALL)
    private Coordinate lightRotation;


}
