package com.autogestion.AutomovilApp.dto;

import java.time.LocalDateTime;

@SuppressWarnings("all")
public class AutomovilElectricoDTO {
    private String id;
    private String marca;
    private String modelo;
    private int anio;
    private String color;
    private double precio;
    private LocalDateTime fechaRegistro;
    private double autonomiaKm;
    private double tiempoCargaHoras;
    private BateriaDTO bateria;

    public AutomovilElectricoDTO() {}

    public AutomovilElectricoDTO(String id, String marca, String modelo, int anio,
                                  String color, double precio, LocalDateTime fechaRegistro,
                                  double autonomiaKm, double tiempoCargaHoras, BateriaDTO bateria) {
        this.id = id;
        this.marca = marca;
        this.modelo = modelo;
        this.anio = anio;
        this.color = color;
        this.precio = precio;
        this.fechaRegistro = fechaRegistro;
        this.autonomiaKm = autonomiaKm;
        this.tiempoCargaHoras = tiempoCargaHoras;
        this.bateria = bateria;
    }

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
    public LocalDateTime getFechaRegistro() { return fechaRegistro; }
    public void setFechaRegistro(LocalDateTime fechaRegistro) { this.fechaRegistro = fechaRegistro; }
    public double getAutonomiaKm() { return autonomiaKm; }
    public void setAutonomiaKm(double autonomiaKm) { this.autonomiaKm = autonomiaKm; }
    public double getTiempoCargaHoras() { return tiempoCargaHoras; }
    public void setTiempoCargaHoras(double tiempoCargaHoras) { this.tiempoCargaHoras = tiempoCargaHoras; }
    public BateriaDTO getBateria() { return bateria; }
    public void setBateria(BateriaDTO bateria) { this.bateria = bateria; }
}