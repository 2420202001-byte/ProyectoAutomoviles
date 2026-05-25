package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.dto.MantenimientoDTO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.RestTemplate;

import java.util.List;

@RestController
@RequestMapping("/mantenimientos")
public class MantenimientoController {

    private final String MICRO2_URL = "http://localhost:8081/mantenimientos";

    @Autowired
    private RestTemplate restTemplate;

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Mantenimiento Ok! (proxy -> micro2)";
    }

    @PostMapping("/")
    public ResponseEntity<MantenimientoDTO> agregar(@RequestBody String body) {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        HttpEntity<String> entity = new HttpEntity<>(body, headers);
        ResponseEntity<MantenimientoDTO> response = restTemplate.exchange(
                MICRO2_URL + "/", HttpMethod.POST, entity, MantenimientoDTO.class);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/{id}")
    public ResponseEntity<MantenimientoDTO> buscar(@PathVariable String id) {
        MantenimientoDTO dto = restTemplate.getForObject(MICRO2_URL + "/" + id, MantenimientoDTO.class);
        return ResponseEntity.ok(dto);
    }

    @PutMapping("/{id}")
    public ResponseEntity<MantenimientoDTO> actualizar(@PathVariable String id, @RequestBody Object body) {
        HttpEntity<Object> entity = new HttpEntity<>(body);
        ResponseEntity<MantenimientoDTO> response = restTemplate.exchange(
                MICRO2_URL + "/" + id, HttpMethod.PUT, entity, MantenimientoDTO.class);
        return ResponseEntity.ok(response.getBody());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable String id) {
        ResponseEntity<String> response = restTemplate.exchange(
                MICRO2_URL + "/" + id, HttpMethod.DELETE, null, String.class);
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/")
    public ResponseEntity<List<MantenimientoDTO>> listar() {
        ResponseEntity<List<MantenimientoDTO>> response = restTemplate.exchange(
                MICRO2_URL + "/", HttpMethod.GET, null,
                new ParameterizedTypeReference<List<MantenimientoDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<MantenimientoDTO>> filtrarPorTipo(@RequestParam String tipo) {
        String url = MICRO2_URL + "/filtrar?tipo=" + tipo;
        ResponseEntity<List<MantenimientoDTO>> response = restTemplate.exchange(
                url, HttpMethod.GET, null,
                new ParameterizedTypeReference<List<MantenimientoDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/auto")
    public ResponseEntity<List<MantenimientoDTO>> filtrarPorAuto(@RequestParam String idAuto) {
        String url = MICRO2_URL + "/auto?idAuto=" + idAuto;
        ResponseEntity<List<MantenimientoDTO>> response = restTemplate.exchange(
                url, HttpMethod.GET, null,
                new ParameterizedTypeReference<List<MantenimientoDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }
}