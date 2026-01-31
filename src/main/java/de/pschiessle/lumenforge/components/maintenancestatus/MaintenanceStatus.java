package de.pschiessle.lumenforge.components.maintenancestatus;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import org.hibernate.annotations.UuidGenerator;

@Getter
@Setter
@Entity
@Table(name = "maintenance_status")
public class MaintenanceStatus {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @UuidGenerator
    @Column(name = "uuid", nullable = false, unique = true, updatable = false, length = 36)
    private String uuid;

    @Column(name = "name")
    private String name;

    @Column(name = "description")
    private String description;

}