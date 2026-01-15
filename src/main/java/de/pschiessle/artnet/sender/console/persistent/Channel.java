package de.pschiessle.artnet.sender.console.persistent;


import de.pschiessle.artnet.sender.console.persistent.gendefs.EffectGenDef;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@NoArgsConstructor
@AllArgsConstructor
@Data
@Entity
public class Channel {
    @Id
    private Long id;

    @OneToOne(cascade = CascadeType.ALL)
    private SpatialDeviceConfig spatialDeviceConfig;

    @OneToMany(cascade = CascadeType.ALL)
    private List<EffectGenDef> effectGeneratorStack;
}
