package com.autogestion.AutomovilApp.dto;

import java.time.LocalDateTime;

public class MantenimientoDTO {
    private String idMantenimiento;
    private String tipo;
    private String descripcion;
    private double costo;
    private LocalDateTime fechaMantenimiento;
    private int kilometraje;
    private String idAutomovilGasolina;
    private String marcaAutomovilGasolina;
    private String modeloAutomovilGasolina;

    public MantenimientoDTO() {}

    public MantenimientoDTO(String idMantenimiento, String tipo, String descripcion,
                             double costo, LocalDateTime fechaMantenimiento, int kilometraje,
                             String idAutomovilGasolina, String marcaAutomovilGasolina,
                             String modeloAutomovilGasolina) {
        this.idMantenimiento = idMantenimiento;
        this.tipo = tipo;
        this.descripcion = descripcion;
        this.costo = costo;
        this.fechaMantenimiento = fechaMantenimiento;
        this.kilometraje = kilometraje;
        this.idAutomovilGasolina = idAutomovilGasolina;
        this.marcaAutomovilGasolina = marcaAutomovilGasolina;
        this.modeloAutomovilGasolina = modeloAutomovilGasolina;
    }

    public String getIdMantenimiento() { return idMantenimiento; }
    public void setIdMantenimiento(String idMantenimiento) { this.idMantenimiento = idMantenimiento; }
    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }
    public String getDescripcion() { return descripcion; }
    public void setDescripcion(String descripcion) { this.descripcion = descripcion; }
    public double getCosto() { return costo; }
    public void setCosto(double costo) { this.costo = costo; }
    public LocalDateTime getFechaMantenimiento() { return fechaMantenimiento; }
    public void setFechaMantenimiento(LocalDateTime fechaMantenimiento) { this.fechaMantenimiento = fechaMantenimiento; }
    public int getKilometraje() { return kilometraje; }
    public void setKilometraje(int kilometraje) { this.kilometraje = kilometraje; }
    public String getIdAutomovilGasolina() { return idAutomovilGasolina; }
    public void setIdAutomovilGasolina(String idAutomovilGasolina) { this.idAutomovilGasolina = idAutomovilGasolina; }
    public String getMarcaAutomovilGasolina() { return marcaAutomovilGasolina; }
    public void setMarcaAutomovilGasolina(String marcaAutomovilGasolina) { this.marcaAutomovilGasolina = marcaAutomovilGasolina; }
    public String getModeloAutomovilGasolina() { return modeloAutomovilGasolina; }
    public void setModeloAutomovilGasolina(String modeloAutomovilGasolina) { this.modeloAutomovilGasolina = modeloAutomovilGasolina; }
}