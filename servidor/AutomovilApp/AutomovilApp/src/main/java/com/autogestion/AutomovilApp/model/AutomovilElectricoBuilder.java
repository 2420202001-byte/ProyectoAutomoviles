package com.autogestion.AutomovilApp.model;



public class AutomovilElectricoBuilder {

    private String id;
    private String marca;
    private String modelo;
    private int anio;
    private String color;
    private double precio;
    private double autonomiaKm;
    private double tiempoCargaHoras;
    private Bateria bateria;

    public AutomovilElectricoBuilder setId(String id) {
        this.id = id; return this;
    }
    public AutomovilElectricoBuilder setMarca(String marca) {
        this.marca = marca; return this;
    }
    public AutomovilElectricoBuilder setModelo(String modelo) {
        this.modelo = modelo; return this;
    }
    public AutomovilElectricoBuilder setAnio(int anio) {
        this.anio = anio; return this;
    }
    public AutomovilElectricoBuilder setColor(String color) {
        this.color = color; return this;
    }
    public AutomovilElectricoBuilder setPrecio(double precio) {
        this.precio = precio; return this;
    }
    public AutomovilElectricoBuilder setAutonomiaKm(double autonomiaKm) {
        this.autonomiaKm = autonomiaKm; return this;
    }
    public AutomovilElectricoBuilder setTiempoCargaHoras(double tiempoCargaHoras) {
        this.tiempoCargaHoras = tiempoCargaHoras; return this;
    }
    public AutomovilElectricoBuilder setBateria(Bateria bateria) {
        this.bateria = bateria; return this;
    }

    public AutomovilElectrico build() {
        return new AutomovilElectrico(id, marca, modelo, anio, color,
                precio, autonomiaKm, tiempoCargaHoras, bateria);
    }
}