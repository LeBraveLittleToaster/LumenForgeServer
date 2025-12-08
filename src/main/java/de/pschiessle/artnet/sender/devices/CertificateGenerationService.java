package de.pschiessle.artnet.sender.devices;

import org.bouncycastle.asn1.x500.X500Name;
import org.bouncycastle.asn1.x509.*;
import org.bouncycastle.cert.X509CertificateHolder;
import org.bouncycastle.cert.X509v3CertificateBuilder;
import org.bouncycastle.cert.jcajce.JcaX509CertificateConverter;
import org.bouncycastle.cert.jcajce.JcaX509ExtensionUtils;
import org.bouncycastle.cert.jcajce.JcaX509v3CertificateBuilder;
import org.bouncycastle.jce.provider.BouncyCastleProvider;
import org.bouncycastle.operator.ContentSigner;
import org.bouncycastle.operator.jcajce.JcaContentSignerBuilder;
import org.springframework.stereotype.Service;

import java.io.FileOutputStream;
import java.io.IOException;
import java.math.BigInteger;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.security.*;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.time.Instant;
import java.time.temporal.ChronoUnit;
import java.util.Base64;
import java.util.Date;

@Service
public class CertificateGenerationService {

    private static final String BC = "BC";
    private static final String SIGNATURE_ALG = "SHA256withRSA";

    private static final String FILE_NAME_CA_PEM_PRIVATE_KEY = "ca.key";
    private static final String FILE_NAME_CA_PEM_PUBLIC_KEY = "ca.crt";
    private static final String FILE_NAME_PEM_PUBLIC_KEY = "device.key";
    private static final String FILE_NAME_PEM_PRIVATE_KEY = "device.crt";

    private static final String FILE_NAME_PEM_DER_PUBLIC_KEY = "device.key.DER";
    private static final String FILE_NAME_PEM_DER_PRIVATE_KEY = "device.crt.DER";

    private static final String FILE_NAME_CPP_CERT = "cert.h";

    private static final int KEY_LENGTH = 4096;

    static {
        if (Security.getProvider(BC) == null) {
            Security.addProvider(new BouncyCastleProvider());
        }
    }

    /**
     * Generates everything into the given directory:
     * exampleca.key, exampleca.crt, example.key, example.crt,
     * example.key.DER, example.crt.DER, esp32-truststore.jks,
     * cert.h, private_key.h
     */
    public void generateAll(Path outDir) throws Exception {
        Files.createDirectories(outDir);

        KeyPair caKeyPair = generateRsaKeyPair(KEY_LENGTH);
        X509Certificate caCert = generateCaCertificate(caKeyPair);

        KeyPair serverKeyPair = generateRsaKeyPair(KEY_LENGTH);
        X509Certificate serverCert = generateServerCertificate(caKeyPair, caCert, serverKeyPair);

        // PEM outputs
        writePemPrivateKey(outDir.resolve(FILE_NAME_CA_PEM_PRIVATE_KEY), caKeyPair.getPrivate());
        writePemCertificate(outDir.resolve(FILE_NAME_CA_PEM_PUBLIC_KEY), caCert);
        writePemPrivateKey(outDir.resolve(FILE_NAME_PEM_PUBLIC_KEY), serverKeyPair.getPrivate());
        writePemCertificate(outDir.resolve(FILE_NAME_PEM_PRIVATE_KEY), serverCert);

        // DER outputs
        byte[] serverKeyDer = serverKeyPair.getPrivate().getEncoded();
        byte[] serverCertDer = serverCert.getEncoded();
        Files.write(outDir.resolve(FILE_NAME_PEM_DER_PUBLIC_KEY), serverKeyDer);
        Files.write(outDir.resolve(FILE_NAME_PEM_DER_PRIVATE_KEY), serverCertDer);

        // Truststore
        createOrUpdateTruststore(
                outDir.resolve("esp32-truststore.jks"),
                "changeit".toCharArray(),
                "my-esp32-ca",
                caCert
        );

        // C headers
        writeCHdr(outDir.resolve("cert.h"), "CERT_H_", "example_crt_der", serverCertDer);
        writeCHdr(outDir.resolve("private_key.h"), "PRIVATE_KEY_H_", "example_key_der", serverKeyDer);
    }

