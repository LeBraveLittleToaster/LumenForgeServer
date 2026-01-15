package de.pschiessle.artnet.sender.console.persistent.gendefs;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
public class EffectGenDef {
    @Id
    private Long id;

    @OneToOne
    private EffectGenType effectGenType;

    @OneToMany(cascade = CascadeType.ALL)
    private List<EffectGenParam> effectGenParams;
}
