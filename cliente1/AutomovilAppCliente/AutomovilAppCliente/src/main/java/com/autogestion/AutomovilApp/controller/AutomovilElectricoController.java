package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.model.AutomovilElectrico;
import com.autogestion.AutomovilApp.service.AutomovilElectricoService;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/electricos")
public class AutomovilElectricoController {

    private final AutomovilElectricoService servicio = AutomovilElectricoService.getInstancia();

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Eléctrico Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilElectrico> agregar(@RequestBody AutomovilElectrico ae) {
        if (ae == null) return ResponseEntity.noContent().build();
        servicio.agregar(ae);
        return ResponseEntity.ok(ae);
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilElectrico> buscar(@PathVariable("id") String id) {
        AutomovilElectrico ae = servicio.buscar(id);
        if (ae == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(ae);
    }

    @PutMapping("/{id}")
    public ResponseEntity<AutomovilElectrico> actualizar(@PathVariable("id") String id,
                                                          @RequestBody AutomovilElectrico ae) {
        boolean ok = servicio.actualizar(id, ae);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(ae);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) return ResponseEntity.notFound().build();
        return ResponseEntity.ok("Eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<AutomovilElectrico>> listar() {
        return ResponseEntity.ok(servicio.listar());
    }


    @GetMapping("/filtrar")
public ResponseEntity<List<AutomovilElectrico>> filtrar(
        @RequestParam(required = false) String marca,
        @RequestParam(required = false) Integer anio) {
    List<AutomovilElectrico> resultado = servicio.listar().stream()
            .filter(a -> marca == null || a.getMarca().equalsIgnoreCase(marca))
            .filter(a -> anio == null || a.getAnio() == anio)
            .toList();
    return ResponseEntity.ok(resultado);
}
}