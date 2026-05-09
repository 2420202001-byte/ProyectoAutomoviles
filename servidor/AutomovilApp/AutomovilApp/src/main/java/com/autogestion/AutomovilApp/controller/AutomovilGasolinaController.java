package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.model.AutomovilGasolina;
import com.autogestion.AutomovilApp.model.AutomovilGasolinaBuilder;
import com.autogestion.AutomovilApp.repository.AutomovilGasolinaRepository;
import com.autogestion.AutomovilApp.service.AutomovilGasolinaService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/gasolina")
public class AutomovilGasolinaController {

    private final AutomovilGasolinaService servicio;

    @Autowired
    public AutomovilGasolinaController(AutomovilGasolinaRepository repositorio) {
        this.servicio = AutomovilGasolinaService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Gasolina Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilGasolina> agregar(@RequestBody AutomovilGasolina ag) {
        if (ag == null) return ResponseEntity.noContent().build();
        AutomovilGasolina nuevo = new AutomovilGasolinaBuilder()
                .setId(ag.getId())
                .setMarca(ag.getMarca())
                .setModelo(ag.getModelo())
                .setAnio(ag.getAnio())
                .setColor(ag.getColor())
                .setPrecio(ag.getPrecio())
                .setConsumoLitrosPor100Km(ag.getConsumoLitrosPor100Km())
                .setCapacidadTanqueLitros(ag.getCapacidadTanqueLitros())
                .setCilindraje(ag.getCilindraje())
                .setTipoCombustible(ag.getTipoCombustible())
                .setTransmision(ag.getTransmision())
                .build();
        servicio.agregar(nuevo);
        return ResponseEntity.ok(nuevo);
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
        return ResponseEntity.ok(servicio.filtrar(marca, anio));
    }

    @GetMapping("/combustible")
    public ResponseEntity<List<AutomovilGasolina>> filtrarPorCombustible(
            @RequestParam String tipoCombustible) {
        return ResponseEntity.ok(servicio.findByTipoCombustible(tipoCombustible));
    }
}