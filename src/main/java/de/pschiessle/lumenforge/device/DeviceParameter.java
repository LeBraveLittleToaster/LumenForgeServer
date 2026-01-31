package de.pschiessle.lumenforge.device;

import jakarta.persistence.*;

@Entity
public record DeviceParameter(
        @Id @GeneratedValue(strategy = GenerationType.IDENTITY) Long id,

        @Column(name = "key", nullable = false)
        String key,

        @Column(name = "value", nullable = false)
        String value,

        @ManyToOne(fetch = FetchType.LAZY)
        @JoinColumn(name = "device_id", nullable = false)
        Device device
) {
}
