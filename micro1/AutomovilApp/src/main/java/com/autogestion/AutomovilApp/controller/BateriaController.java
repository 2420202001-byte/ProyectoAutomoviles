package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.dto.BateriaDTO;
import com.autogestion.AutomovilApp.exception.ResourceNotFoundException;
import com.autogestion.AutomovilApp.model.Bateria;
import com.autogestion.AutomovilApp.repository.BateriaRepository;
import com.autogestion.AutomovilApp.service.BateriaService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/baterias")
public class BateriaController {

    private final BateriaService servicio;

    @Autowired
    public BateriaController(BateriaRepository repositorio) {
        this.servicio = BateriaService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    private BateriaDTO toDTO(Bateria b) {
        return new BateriaDTO(
            b.getIdBateria(), b.getMarca(),
            b.getCapacidadKwh(), b.getCiclosVida(), b.getVoltaje()
        );
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Batería Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<BateriaDTO> agregar(@RequestBody Bateria b) {
        if (b == null) throw new IllegalArgumentException("El cuerpo de la solicitud no puede ser nulo");
        servicio.agregar(b);
        return ResponseEntity.ok(toDTO(b));
    }

    @GetMapping("/{id}")
    public ResponseEntity<BateriaDTO> buscar(@PathVariable("id") String id) {
        Bateria b = servicio.buscar(id);
        if (b == null) throw new ResourceNotFoundException("Batería", "ID", id);
        return ResponseEntity.ok(toDTO(b));
    }

    @PutMapping("/{id}")
    public ResponseEntity<BateriaDTO> actualizar(@PathVariable("id") String id,
                                                  @RequestBody Bateria b) {
        boolean ok = servicio.actualizar(id, b);
        if (!ok) throw new ResourceNotFoundException("Batería", "ID", id);
        return ResponseEntity.ok(toDTO(b));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) throw new ResourceNotFoundException("Batería", "ID", id);
        return ResponseEntity.ok("Batería eliminada correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<BateriaDTO>> listar() {
        List<BateriaDTO> lista = servicio.listar().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<BateriaDTO>> filtrarPorMarca(
            @RequestParam String marca) {
        List<BateriaDTO> lista = servicio.findByMarca(marca).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/capacidad")
    public ResponseEntity<List<BateriaDTO>> filtrarPorCapacidad(
            @RequestParam double capacidad) {
        List<BateriaDTO> lista = servicio.findByCapacidadMinima(capacidad).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }
}