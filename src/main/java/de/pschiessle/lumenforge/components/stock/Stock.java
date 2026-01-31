package de.pschiessle.lumenforge.components.stock;

import de.pschiessle.lumenforge.components.device.Device;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.UuidGenerator;

import java.math.BigDecimal;
import java.util.UUID;

@Data
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Table(
        name = "stock",
        uniqueConstraints = @UniqueConstraint(name = "uk_stock_device", columnNames = "device_id") // @OneToOne equivalent for now
)
public class Stock {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @UuidGenerator
    @Column(nullable = false, unique = true, updatable = false)
    private UUID uuid;

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "device_id", nullable = false)
    private Device device;

    @Column(name = "unit_stock_type", nullable = false, length = 32)
    @Enumerated(EnumType.STRING)
    private StockUnitType stockUnitType;

    @Column(name = "stock_count", nullable = false, precision = 19, scale = 6)
    private BigDecimal stockCount;

    @Column(name = "is_fractional", nullable = false)
    private boolean isFractional;
}

