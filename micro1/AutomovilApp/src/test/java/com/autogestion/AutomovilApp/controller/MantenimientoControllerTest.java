package com.autogestion.AutomovilApp.controller;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
class MantenimientoControllerTest {

    @Autowired
    private MantenimientoController controller;

    @Test
    void healthCheckDebeResponderOk() {
        assertThat(controller.healthCheck())
                .isEqualTo("Servicio Mantenimiento Ok! (proxy -> micro2)");
    }
}
