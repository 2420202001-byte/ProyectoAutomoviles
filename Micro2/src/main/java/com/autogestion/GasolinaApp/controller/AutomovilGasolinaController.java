package com.autogestion.GasolinaApp.controller;

import com.autogestion.GasolinaApp.dto.AutomovilGasolinaDTO;
import com.autogestion.GasolinaApp.exception.ResourceNotFoundException;
import com.autogestion.GasolinaApp.model.AutomovilGasolina;
import com.autogestion.GasolinaApp.model.AutomovilGasolinaBuilder;
import com.autogestion.GasolinaApp.repository.AutomovilGasolinaRepository;
import com.autogestion.GasolinaApp.service.AutomovilGasolinaService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/gasolina")
public class AutomovilGasolinaController {

    private final AutomovilGasolinaService servicio;

    @Autowired
    public AutomovilGasolinaController(AutomovilGasolinaRepository repositorio) {
        this.servicio = AutomovilGasolinaService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    private AutomovilGasolinaDTO toDTO(AutomovilGasolina ag) {
        return new AutomovilGasolinaDTO(
            ag.getId(), ag.getMarca(), ag.getModelo(), ag.getAnio(),
            ag.getColor(), ag.getPrecio(), ag.getFechaRegistro(),
            ag.getConsumoLitrosPor100Km(), ag.getCapacidadTanqueLitros(),
            ag.getCilindraje(), ag.getTipoCombustible(), ag.getTransmision()
        );
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Gasolina Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilGasolinaDTO> agregar(@RequestBody AutomovilGasolina ag) {
        if (ag == null) throw new IllegalArgumentException("El cuerpo de la solicitud no puede ser nulo");
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
        return ResponseEntity.ok(toDTO(nuevo));
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilGasolinaDTO> buscar(@PathVariable("id") String id) {
        AutomovilGasolina ag = servicio.buscar(id);
        if (ag == null) throw new ResourceNotFoundException("Automóvil Gasolina", "ID", id);
        return ResponseEntity.ok(toDTO(ag));
    }

    @PutMapping("/{id}")
    public ResponseEntity<AutomovilGasolinaDTO> actualizar(@PathVariable("id") String id,
                                                            @RequestBody AutomovilGasolina ag) {
        boolean ok = servicio.actualizar(id, ag);
        if (!ok) throw new ResourceNotFoundException("Automóvil Gasolina", "ID", id);
        return ResponseEntity.ok(toDTO(ag));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) throw new ResourceNotFoundException("Automóvil Gasolina", "ID", id);
        return ResponseEntity.ok("Automóvil a gasolina eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<AutomovilGasolinaDTO>> listar() {
        List<AutomovilGasolinaDTO> lista = servicio.listar().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<AutomovilGasolinaDTO>> filtrar(
            @RequestParam(required = false) String marca,
            @RequestParam(required = false) Integer anio) {
        List<AutomovilGasolinaDTO> lista = servicio.filtrar(marca, anio).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/combustible")
    public ResponseEntity<List<AutomovilGasolinaDTO>> filtrarPorCombustible(
            @RequestParam String tipoCombustible) {
        List<AutomovilGasolinaDTO> lista = servicio.findByTipoCombustible(tipoCombustible).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }
}