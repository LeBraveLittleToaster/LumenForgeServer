package de.pschiessle.device.vendor;

import jakarta.persistence.*;
import lombok.Data;

@Data
@Entity
@Table(name = "vendor")
public class Vendor {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "name", nullable = false, unique = true)
    private String name;
}