package de.pschiessle.artnet.sender.controller;

import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@Slf4j
@RestController
@RequestMapping("/api/v1/admin")
public class AdminController {

    @PreAuthorize("hasAuthority('read:users')")
    @GetMapping("/users")
    public ResponseEntity<?> getAll() {
        return ResponseEntity.ok("All users");
    }
}