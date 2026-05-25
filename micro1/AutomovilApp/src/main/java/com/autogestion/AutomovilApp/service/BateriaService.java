package com.autogestion.AutomovilApp.service;

import com.autogestion.AutomovilApp.model.Bateria;
import com.autogestion.AutomovilApp.repository.BateriaRepository;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public class BateriaService {

    private static BateriaService instancia;
    private BateriaRepository repositorio;

    public BateriaService() {}

    public BateriaService(BateriaRepository repositorio) {
        this.repositorio = repositorio;
    }

    public static BateriaService getInstancia() {
        if (instancia == null) {
            instancia = new BateriaService();
        }
        return instancia;
    }

    public void setRepositorio(BateriaRepository repositorio) {
        this.repositorio = repositorio;
    }

    public void agregar(Bateria b) {
        repositorio.save(b);
    }

    public Bateria buscar(String id) {
        Optional<Bateria> result = repositorio.findById(id);
        return result.orElse(null);
    }

    public boolean actualizar(String id, Bateria nueva) {
        if (!repositorio.existsById(id)) return false;
        nueva.setIdBateria(id);
        repositorio.save(nueva);
        return true;
    }

    public boolean eliminar(String id) {
        if (!repositorio.existsById(id)) return false;
        repositorio.deleteById(id);
        return true;
    }

    public List<Bateria> listar() {
        return repositorio.findAll();
    }

    public List<Bateria> findByMarca(String marca) {
        return repositorio.findByMarca(marca);
    }

    public List<Bateria> findByCapacidadMinima(double capacidad) {
        return repositorio.findByCapacidadMinima(capacidad);
    }
}