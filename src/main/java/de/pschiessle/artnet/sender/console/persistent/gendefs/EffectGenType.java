package de.pschiessle.artnet.sender.console.persistent.gendefs;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;

@Entity
public enum EffectGenType {
    SINUS,
    CHASE,
    CONSTANT;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Id
    private Long id;
}
