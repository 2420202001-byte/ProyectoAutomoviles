package com.autogestion.AutomovilApp.model;

public class Bateria {
    private String idBateria;
    private String marca;
    private double capacidadKwh;
    private int ciclosVida;
    private double voltaje;

    public Bateria() {}

    public Bateria(String idBateria, String marca, double capacidadKwh, int ciclosVida, double voltaje) {
        this.idBateria = idBateria;
        this.marca = marca;
        this.capacidadKwh = capacidadKwh;
        this.ciclosVida = ciclosVida;
        this.voltaje = voltaje;
    }

    public String getIdBateria() { return idBateria; }
    public void setIdBateria(String idBateria) { this.idBateria = idBateria; }

    public String getMarca() { return marca; }
    public void setMarca(String marca) { this.marca = marca; }

    public double getCapacidadKwh() { return capacidadKwh; }
    public void setCapacidadKwh(double capacidadKwh) { this.capacidadKwh = capacidadKwh; }

    public int getCiclosVida() { return ciclosVida; }
    public void setCiclosVida(int ciclosVida) { this.ciclosVida = ciclosVida; }

    public double getVoltaje() { return voltaje; }
    public void setVoltaje(double voltaje) { this.voltaje = voltaje; }

    @Override
    public String toString() {
        return idBateria + " - " + marca + " (" + capacidadKwh + " kWh)";
    }
}

