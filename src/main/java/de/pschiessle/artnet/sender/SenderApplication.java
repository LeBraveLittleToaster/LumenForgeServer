package de.pschiessle.artnet.sender;

import de.pschiessle.artnet.sender.console.Console;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.Bean;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;

@SpringBootApplication
@EnableCaching
@EnableWebSecurity
public class SenderApplication {

	@Bean
	public Console generateConsole(){
		return new Console(5);
	}

	public static void main(String[] args) {
		SpringApplication.run(SenderApplication.class, args);
	}

}

