package com.autogestion.AutomovilApp.model;

public abstract class Automovil {
    private String id;
    private String marca;
    private String modelo;
    private int anio;
    private String color;
    private double precio;

    public Automovil() {}

    public Automovil(String id, String marca, String modelo, int anio, String color, double precio) {
        this.id = id;
        this.marca = marca;
        this.modelo = modelo;
        this.anio = anio;
        this.color = color;
        this.precio = precio;
    }

    // Método abstracto para polimorfismo
    public abstract double calcularCostoOperacion();
    public abstract String getTipoAutomovil();

    // Getters y Setters
    public String getId() { return id; }
    public void setId(String id) { this.id = id; }

    public String getMarca() { return marca; }
    public void setMarca(String marca) { this.marca = marca; }

    public String getModelo() { return modelo; }
    public void setModelo(String modelo) { this.modelo = modelo; }

    public int getAnio() { return anio; }
    public void setAnio(int anio) { this.anio = anio; }

    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }

    public double getPrecio() { return precio; }
    public void setPrecio(double precio) { this.precio = precio; }

    @Override
    public String toString() {
        return id + " - " + marca + " " + modelo + " (" + anio + ")";
    }
}
