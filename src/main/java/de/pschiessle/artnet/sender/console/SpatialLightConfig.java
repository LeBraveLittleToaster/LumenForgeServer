package de.pschiessle.artnet.sender.console;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import lombok.Data;

import java.util.Map;

@Data
@Entity
public class SpatialLightConfig {
    @Id
    private Long id;

}
