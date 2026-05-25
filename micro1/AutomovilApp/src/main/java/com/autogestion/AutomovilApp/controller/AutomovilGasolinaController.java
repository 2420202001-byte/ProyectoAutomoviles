package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.dto.AutomovilGasolinaDTO;
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
@RequestMapping("/gasolina")
public class AutomovilGasolinaController {

    private final String MICRO2_URL = "http://localhost:8081/gasolina";

    @Autowired
    private RestTemplate restTemplate;

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Gasolina Ok! (proxy -> micro2)";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilGasolinaDTO> agregar(@RequestBody String body) {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        HttpEntity<String> entity = new HttpEntity<>(body, headers);
        ResponseEntity<AutomovilGasolinaDTO> response = restTemplate.exchange(
                MICRO2_URL + "/", HttpMethod.POST, entity, AutomovilGasolinaDTO.class);
        return ResponseEntity.status(response.getStatusCode()).body(response.getBody());
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilGasolinaDTO> buscar(@PathVariable String id) {
        AutomovilGasolinaDTO dto = restTemplate.getForObject(MICRO2_URL + "/" + id, AutomovilGasolinaDTO.class);
        return ResponseEntity.ok(dto);
    }

    @PutMapping("/{id}")
    public ResponseEntity<AutomovilGasolinaDTO> actualizar(@PathVariable String id, @RequestBody Object body) {
        HttpEntity<Object> entity = new HttpEntity<>(body);
        ResponseEntity<AutomovilGasolinaDTO> response = restTemplate.exchange(
                MICRO2_URL + "/" + id, HttpMethod.PUT, entity, AutomovilGasolinaDTO.class);
        return ResponseEntity.ok(response.getBody());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable String id) {
        ResponseEntity<String> response = restTemplate.exchange(
                MICRO2_URL + "/" + id, HttpMethod.DELETE, null, String.class);
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/")
    public ResponseEntity<List<AutomovilGasolinaDTO>> listar() {
        ResponseEntity<List<AutomovilGasolinaDTO>> response = restTemplate.exchange(
                MICRO2_URL + "/", HttpMethod.GET, null,
                new ParameterizedTypeReference<List<AutomovilGasolinaDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<AutomovilGasolinaDTO>> filtrar(
            @RequestParam(required = false) String marca,
            @RequestParam(required = false) Integer anio) {
        String url = MICRO2_URL + "/filtrar?marca=" + (marca != null ? marca : "")
                + "&anio=" + (anio != null ? anio : "");
        ResponseEntity<List<AutomovilGasolinaDTO>> response = restTemplate.exchange(
                url, HttpMethod.GET, null,
                new ParameterizedTypeReference<List<AutomovilGasolinaDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }

    @GetMapping("/combustible")
    public ResponseEntity<List<AutomovilGasolinaDTO>> filtrarPorCombustible(@RequestParam String tipoCombustible) {
        String url = MICRO2_URL + "/combustible?tipoCombustible=" + tipoCombustible;
        ResponseEntity<List<AutomovilGasolinaDTO>> response = restTemplate.exchange(
                url, HttpMethod.GET, null,
                new ParameterizedTypeReference<List<AutomovilGasolinaDTO>>() {});
        return ResponseEntity.ok(response.getBody());
    }
}