package com.autogestion.AutomovilApp.dto;

import java.time.LocalDateTime;

public class AutomovilGasolinaDTO {
    private String id;
    private String marca;
    private String modelo;
    private int anio;
    private String color;
    private double precio;
    private LocalDateTime fechaRegistro;
    private double consumoLitrosPor100Km;
    private double capacidadTanqueLitros;
    private int cilindraje;
    private String tipoCombustible;
    private String transmision;

    public AutomovilGasolinaDTO() {}

    public AutomovilGasolinaDTO(String id, String marca, String modelo, int anio,
                                 String color, double precio, LocalDateTime fechaRegistro,
                                 double consumoLitrosPor100Km, double capacidadTanqueLitros,
                                 int cilindraje, String tipoCombustible, String transmision) {
        this.id = id;
        this.marca = marca;
        this.modelo = modelo;
        this.anio = anio;
        this.color = color;
        this.precio = precio;
        this.fechaRegistro = fechaRegistro;
        this.consumoLitrosPor100Km = consumoLitrosPor100Km;
        this.capacidadTanqueLitros = capacidadTanqueLitros;
        this.cilindraje = cilindraje;
        this.tipoCombustible = tipoCombustible;
        this.transmision = transmision;
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
    public double getConsumoLitrosPor100Km() { return consumoLitrosPor100Km; }
    public void setConsumoLitrosPor100Km(double v) { this.consumoLitrosPor100Km = v; }
    public double getCapacidadTanqueLitros() { return capacidadTanqueLitros; }
    public void setCapacidadTanqueLitros(double v) { this.capacidadTanqueLitros = v; }
    public int getCilindraje() { return cilindraje; }
    public void setCilindraje(int cilindraje) { this.cilindraje = cilindraje; }
    public String getTipoCombustible() { return tipoCombustible; }
    public void setTipoCombustible(String tipoCombustible) { this.tipoCombustible = tipoCombustible; }
    public String getTransmision() { return transmision; }
    public void setTransmision(String transmision) { this.transmision = transmision; }
}