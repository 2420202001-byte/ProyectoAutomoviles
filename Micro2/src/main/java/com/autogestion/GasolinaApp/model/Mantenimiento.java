package com.autogestion.GasolinaApp.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "MANTENIMIENTO")
public class Mantenimiento {

    @Id
    @Column(name = "ID_MANTENIMIENTO", nullable = false, unique = true)
    private String idMantenimiento;

    @Column(name = "TIPO", nullable = false)
    private String tipo;

    @Column(name = "DESCRIPCION", nullable = false)
    private String descripcion;

    @Column(name = "COSTO", nullable = false)
    private double costo;

    @Column(name = "FECHA_MANTENIMIENTO", nullable = false)
    private LocalDateTime fechaMantenimiento;

    @Column(name = "KILOMETRAJE", nullable = false)
    private int kilometraje;

    @ManyToOne
    @JoinColumn(name = "ID_AUTOMOVIL", nullable = false)
    private AutomovilGasolina automovilGasolina;

    public Mantenimiento() {}

    public Mantenimiento(String idMantenimiento, String tipo, String descripcion,
                         double costo, LocalDateTime fechaMantenimiento,
                         int kilometraje, AutomovilGasolina automovilGasolina) {
        this.idMantenimiento = idMantenimiento;
        this.tipo = tipo;
        this.descripcion = descripcion;
        this.costo = costo;
        this.fechaMantenimiento = fechaMantenimiento;
        this.kilometraje = kilometraje;
        this.automovilGasolina = automovilGasolina;
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
    public AutomovilGasolina getAutomovilGasolina() { return automovilGasolina; }
    public void setAutomovilGasolina(AutomovilGasolina automovilGasolina) { this.automovilGasolina = automovilGasolina; }
}