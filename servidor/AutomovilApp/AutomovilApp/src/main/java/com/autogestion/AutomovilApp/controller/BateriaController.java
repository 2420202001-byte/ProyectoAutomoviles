package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.model.Bateria;
import com.autogestion.AutomovilApp.repository.BateriaRepository;
import com.autogestion.AutomovilApp.service.BateriaService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/baterias")
public class BateriaController {

    private final BateriaService servicio;

    @Autowired
    public BateriaController(BateriaRepository repositorio) {
        this.servicio = BateriaService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Batería Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<Bateria> agregar(@RequestBody Bateria b) {
        if (b == null) return ResponseEntity.noContent().build();
        servicio.agregar(b);
        return ResponseEntity.ok(b);
    }

    @GetMapping("/{id}")
    public ResponseEntity<Bateria> buscar(@PathVariable("id") String id) {
        Bateria b = servicio.buscar(id);
        if (b == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(b);
    }

    @PutMapping("/{id}")
    public ResponseEntity<Bateria> actualizar(@PathVariable("id") String id,
                                               @RequestBody Bateria b) {
        boolean ok = servicio.actualizar(id, b);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(b);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok("Eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<Bateria>> listar() {
        return ResponseEntity.ok(servicio.listar());
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<Bateria>> filtrarPorMarca(
            @RequestParam String marca) {
        return ResponseEntity.ok(servicio.findByMarca(marca));
    }

    @GetMapping("/capacidad")
    public ResponseEntity<List<Bateria>> filtrarPorCapacidad(
            @RequestParam double capacidad) {
        return ResponseEntity.ok(servicio.findByCapacidadMinima(capacidad));
    }
}