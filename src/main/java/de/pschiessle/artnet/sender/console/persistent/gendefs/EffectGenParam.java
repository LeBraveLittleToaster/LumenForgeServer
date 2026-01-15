package de.pschiessle.artnet.sender.console.persistent.gendefs;


import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@AllArgsConstructor
@Data
@Entity
public class EffectGenParam {
    @Id
    private Long id;

    private String key;

    private float value;
}
