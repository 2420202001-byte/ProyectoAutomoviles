package com.autogestion.AutomovilApp.service;

import com.autogestion.AutomovilApp.model.AutomovilElectrico;
import com.autogestion.AutomovilApp.repository.AutomovilElectricoRepository;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public class AutomovilElectricoService {

    private static AutomovilElectricoService instancia;
    private AutomovilElectricoRepository repositorio;

    public AutomovilElectricoService() {}

    public AutomovilElectricoService(AutomovilElectricoRepository repositorio) {
        this.repositorio = repositorio;
    }

    public static AutomovilElectricoService getInstancia() {
        if (instancia == null) {
            instancia = new AutomovilElectricoService();
        }
        return instancia;
    }

    public void setRepositorio(AutomovilElectricoRepository repositorio) {
        this.repositorio = repositorio;
    }

    public void agregar(AutomovilElectrico ae) {
        repositorio.save(ae);
    }

    public AutomovilElectrico buscar(String id) {
        Optional<AutomovilElectrico> result = repositorio.findById(id);
        return result.orElse(null);
    }

    public AutomovilElectrico buscarConBateria(String id) {
        return repositorio.findByIdConBateria(id);
    }

    public boolean actualizar(String id, AutomovilElectrico nuevo) {
        if (!repositorio.existsById(id)) return false;
        nuevo.setId(id);
        repositorio.save(nuevo);
        return true;
    }

    public boolean eliminar(String id) {
        if (!repositorio.existsById(id)) return false;
        repositorio.deleteById(id);
        return true;
    }

    public List<AutomovilElectrico> listar() {
        return repositorio.findAll();
    }

    public List<AutomovilElectrico> filtrar(String marca, Integer anio) {
        return repositorio.filtrar(marca, anio);
    }
}