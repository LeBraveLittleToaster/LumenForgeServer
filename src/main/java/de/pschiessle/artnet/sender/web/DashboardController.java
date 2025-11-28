package de.pschiessle.artnet.sender.artnet.web;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class DashboardController {

    @GetMapping({"/", "/dashboard"})
    public String dashboard(Model model) {
        // Add whatever stats you want later
        model.addAttribute("pageTitle", "Dashboard");
        return "dashboard";
    }
}
