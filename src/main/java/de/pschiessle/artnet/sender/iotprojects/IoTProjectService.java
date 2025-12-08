package de.pschiessle.artnet.sender.iotprojects;


import java.util.List;
import java.util.Optional;

public interface IoTProjectService {

    List<IoTProjectDTO> findAll();

    Optional<IoTProjectDTO> findById(Long id);

    IoTProjectDTO save(IoTProjectDTO project);

    void deleteById(Long id);
}