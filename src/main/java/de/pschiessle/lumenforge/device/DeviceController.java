package de.pschiessle.lumenforge.device;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("/api/v1/user/devices")
public class DeviceController {

    private final DeviceServiceImpl deviceService;

    public DeviceController(DeviceServiceImpl deviceService) {
        this.deviceService = deviceService;
    }

    @GetMapping()
    public Page<DeviceResponseDTO> getPage(
            @PageableDefault(size = 20, sort = "id", direction = Sort.Direction.DESC) Pageable pageable
    ) {
        return deviceService.getPage(pageable).map(DeviceResponseDTO::from);
    }

    @GetMapping("/{id}")
    public DeviceResponseDTO getById(@PathVariable Long id) {
        return DeviceResponseDTO.from(deviceService.getById(id));
    }

    @GetMapping("/by-uuid/{uuid}")
    public DeviceResponseDTO getByUuid(@PathVariable UUID uuid) {
        return DeviceResponseDTO.from(deviceService.getByUuid(uuid));
    }

    @PostMapping
    public ResponseEntity<DeviceResponseDTO> create(@RequestBody DeviceRequestDTO request) {
        Device created = deviceService.create(request);
        return ResponseEntity.status(HttpStatus.CREATED).body(DeviceResponseDTO.from(created));
    }

    @PutMapping("/{id}")
    public DeviceResponseDTO update(@PathVariable Long id, @RequestBody DeviceRequestDTO request) {
        return DeviceResponseDTO.from(deviceService.update(id, request));
    }

    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void delete(@PathVariable Long id) {
        deviceService.delete(id);
    }


}
