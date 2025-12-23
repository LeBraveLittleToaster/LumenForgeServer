package de.pschiessle.artnet.sender.auth;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;

@Configuration
@EnableMethodSecurity(prePostEnabled = true)
public class MethodSecurityConfig {
    // The annotation @EnableMethodSecurity activates the @PreAuthorize annotations
    // which are used by our custom @RequiresPermission annotation
}