import org.gradle.kotlin.dsl.testImplementation
import java.time.Duration


plugins {
    java
    id("org.springframework.boot") version "3.5.7"
    id("io.spring.dependency-management") version "1.1.7"
    id("com.avast.gradle.docker-compose") version "0.17.10"
}

group = "de.pschiessle.artnet"
version = "0.0.1-SNAPSHOT"
description = "Inventory System for Event"

java {
    toolchain {
        languageVersion = JavaLanguageVersion.of(21)
    }
}

configurations {
    compileOnly {
        extendsFrom(configurations.annotationProcessor.get())
    }
}

repositories {
    mavenCentral()
}

dependencies {

    implementation ("org.springframework.boot:spring-boot-starter-actuator")
    implementation ("org.springframework.boot:spring-boot-starter-data-jpa")
    implementation("org.springframework.boot:spring-boot-starter-thymeleaf")
    implementation("org.springframework.boot:spring-boot-starter-web")
    implementation ("org.springframework.boot:spring-boot-starter-data-redis")

    implementation("org.springframework.boot:spring-boot-starter-websocket")
    implementation("org.springframework.boot:spring-boot-starter-security")
    implementation("org.springframework.boot:spring-boot-starter-oauth2-resource-server")

    implementation("org.postgresql:postgresql:42.7.8")
    implementation("org.bouncycastle:bcprov-jdk18on:1.79")
    implementation("org.bouncycastle:bcpkix-jdk18on:1.79")

    testImplementation("com.h2database:h2")

    compileOnly("org.projectlombok:lombok")
    annotationProcessor("org.projectlombok:lombok")
    testImplementation("org.springframework.boot:spring-boot-starter-test")
    developmentOnly("org.springframework.boot:spring-boot-devtools")
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
}

tasks.withType<Test> {
    useJUnitPlatform()
}

