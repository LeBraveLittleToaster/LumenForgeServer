package de.pschiessle.artnet.sender.web;

import de.pschiessle.artnet.sender.artnet.DeviceDTO;
import de.pschiessle.artnet.sender.artnet.DeviceService;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@Controller
@RequestMapping("/web/devices")
public class DeviceViewController {

    private final DeviceService deviceService;

    public DeviceViewController(DeviceService deviceService) {
        this.deviceService = deviceService;
    }

    @GetMapping
    public String listDevices(Model model) {
        List<DeviceDTO> devices = deviceService.getAllDevices();
        model.addAttribute("devices", devices);
        return "devices";
    }

    @GetMapping("/create")
    public String showCreateForm(Model model) {
        var device = new DeviceDTO();
        device.setArtnetUrl("localhost");
        device.setArtnetSubnet(0);
        device.setArtnetPort(6454);
        device.setArtnetUniverse(0);
        device.setIsDmxOTAV1Compatible(true);
        device.setName("Test Device");
        model.addAttribute("device", device);
        model.addAttribute("mode", "create");
        return "device-form";
    }

    @GetMapping("/{id}")
    public String showDevice(@PathVariable Long id, Model model) {
        DeviceDTO device = deviceService.getDeviceById(id)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Device not found"));
        model.addAttribute("device", device);
        return "device-detail";
    }



    @PostMapping("/create")
    public String handleCreate(@ModelAttribute("device") DeviceDTO deviceDTO) {
        deviceService.createDevice(deviceDTO);
        // redirect back to list
        return "redirect:/web/devices";
    }

    @GetMapping("/{id}/edit")
    public String showEditForm(@PathVariable Long id, Model model) {
        DeviceDTO device = deviceService.getDeviceById(id)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Device not found"));
        model.addAttribute("device", device);
        model.addAttribute("mode", "edit");
        return "device-form";
    }

    @PostMapping("/{id}")
    public String handleUpdate(@PathVariable Long id,
                               @ModelAttribute("device") DeviceDTO deviceDTO) {
        deviceService.updateDevice(id, deviceDTO)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Device not found"));
        return "redirect:/web/devices";
    }

    @PostMapping("/{id}/delete")
    public String handleDelete(@PathVariable Long id) {
        deviceService.deleteDevice(id);
        return "redirect:/web/devices";
    }
}