    // ========================== crypto helpers ==========================

    private KeyPair generateRsaKeyPair(int bits) throws NoSuchAlgorithmException {
        KeyPairGenerator kpg = KeyPairGenerator.getInstance("RSA");
        kpg.initialize(bits, new SecureRandom());
        return kpg.generateKeyPair();
    }

    private X509Certificate generateCaCertificate(KeyPair caKeyPair) throws Exception {
        X500Name subject = new X500Name("C=DE,ST=BE,L=Berlin,O=MyCompany,CN=myca.local");

        Instant now = Instant.now();
        Date notBefore = Date.from(now.minus(1, ChronoUnit.DAYS));
        Date notAfter = Date.from(now.plus(3650, ChronoUnit.DAYS));

        BigInteger serial = new BigInteger(64, new SecureRandom());

        X509v3CertificateBuilder certBuilder = new JcaX509v3CertificateBuilder(
                subject,
                serial,
                notBefore,
                notAfter,
                subject,
                caKeyPair.getPublic()
        );

        JcaX509ExtensionUtils extUtils = new JcaX509ExtensionUtils();

        certBuilder.addExtension(
                Extension.basicConstraints,
                true,
                new BasicConstraints(0)
        );
        certBuilder.addExtension(
                Extension.keyUsage,
                true,
                new KeyUsage(KeyUsage.keyCertSign | KeyUsage.cRLSign)
        );
        certBuilder.addExtension(
                Extension.subjectKeyIdentifier,
                false,
                extUtils.createSubjectKeyIdentifier(caKeyPair.getPublic())
        );
        certBuilder.addExtension(
                Extension.authorityKeyIdentifier,
                false,
                extUtils.createAuthorityKeyIdentifier(caKeyPair.getPublic())
        );

        ContentSigner signer = new JcaContentSignerBuilder(SIGNATURE_ALG)
                .setProvider(BC)
                .build(caKeyPair.getPrivate());

        X509CertificateHolder holder = certBuilder.build(signer);
        return new JcaX509CertificateConverter()
                .setProvider(BC)
                .getCertificate(holder);
    }

    private X509Certificate generateServerCertificate(KeyPair caKeyPair,
                                                      X509Certificate caCert,
                                                      KeyPair serverKeyPair) throws Exception {
        X500Name issuer = new X500Name(caCert.getSubjectX500Principal().getName());
        X500Name subject = new X500Name("C=DE,ST=BE,L=Berlin,O=MyCompany,CN=esp32.local");

        Instant now = Instant.now();
        Date notBefore = Date.from(now.minus(1, ChronoUnit.DAYS));
        Date notAfter = Date.from(now.plus(3650, ChronoUnit.DAYS));

        BigInteger serial = new BigInteger(64, new SecureRandom());

        X509v3CertificateBuilder certBuilder = new JcaX509v3CertificateBuilder(
                issuer,
                serial,
                notBefore,
                notAfter,
                subject,
                serverKeyPair.getPublic()
        );

        JcaX509ExtensionUtils extUtils = new JcaX509ExtensionUtils();

        certBuilder.addExtension(Extension.basicConstraints, true, new BasicConstraints(false));
        certBuilder.addExtension(
                Extension.keyUsage,
                true,
                new KeyUsage(KeyUsage.digitalSignature | KeyUsage.keyEncipherment)
        );
        certBuilder.addExtension(
                Extension.extendedKeyUsage,
                false,
                new ExtendedKeyUsage(new KeyPurposeId[]{
                        KeyPurposeId.id_kp_serverAuth,
                        KeyPurposeId.id_kp_clientAuth
                })
        );

        GeneralName[] altNames = new GeneralName[]{
                new GeneralName(GeneralName.dNSName, "esp32.local"),
                new GeneralName(GeneralName.dNSName, "myesp")
        };
        certBuilder.addExtension(
                Extension.subjectAlternativeName,
                false,
                new GeneralNames(altNames)
        );

        certBuilder.addExtension(
                Extension.subjectKeyIdentifier,
                false,
                extUtils.createSubjectKeyIdentifier(serverKeyPair.getPublic())
        );
        certBuilder.addExtension(
                Extension.authorityKeyIdentifier,
                false,
                extUtils.createAuthorityKeyIdentifier(caCert.getPublicKey())
        );

        ContentSigner signer = new JcaContentSignerBuilder(SIGNATURE_ALG)
                .setProvider(BC)
                .build(caKeyPair.getPrivate());

        X509CertificateHolder holder = certBuilder.build(signer);
        X509Certificate cert = new JcaX509CertificateConverter()
                .setProvider(BC)
                .getCertificate(holder);

        cert.verify(caCert.getPublicKey());
        return cert;
    }

