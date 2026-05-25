package com.autogestion.AutomovilApp.service;

import java.util.ArrayList;
import java.util.List;

import com.autogestion.AutomovilApp.model.AutomovilElectrico;

public class AutomovilElectricoService {

    private static AutomovilElectricoService instancia;
    private final List<AutomovilElectrico> coleccion;

    private AutomovilElectricoService() {
        coleccion = new ArrayList<>();
    }

    public static AutomovilElectricoService getInstancia() {
        if (instancia == null) {
            instancia = new AutomovilElectricoService();
        }
        return instancia;
    }

    public void agregar(AutomovilElectrico ae) {
        coleccion.add(ae);
    }

    public AutomovilElectrico buscar(String id) {
        return coleccion.stream()
                .filter(a -> a.getId().equalsIgnoreCase(id))
                .findFirst().orElse(null);
    }

    public boolean actualizar(String id, AutomovilElectrico nuevo) {
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

    public List<AutomovilElectrico> listar() {
        return new ArrayList<>(coleccion);
    }
}