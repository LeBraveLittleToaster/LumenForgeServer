package de.pschiessle.artnet.sender.web;

import de.pschiessle.artnet.sender.devices.DeviceDTO;
import de.pschiessle.artnet.sender.devices.DeviceService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.List;
import java.util.Optional;

@Controller
@Slf4j
@RequestMapping("/web/dmx-ota")
public class DmxOtaViewController {
    private static String DMX_ADDRESS_PARAM = "dmxAddress";
    private static String DMX_UNIVERSE_PARAM = "dmxUniverse";
    private static String DMX_RECEIVER_TYPE_PARAM = "dmxReceiverType";
    private static String DMX_MODE_PARAM = "dmxMode";
    enum DmxMode {
        NO_CHANGE,
        DMX_1,
        DMX_5,
        DMX_30,
        DMX_40,
        DMX_80
    }

    enum DmxReceiverType {
        NO_CHANGE,
        WIRED_DMX,
        ARTNET
    }

    private final DeviceService deviceService;

    private final HttpClient httpClient;

    public DmxOtaViewController(DeviceService deviceService,  HttpClient httpClient) {
        this.deviceService = deviceService;
        this.httpClient = httpClient;
    }

    private Optional<String> sendHttpRequestToDevice(DeviceDTO deviceDTO, DmxConfigForm form) {

        StringBuilder urlBuilder = new StringBuilder();
        urlBuilder.append("https://");

        String artnetUrl = deviceDTO.getArtnetUrl();
        if (artnetUrl == null || artnetUrl.isBlank()) {
            return Optional.of("Mandatory field <Artnet URL> is empty, contact administrator!");
        }
        urlBuilder.append(artnetUrl);
        urlBuilder.append("/internal?");

        boolean isFirstParam = true;

        if(form.getDmxMode() != null && form.getDmxMode() != DmxMode.NO_CHANGE.ordinal()) {
            int modeEnum = form.getDmxMode() - 1;
            urlBuilder.append(DMX_MODE_PARAM);
            urlBuilder.append("=");
            urlBuilder.append(modeEnum);
            isFirstParam = false;
            urlBuilder.append("&");
        }

        if(form.getDmxAddress() != null &&  form.getDmxAddress() >= 0 && form.getDmxAddress() < 512) {
            if(isFirstParam) {
                isFirstParam = false;
            }else{
                urlBuilder.append("&");
            }
            urlBuilder.append(DMX_ADDRESS_PARAM);
            urlBuilder.append("=");
            urlBuilder.append(form.getDmxAddress());
        }

        if(form.getDmxUniverse() != null && form.getDmxUniverse() >= 0) {
            if(isFirstParam) {
                isFirstParam = false;
            }else{
                urlBuilder.append("&");
            }
            urlBuilder.append(DMX_UNIVERSE_PARAM);
            urlBuilder.append("=");
            urlBuilder.append(form.getDmxUniverse());

        }

        if(form.getReceiverType() != null && form.getReceiverType() != DmxReceiverType.NO_CHANGE.ordinal()) {
            if(isFirstParam) {
                isFirstParam = false;
            }else{
                urlBuilder.append("&");
            }
            int receiverType = form.getReceiverType() - 1;
            urlBuilder.append(DMX_RECEIVER_TYPE_PARAM);
            urlBuilder.append("=");
            urlBuilder.append(receiverType);
        }

        if(isFirstParam) {
            return Optional.of("No param specified!");
        }
        log.info("Sending GET to URL=" +  urlBuilder.toString());

        //TODO: Store them save, will be build with login!
        String creds = "admin:secret";
        String basicAuth = "Basic " + Base64.getEncoder().encodeToString(creds.getBytes(StandardCharsets.UTF_8));

        try {

            HttpRequest request = HttpRequest.newBuilder()
                    .GET()
                    .uri(URI.create(urlBuilder.toString()))
                    .header("Authorization", basicAuth)
                    .build();
            HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

            log.info("Status {}", response.statusCode());
        } catch (Exception ex) {
            // Log in real code – here we just surface the problem
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY,
                    "Failed to send request to lamp: " + ex.getMessage(), ex);
        }
        return Optional.empty();
    }

    @GetMapping
    public String showPage(Model model) {
        // Only DMX OTA v1 compatible devices
        List<DeviceDTO> devices = deviceService.getDeviceByIsDmxOTAV1Compatible(Boolean.TRUE)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Devices not found"));

        model.addAttribute("devices", devices);
        model.addAttribute("dmxConfig", new DmxConfigForm());
        model.addAttribute("dmxModes", DmxMode.values());
        model.addAttribute("receiverTypes", DmxReceiverType.values());
        return "dmx-ota";
    }

    @PostMapping("/send")
    public String sendDmxConfig(@ModelAttribute("dmxConfig") DmxConfigForm form,
                                Model model) {


        if (form.getDeviceUUID() == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "No device selected");
        }

        DeviceDTO device = deviceService.getDeviceByUuid(form.getDeviceUUID())
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Device not found"));

        var errors = sendHttpRequestToDevice(device, form);
        if(errors.isPresent()){
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, errors.get());
        }

        // Reload devices for the view and show success info
        List<DeviceDTO> devices = deviceService.getDeviceByIsDmxOTAV1Compatible(Boolean.TRUE)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Devices not found"));

        model.addAttribute("devices", devices);
        model.addAttribute("dmxConfig", form);
        model.addAttribute("message", "DMX configuration sent to device " + device.getName());
        model.addAttribute("dmxModes", DmxMode.values());
        model.addAttribute("receiverTypes", DmxReceiverType.values());
        return "dmx-ota";
    }
}
