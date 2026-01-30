package de.pschiessle.lumenforge.device;

import de.pschiessle.lumenforge.device.category.Category;
import de.pschiessle.lumenforge.device.maintenancestatus.MaintenanceStatus;
import de.pschiessle.lumenforge.device.vendor.Vendor;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.math.BigDecimal;
import java.time.Instant;
import java.util.List;
import java.util.UUID;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Device {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @org.hibernate.annotations.UuidGenerator
    @Column(nullable = false, unique = true, updatable = false)
    private UUID uuid;

    @Column(name = "serial_number", unique = true)
    private String serialNumber;

    @Column(name = "device_name")
    private String name;

    @Column(name = "device_description")
    private String description;

    @Column(name = "photo_url")
    private String photoUrl;

    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "vendor_id", nullable = false)
    private Vendor vendor;

    @Column(name = "purchase_price", precision = 12, scale = 2)
    private BigDecimal purchasePrice;

    @Column(name = "purchase_date")
    private java.time.LocalDate purchaseDate;

    @ManyToMany
    @JoinTable(
            name = "device_category",
            joinColumns = @JoinColumn(name = "device_id"),
            inverseJoinColumns = @JoinColumn(name = "category_id")
    )
    private List<Category> categories;

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "maintenance_status_id", nullable = false)
    private MaintenanceStatus maintenanceStatus;

    @OneToMany(mappedBy = "device", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<DeviceParameter> deviceParameters;

    @CreationTimestamp
    @Column(name = "create_date")
    private Instant createDate;

    @UpdateTimestamp
    @Column(name = "update_date")
    private Instant updateDate;
}
