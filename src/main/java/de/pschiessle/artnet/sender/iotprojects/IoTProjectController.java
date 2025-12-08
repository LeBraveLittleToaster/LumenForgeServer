package de.pschiessle.artnet.sender.iotprojects;

import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

@Slf4j
@Controller
@RequestMapping("/web/projects")
public class IoTProjectController {

    private final IoTProjectService service;

    public IoTProjectController(IoTProjectService service) {
        this.service = service;
    }

    @GetMapping
    public String listProjects(Model model) {
        List<IoTProjectDTO> projects = service.findAll();
        model.addAttribute("projects", projects);
        return "iotprojects/projects"; // projects.html
    }

    @GetMapping("/new")
    public String showCreateForm(Model model) {
        IoTProjectDTO project = new IoTProjectDTO();
        model.addAttribute("project", project);
        model.addAttribute("buildOptionsRaw", "");
        model.addAttribute("isNew", true);
        return "iotprojects/project-form";
    }

    @GetMapping("/{id}/edit")
    public String showEditForm(@PathVariable Long id, Model model) {
        IoTProjectDTO project = service.findById(id)
                .orElseThrow(() -> new IllegalArgumentException("Invalid project ID: " + id));

        model.addAttribute("project", project);
        model.addAttribute("isNew", false);
        return "iotprojects/project-form";
    }

    @PostMapping("/save")
    public String saveProject(@ModelAttribute("project") IoTProjectDTO project,
                              @RequestParam(value = "buildOptionsRaw", required = false) String buildOptionsRaw,
                              BindingResult bindingResult,
                              Model model) {

                if (bindingResult.hasErrors()) {
            model.addAttribute("isNew", project.getId() == null);
            model.addAttribute("buildOptionsRaw", buildOptionsRaw);
            return "iotprojects/project-form";
        }

        service.save(project);
        return "redirect:/web/projects";
    }

    @PostMapping("/{id}/delete")
    public String deleteProject(@PathVariable Long id) {
        service.deleteById(id);
        return "redirect:/projects";
    }

    @PostMapping("/{id}/genHTTPS")
    public String generateHttpsHeaders(@PathVariable Long id) {
        log.info("Generating HTTPS headers for project " + id);
        return "redirect:/projects";
    }

    private Map<String, String> parseBuildOptions(String text) {
        Map<String, String> map = new LinkedHashMap<>();
        if (text == null || text.trim().isEmpty()) {
            return map;
        }
        String[] lines = text.split("\\r?\\n");
        for (String line : lines) {
            String trimmed = line.trim();
            if (trimmed.isEmpty()) continue;
            String[] parts = trimmed.split("=", 2);
            if (parts.length != 2) {
                throw new IllegalArgumentException("Invalid build option line: '" + line + "'. Use key=value format.");
            }
            map.put(parts[0].trim(), parts[1].trim());
        }
        return map;
    }

    private String mapToText(Map<String, String> map) {
        if (map == null || map.isEmpty()) {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        map.forEach((k, v) -> sb.append(k).append("=").append(v == null ? "" : v).append("\n"));
        return sb.toString().trim();
    }
}