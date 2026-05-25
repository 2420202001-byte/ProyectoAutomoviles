package com.autogestion.AutomovilApp.controller;

import com.autogestion.AutomovilApp.dto.AutomovilElectricoDTO;
import com.autogestion.AutomovilApp.dto.BateriaDTO;
import com.autogestion.AutomovilApp.exception.ResourceNotFoundException;
import com.autogestion.AutomovilApp.model.AutomovilElectrico;
import com.autogestion.AutomovilApp.model.AutomovilElectricoBuilder;
import com.autogestion.AutomovilApp.repository.AutomovilElectricoRepository;
import com.autogestion.AutomovilApp.service.AutomovilElectricoService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/electricos")
public class AutomovilElectricoController {

    private final AutomovilElectricoService servicio;

    @Autowired
    public AutomovilElectricoController(AutomovilElectricoRepository repositorio) {
        this.servicio = AutomovilElectricoService.getInstancia();
        this.servicio.setRepositorio(repositorio);
    }

    private AutomovilElectricoDTO toDTO(AutomovilElectrico ae) {
        BateriaDTO bateriaDTO = null;
        if (ae.getBateria() != null) {
            bateriaDTO = new BateriaDTO(
                ae.getBateria().getIdBateria(),
                ae.getBateria().getMarca(),
                ae.getBateria().getCapacidadKwh(),
                ae.getBateria().getCiclosVida(),
                ae.getBateria().getVoltaje()
            );
        }
        return new AutomovilElectricoDTO(
            ae.getId(), ae.getMarca(), ae.getModelo(), ae.getAnio(),
            ae.getColor(), ae.getPrecio(), ae.getFechaRegistro(),
            ae.getAutonomiaKm(), ae.getTiempoCargaHoras(), bateriaDTO
        );
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Automóvil Eléctrico Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<AutomovilElectricoDTO> agregar(@RequestBody AutomovilElectrico ae) {
        if (ae == null) throw new IllegalArgumentException("El cuerpo de la solicitud no puede ser nulo");
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
        return ResponseEntity.ok(toDTO(nuevo));
    }

    @GetMapping("/{id}")
    public ResponseEntity<AutomovilElectricoDTO> buscar(@PathVariable("id") String id) {
        AutomovilElectrico ae = servicio.buscarConBateria(id);
        if (ae == null) throw new ResourceNotFoundException("Automóvil Eléctrico", "ID", id);
        return ResponseEntity.ok(toDTO(ae));
    }

    @PutMapping("/{id}")
    public ResponseEntity<AutomovilElectricoDTO> actualizar(@PathVariable("id") String id,
                                                             @RequestBody AutomovilElectrico ae) {
        boolean ok = servicio.actualizar(id, ae);
        if (!ok) throw new ResourceNotFoundException("Automóvil Eléctrico", "ID", id);
        return ResponseEntity.ok(toDTO(ae));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) throw new ResourceNotFoundException("Automóvil Eléctrico", "ID", id);
        return ResponseEntity.ok("Automóvil eléctrico eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<AutomovilElectricoDTO>> listar() {
        List<AutomovilElectricoDTO> lista = servicio.listar().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<AutomovilElectricoDTO>> filtrar(
            @RequestParam(required = false) String marca,
            @RequestParam(required = false) Integer anio) {
        List<AutomovilElectricoDTO> lista = servicio.filtrar(marca, anio).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }
}