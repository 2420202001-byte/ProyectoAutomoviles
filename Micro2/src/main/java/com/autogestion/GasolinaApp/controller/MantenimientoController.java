package com.autogestion.GasolinaApp.controller;

import com.autogestion.GasolinaApp.dto.MantenimientoDTO;
import com.autogestion.GasolinaApp.exception.ResourceNotFoundException;
import com.autogestion.GasolinaApp.model.AutomovilGasolina;
import com.autogestion.GasolinaApp.model.Mantenimiento;
import com.autogestion.GasolinaApp.repository.AutomovilGasolinaRepository;
import com.autogestion.GasolinaApp.repository.MantenimientoRepository;
import com.autogestion.GasolinaApp.service.AutomovilGasolinaService;
import com.autogestion.GasolinaApp.service.MantenimientoService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/mantenimientos")
public class MantenimientoController {

    private final MantenimientoService servicio;
    private final AutomovilGasolinaService servicioGasolina;

    @Autowired
    public MantenimientoController(MantenimientoRepository repositorio,
                                   AutomovilGasolinaRepository repositorioGasolina) {
        this.servicio = MantenimientoService.getInstancia();
        this.servicio.setRepositorio(repositorio);
        this.servicioGasolina = AutomovilGasolinaService.getInstancia();
        this.servicioGasolina.setRepositorio(repositorioGasolina);
    }

    private MantenimientoDTO toDTO(Mantenimiento m) {
        String idAuto = null;
        String marcaAuto = null;
        String modeloAuto = null;
        if (m.getAutomovilGasolina() != null) {
            idAuto = m.getAutomovilGasolina().getId();
            marcaAuto = m.getAutomovilGasolina().getMarca();
            modeloAuto = m.getAutomovilGasolina().getModelo();
        }
        return new MantenimientoDTO(
            m.getIdMantenimiento(), m.getTipo(), m.getDescripcion(),
            m.getCosto(), m.getFechaMantenimiento(), m.getKilometraje(),
            idAuto, marcaAuto, modeloAuto
        );
    }

    private Mantenimiento enriquecerDatosDelAuto(Mantenimiento m) {
        if (m == null || m.getAutomovilGasolina() == null || m.getAutomovilGasolina().getId() == null) {
            return m;
        }

        if (servicioGasolina == null) {
            return m;
        }

        AutomovilGasolina auto = servicioGasolina.buscar(m.getAutomovilGasolina().getId());
        if (auto != null) {
            m.setAutomovilGasolina(auto);
        }
        return m;
    }

    @GetMapping("/healthCheck")
    public String healthCheck() {
        return "Servicio Mantenimiento Ok!";
    }

    @PostMapping("/")
    public ResponseEntity<MantenimientoDTO> agregar(@RequestBody Mantenimiento m) {
        if (m == null) throw new IllegalArgumentException("El cuerpo de la solicitud no puede ser nulo");

        if (m.getAutomovilGasolina() == null || m.getAutomovilGasolina().getId() == null) {
            throw new IllegalArgumentException("Debe especificar un automóvil a gasolina válido");
        }

        AutomovilGasolina auto = servicioGasolina.buscar(m.getAutomovilGasolina().getId());
        if (auto == null) {
            throw new ResourceNotFoundException("El Automovil a Gasolina", "ID", m.getAutomovilGasolina().getId());
        }

        m.setAutomovilGasolina(auto);
        servicio.agregar(m);
        return ResponseEntity.ok(toDTO(m));
    }

    @GetMapping("/{id}")
    public ResponseEntity<MantenimientoDTO> buscar(@PathVariable("id") String id) {
        Mantenimiento m = servicio.buscarConAuto(id);
        if (m == null) throw new ResourceNotFoundException("Mantenimiento", "ID", id);
        return ResponseEntity.ok(toDTO(m));
    }

    @PutMapping("/{id}")
    public ResponseEntity<MantenimientoDTO> actualizar(@PathVariable("id") String id,
                                                        @RequestBody Mantenimiento m) {
        boolean ok = servicio.actualizar(id, m);
        if (!ok) throw new ResourceNotFoundException("Mantenimiento", "ID", id);
        return ResponseEntity.ok(toDTO(enriquecerDatosDelAuto(m)));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> eliminar(@PathVariable("id") String id) {
        boolean ok = servicio.eliminar(id);
        if (!ok) throw new ResourceNotFoundException("Mantenimiento", "ID", id);
        return ResponseEntity.ok("Mantenimiento eliminado correctamente");
    }

    @GetMapping("/")
    public ResponseEntity<List<MantenimientoDTO>> listar() {
        List<MantenimientoDTO> lista = servicio.listar().stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/filtrar")
    public ResponseEntity<List<MantenimientoDTO>> filtrarPorTipo(
            @RequestParam String tipo) {
        List<MantenimientoDTO> lista = servicio.findByTipo(tipo).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }

    @GetMapping("/auto")
    public ResponseEntity<List<MantenimientoDTO>> filtrarPorAuto(
            @RequestParam String idAuto) {
        List<MantenimientoDTO> lista = servicio.findByAutomovilGasolina(idAuto).stream()
                .map(this::toDTO)
                .collect(Collectors.toList());
        return ResponseEntity.ok(lista);
    }
}