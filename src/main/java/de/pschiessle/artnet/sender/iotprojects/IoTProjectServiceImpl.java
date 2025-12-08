package de.pschiessle.artnet.sender.iotprojects;

import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@Transactional
public class IoTProjectServiceImpl implements IoTProjectService {

    private final IoTProjectRepository repository;

    public IoTProjectServiceImpl(IoTProjectRepository repository) {
        this.repository = repository;
    }

    @Override
    public List<IoTProjectDTO> findAll() {
        return repository.findAll();
    }

    @Override
    public Optional<IoTProjectDTO> findById(Long id) {
        return repository.findById(id);
    }

    @Override
    public IoTProjectDTO save(IoTProjectDTO project) {
        project.setUuid(UUID.randomUUID().toString());
        LocalDateTime now = LocalDateTime.now();
        if (project.getCreatedAt() == null) {
            project.setCreatedAt(now);
        }
        project.setUpdatedAt(now);
        return repository.save(project);
    }

    @Override
    public void deleteById(Long id) {
        repository.deleteById(id);
    }
}