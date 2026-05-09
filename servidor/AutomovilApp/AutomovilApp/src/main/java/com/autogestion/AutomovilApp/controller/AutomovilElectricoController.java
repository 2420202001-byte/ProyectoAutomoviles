package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.model.AutomovilElectrico;
import com.autogestion.AutomovilApp.model.AutomovilElectricoBuilder;
import com.autogestion.AutomovilApp.repository.AutomovilElectricoRepository;
import com.autogestion.AutomovilApp.service.AutomovilElectricoService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/electricos")
public class AutomovilElectricoController {

    private final AutomovilElectricoService servicio;

    @Autowired
    public AutomovilElectricoController(AutomovilElectricoRepository repositorio) {
        this.servicio = AutomovilElectricoService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Eléctrico Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilElectrico> agregar(@RequestBody AutomovilElectrico ae) {
        if (ae == null) return ResponseEntity.noContent().build();
        AutomovilElectrico nuevo = new AutomovilElectricoBuilder()
                .setId(ae.getId())
                .setMarca(ae.getMarca())
                .setModelo(ae.getModelo())
                .setAnio(ae.getAnio())
                .setColor(ae.getColor())
                .setPrecio(ae.getPrecio())
                .setAutonomiaKm(ae.getAutonomiaKm())
                .setTiempoCargaHoras(ae.getTiempoCargaHoras())
                .setBateria(ae.getBateria())
                .build();
        servicio.agregar(nuevo);
        return ResponseEntity.ok(nuevo);
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilElectrico> buscar(@PathVariable("id") String id) {
        AutomovilElectrico ae = servicio.buscarConBateria(id);
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
        return ResponseEntity.ok(servicio.filtrar(marca, anio));
    }
}