package de.pschiessle.artnet.sender.console.persistent;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@AllArgsConstructor
@Data
@Entity
public class Coordinate {

    @Id
    private Long id;
    private int x;
    private int y;

}
