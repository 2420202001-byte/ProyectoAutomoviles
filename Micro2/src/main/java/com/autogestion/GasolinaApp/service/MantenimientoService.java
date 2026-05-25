package com.autogestion.GasolinaApp.service;

import com.autogestion.GasolinaApp.model.Mantenimiento;
import com.autogestion.GasolinaApp.repository.MantenimientoRepository;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public class MantenimientoService {

    private static MantenimientoService instancia;
    private MantenimientoRepository repositorio;

    public MantenimientoService() {}

    public static MantenimientoService getInstancia() {
        if (instancia == null)
            instancia = new MantenimientoService();
        return instancia;
    }

    public void setRepositorio(MantenimientoRepository repositorio) {
        this.repositorio = repositorio;
    }

    public void agregar(Mantenimiento m) {
        repositorio.save(m);
    }

    public Mantenimiento buscar(String id) {
        Optional<Mantenimiento> result = repositorio.findById(id);
        return result.orElse(null);
    }

    public Mantenimiento buscarConAuto(String id) {
        return repositorio.findByIdConAuto(id);
    }

    public boolean actualizar(String id, Mantenimiento nuevo) {
        if (!repositorio.existsById(id)) return false;
        nuevo.setIdMantenimiento(id);
        repositorio.save(nuevo);
        return true;
    }

    public boolean eliminar(String id) {
        if (!repositorio.existsById(id)) return false;
        repositorio.deleteById(id);
        return true;
    }

    public List<Mantenimiento> listar() {
        return repositorio.findAll();
    }

    public List<Mantenimiento> findByTipo(String tipo) {
        return repositorio.findByTipo(tipo);
    }

    public List<Mantenimiento> findByAutomovilGasolina(String idAuto) {
        return repositorio.findByAutomovilGasolina(idAuto);
    }
}