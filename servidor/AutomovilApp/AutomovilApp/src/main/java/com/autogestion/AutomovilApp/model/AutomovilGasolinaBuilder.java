package com.autogestion.AutomovilApp.model;

public class AutomovilGasolinaBuilder {

    private String id;
    private String marca;
    private String modelo;
    private int anio;
    private String color;
    private double precio;
    private double consumoLitrosPor100Km;
    private double capacidadTanqueLitros;
    private int cilindraje;
    private String tipoCombustible;
    private String transmision;

    public AutomovilGasolinaBuilder setId(String id) {
        this.id = id; return this;
    }
    public AutomovilGasolinaBuilder setMarca(String marca) {
        this.marca = marca; return this;
    }
    public AutomovilGasolinaBuilder setModelo(String modelo) {
        this.modelo = modelo; return this;
    }
    public AutomovilGasolinaBuilder setAnio(int anio) {
        this.anio = anio; return this;
    }
    public AutomovilGasolinaBuilder setColor(String color) {
        this.color = color; return this;
    }
    public AutomovilGasolinaBuilder setPrecio(double precio) {
        this.precio = precio; return this;
    }
    public AutomovilGasolinaBuilder setConsumoLitrosPor100Km(double consumo) {
        this.consumoLitrosPor100Km = consumo; return this;
    }
    public AutomovilGasolinaBuilder setCapacidadTanqueLitros(double capacidad) {
        this.capacidadTanqueLitros = capacidad; return this;
    }
    public AutomovilGasolinaBuilder setCilindraje(int cilindraje) {
        this.cilindraje = cilindraje; return this;
    }
    public AutomovilGasolinaBuilder setTipoCombustible(String tipoCombustible) {
        this.tipoCombustible = tipoCombustible; return this;
    }
    public AutomovilGasolinaBuilder setTransmision(String transmision) {
        this.transmision = transmision; return this;
    }

    public AutomovilGasolina build() {
        return new AutomovilGasolina(id, marca, modelo, anio, color, precio,
                consumoLitrosPor100Km, capacidadTanqueLitros, cilindraje,
                tipoCombustible, transmision);
    }
}