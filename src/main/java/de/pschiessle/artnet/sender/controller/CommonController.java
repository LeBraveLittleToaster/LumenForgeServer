package de.pschiessle.artnet.sender.controller;

import lombok.extern.slf4j.Slf4j;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@Slf4j
@RestController
@RequestMapping("/common")
public class CommonController {

    @GetMapping("/hello")
    public String helloPublic() {
        return "Hi! this is public API!";
    }

}