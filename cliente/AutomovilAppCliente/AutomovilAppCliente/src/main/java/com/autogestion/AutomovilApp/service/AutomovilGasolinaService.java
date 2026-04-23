package com.autogestion.AutomovilApp.service;

import java.util.ArrayList;
import java.util.List;

import com.autogestion.AutomovilApp.model.AutomovilGasolina;

public class AutomovilGasolinaService {

    private static AutomovilGasolinaService instancia;
    private final List<AutomovilGasolina> coleccion;

    private AutomovilGasolinaService() {
        coleccion = new ArrayList<>();
    }

    public static AutomovilGasolinaService getInstancia() {
        if (instancia == null) {
            instancia = new AutomovilGasolinaService();
        }
        return instancia;
    }

    public void agregar(AutomovilGasolina ag) {
        coleccion.add(ag);
    }

    public AutomovilGasolina buscar(String id) {
        return coleccion.stream()
                .filter(a -> a.getId().equalsIgnoreCase(id))
                .findFirst().orElse(null);
    }

    public boolean actualizar(String id, AutomovilGasolina nuevo) {
        for (int i = 0; i < coleccion.size(); i++) {
            if (coleccion.get(i).getId().equalsIgnoreCase(id)) {
                coleccion.set(i, nuevo);
                return true;
            }
        }
        return false;
    }

    public boolean eliminar(String id) {
        return coleccion.removeIf(a -> a.getId().equalsIgnoreCase(id));
    }

    public List<AutomovilGasolina> listar() {
        return new ArrayList<>(coleccion);
    }
}