package com.autogestion.AutomovilApp.service;

import java.util.ArrayList;
import java.util.List;

import com.autogestion.AutomovilApp.model.Bateria;

public class BateriaService {

    private static BateriaService instancia;
    private final List<Bateria> coleccion;

    private BateriaService() {
        coleccion = new ArrayList<>();
        coleccion.add(new Bateria("BAT001", "Tesla",    75.0,  1500, 400.0));
        coleccion.add(new Bateria("BAT002", "LG Chem",  60.0,  1200, 360.0));
        coleccion.add(new Bateria("BAT003", "Panasonic",100.0, 2000, 800.0));
    }

    public static BateriaService getInstancia() {
        if (instancia == null) {
            instancia = new BateriaService();
        }
        return instancia;
    }

    public void agregar(Bateria b) {
        coleccion.add(b);
    }

    public Bateria buscar(String id) {
        return coleccion.stream()
                .filter(b -> b.getIdBateria().equalsIgnoreCase(id))
                .findFirst().orElse(null);
    }

    public boolean actualizar(String id, Bateria nueva) {
        for (int i = 0; i < coleccion.size(); i++) {
            if (coleccion.get(i).getIdBateria().equalsIgnoreCase(id)) {
                coleccion.set(i, nueva);
                return true;
            }
        }
        return false;
    }

    public boolean eliminar(String id) {
        return coleccion.removeIf(b -> b.getIdBateria().equalsIgnoreCase(id));
    }

    public List<Bateria> listar() {
        return new ArrayList<>(coleccion);
    }
}