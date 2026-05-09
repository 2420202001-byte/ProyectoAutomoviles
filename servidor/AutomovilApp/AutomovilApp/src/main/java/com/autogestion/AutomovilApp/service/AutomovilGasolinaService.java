package com.autogestion.AutomovilApp.service;

import com.autogestion.AutomovilApp.model.AutomovilGasolina;
import com.autogestion.AutomovilApp.repository.AutomovilGasolinaRepository;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public class AutomovilGasolinaService {

    private static AutomovilGasolinaService instancia;
    private AutomovilGasolinaRepository repositorio;

    public AutomovilGasolinaService() {}

    public AutomovilGasolinaService(AutomovilGasolinaRepository repositorio) {
        this.repositorio = repositorio;
    }

    public static AutomovilGasolinaService getInstancia() {
        if (instancia == null) {
            instancia = new AutomovilGasolinaService();
        }
        return instancia;
    }

    public void setRepositorio(AutomovilGasolinaRepository repositorio) {
        this.repositorio = repositorio;
    }

    public void agregar(AutomovilGasolina ag) {
        repositorio.save(ag);
    }

    public AutomovilGasolina buscar(String id) {
        Optional<AutomovilGasolina> result = repositorio.findById(id);
        return result.orElse(null);
    }

    public boolean actualizar(String id, AutomovilGasolina nuevo) {
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

    public List<AutomovilGasolina> listar() {
        return repositorio.findAll();
    }

    public List<AutomovilGasolina> filtrar(String marca, Integer anio) {
        return repositorio.filtrar(marca, anio);
    }

    public List<AutomovilGasolina> findByTipoCombustible(String tipoCombustible) {
        return repositorio.findByTipoCombustible(tipoCombustible);
    }
}