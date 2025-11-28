package de.pschiessle.artnet.sender.web;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.ResourceLoader;

import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManagerFactory;
import java.io.InputStream;
import java.security.KeyStore;

@Configuration
public class TlsConfig {

    private final ResourceLoader resourceLoader;

    public TlsConfig(ResourceLoader resourceLoader) {
        this.resourceLoader = resourceLoader;
    }

    @Bean
    public SSLContext sslContext() throws Exception {
        String truststoreLocation = "classpath:esp32-truststore.jks";
        char[] truststorePassword = "changeit".toCharArray();
        String truststoreType = "JKS";

        KeyStore trustStore = KeyStore.getInstance(truststoreType);
        try (InputStream is = resourceLoader.getResource(truststoreLocation).getInputStream()) {
            trustStore.load(is, truststorePassword);
        }

        TrustManagerFactory tmf =
                TrustManagerFactory.getInstance(TrustManagerFactory.getDefaultAlgorithm());
        tmf.init(trustStore);

        SSLContext sslContext = SSLContext.getInstance("TLS");
        sslContext.init(null, tmf.getTrustManagers(), null);

        return sslContext;
    }
}