    private void createOrUpdateTruststore(Path jksPath,
                                          char[] password,
                                          String alias,
                                          X509Certificate caCert) throws Exception {
        KeyStore ks = KeyStore.getInstance("JKS");
        if (Files.exists(jksPath)) {
            try (var in = Files.newInputStream(jksPath)) {
                ks.load(in, password);
            }
        } else {
            ks.load(null, null);
        }

        ks.setCertificateEntry(alias, caCert);

        try (FileOutputStream fos = new FileOutputStream(jksPath.toFile())) {
            ks.store(fos, password);
        }
    }

    private void writePemCertificate(Path path, X509Certificate cert) throws Exception {
        byte[] der = cert.getEncoded();
        writePem(path, "CERTIFICATE", der);
    }

    private void writePemPrivateKey(Path path, PrivateKey key) throws Exception {
        byte[] der = key.getEncoded();
        writePem(path, "PRIVATE KEY", der);
    }

    private void writePem(Path path, String type, byte[] der) throws IOException {
        String base64 = Base64.getEncoder().encodeToString(der);
        StringBuilder sb = new StringBuilder();
        sb.append("-----BEGIN ").append(type).append("-----\n");

        int index = 0;
        while (index < base64.length()) {
            int end = Math.min(index + 64, base64.length());
            sb.append(base64, index, end).append("\n");
            index = end;
        }

        sb.append("-----END ").append(type).append("-----\n");
        Files.writeString(path, sb.toString(), StandardCharsets.US_ASCII);
    }

    private void writeCHdr(Path path,
                           String headerGuard,
                           String varName,
                           byte[] data) throws IOException {
        StringBuilder sb = new StringBuilder();
        sb.append("#ifndef ").append(headerGuard).append("\n");
        sb.append("#define ").append(headerGuard).append("\n\n");

        sb.append("unsigned char ").append(varName).append("[] = {\n");
        for (int i = 0; i < data.length; i++) {
            if (i % 12 == 0) {
                sb.append("  ");
            }
            sb.append(String.format("0x%02x", data[i] & 0xff));
            if (i < data.length - 1) {
                sb.append(", ");
            }
            if (i % 12 == 11 || i == data.length - 1) {
                sb.append("\n");
            }
        }
        sb.append("};\n\n");
        sb.append("#endif\n");

        Files.writeString(path, sb.toString(), StandardCharsets.UTF_8);
    }

    // Optional helper for tests
    public X509Certificate readPemCert(Path pemPath) throws Exception {
        try (var in = Files.newInputStream(pemPath)) {
            CertificateFactory cf = CertificateFactory.getInstance("X.509");
            Certificate cert = cf.generateCertificate(in);
            return (X509Certificate) cert;
        }
    }
}
