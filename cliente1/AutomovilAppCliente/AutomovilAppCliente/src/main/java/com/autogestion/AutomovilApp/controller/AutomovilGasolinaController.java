package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.model.AutomovilGasolina;
import com.autogestion.AutomovilApp.service.AutomovilGasolinaService;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/gasolina")
public class AutomovilGasolinaController {

    private final AutomovilGasolinaService servicio = AutomovilGasolinaService.getInstancia();

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Gasolina Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilGasolina> agregar(@RequestBody AutomovilGasolina ag) {
        if (ag == null) return ResponseEntity.noContent().build();
        servicio.agregar(ag);
        return ResponseEntity.ok(ag);
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilGasolina> buscar(@PathVariable("id") String id) {
        AutomovilGasolina ag = servicio.buscar(id);
        if (ag == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(ag);
    }

    @PutMapping("/{id}")
    public ResponseEntity<AutomovilGasolina> actualizar(@PathVariable("id") String id,
                                                         @RequestBody AutomovilGasolina ag) {
        boolean ok = servicio.actualizar(id, ag);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(ag);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok("Eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<AutomovilGasolina>> listar() {
        return ResponseEntity.ok(servicio.listar());
    }

    
@GetMapping("/filtrar")
public ResponseEntity<List<AutomovilGasolina>> filtrar(
        @RequestParam(required = false) String marca,
        @RequestParam(required = false) Integer anio) {
    List<AutomovilGasolina> resultado = servicio.listar().stream()
            .filter(a -> marca == null || a.getMarca().equalsIgnoreCase(marca))
            .filter(a -> anio == null || a.getAnio() == anio)
            .toList();
    return ResponseEntity.ok(resultado);

        


        
        
    }

    
}