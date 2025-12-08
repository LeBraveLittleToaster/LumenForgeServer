package de.pschiessle.artnet.sender.cert;

import de.pschiessle.artnet.sender.devices.CertificateGenerationService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.nio.file.Files;
import java.nio.file.Path;
import java.security.KeyStore;
import java.security.cert.X509Certificate;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
class CertificateGenerationServiceTest {

    @Autowired
    private CertificateGenerationService certificateGenerationService;

    @TempDir
    Path tempDir;

    @Test
    void generateAll_createsExpectedFiles() throws Exception {

        certificateGenerationService.generateAll(tempDir);

        assertThat(tempDir.resolve("exampleca.key")).exists().isRegularFile();
        assertThat(tempDir.resolve("exampleca.crt")).exists().isRegularFile();
        assertThat(tempDir.resolve("example.key")).exists().isRegularFile();
        assertThat(tempDir.resolve("example.crt")).exists().isRegularFile();
        assertThat(tempDir.resolve("example.key.DER")).exists().isRegularFile();
        assertThat(tempDir.resolve("example.crt.DER")).exists().isRegularFile();
        assertThat(tempDir.resolve("esp32-truststore.jks")).exists().isRegularFile();
        assertThat(tempDir.resolve("cert.h")).exists().isRegularFile();
        assertThat(tempDir.resolve("private_key.h")).exists().isRegularFile();
    }

    @Test
    void truststore_containsCaUnderCorrectAlias() throws Exception {

        certificateGenerationService.generateAll(tempDir);
        Path truststorePath = tempDir.resolve("esp32-truststore.jks");

        KeyStore ks = KeyStore.getInstance("JKS");
        try (var in = Files.newInputStream(truststorePath)) {
            ks.load(in, "changeit".toCharArray());
        }

        assertThat(ks.containsAlias("my-esp32-ca")).isTrue();
        var cert = ks.getCertificate("my-esp32-ca");
        assertThat(cert).isInstanceOf(X509Certificate.class);
        X509Certificate x509 = (X509Certificate) cert;

        assertThat(x509.getBasicConstraints())
                .as("CA basic constraints")
                .isGreaterThanOrEqualTo(0);
    }

    @Test
    void serverCertificate_hasExpectedSubjectCn() throws Exception {
        certificateGenerationService.generateAll(tempDir);
        Path serverCertPem = tempDir.resolve("example.crt");


        X509Certificate serverCert = certificateGenerationService.readPemCert(serverCertPem);


        String subjectDn = serverCert.getSubjectX500Principal().getName();
        assertThat(subjectDn).contains("CN=esp32.local");
    }
}
