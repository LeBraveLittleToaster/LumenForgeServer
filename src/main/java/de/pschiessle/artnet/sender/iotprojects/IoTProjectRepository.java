package de.pschiessle.artnet.sender.iotprojects;


import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface IoTProjectRepository extends JpaRepository<IoTProjectDTO, Long> {

    Optional<IoTProjectDTO> findByUuid(String uuid);

    Optional<IoTProjectDTO> findByName(String name);
}