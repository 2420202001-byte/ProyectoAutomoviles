package com.autogestion.AutomovilApp.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@MappedSuperclass
public abstract class Automovil {

    @Id
    @Column(name = "ID", nullable = false, unique = true)
    private String id;

    @Column(name = "MARCA", nullable = false)
    private String marca;

    @Column(name = "MODELO", nullable = false)
    private String modelo;

    @Column(name = "ANIO", nullable = false)
    private int anio;

    @Column(name = "COLOR")
    private String color;

    @Column(name = "PRECIO", nullable = false)
    private double precio;

    @Column(name = "FECHA_REGISTRO")
    private LocalDateTime fechaRegistro;

    public Automovil() {}

    public Automovil(String id, String marca, String modelo, int anio, String color, double precio) {
        this.id = id;
        this.marca = marca;
        this.modelo = modelo;
        this.anio = anio;
        this.color = color;
        this.precio = precio;
        this.fechaRegistro = LocalDateTime.now();
    }

    public abstract double calcularCostoOperacion();
    public abstract String getTipoAutomovil();

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
}