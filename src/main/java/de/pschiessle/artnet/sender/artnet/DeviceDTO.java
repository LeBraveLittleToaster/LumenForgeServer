package de.pschiessle.artnet.sender.artnet;

import jakarta.annotation.Nonnull;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;

@NoArgsConstructor
@AllArgsConstructor
@Data
@Entity
@Table(name = "artnet_devices")
public class DeviceDTO implements Serializable {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Long id;

    @Column(nullable = false, unique = true)
    private String uuid;

    @Column(nullable = false)
    private Boolean isActive;

    @Column(nullable = false)
    private Boolean isDmxOTAV1Compatible;

    @Column(nullable = false, unique = true)
    private String name;

    @Column(nullable = false)
    private String artnetUrl;

    @Column(nullable = false)
    private Integer artnetPort;

    @Column(nullable = false)
    private Integer artnetUniverse;

    @Column(nullable = false)
    private Integer artnetSubnet;

    @Column(nullable = false)
    private String x509Certificate;

    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
}
