package com.autogestion.AutomovilApp.model;

import jakarta.persistence.*;

@Entity
@Table(name = "AUTOMOVIL_ELECTRICO")
public class AutomovilElectrico extends Automovil {

    @Column(name = "AUTONOMIA_KM", nullable = false)
    private double autonomiaKm;

    @Column(name = "TIEMPO_CARGA_HORAS", nullable = false)
    private double tiempoCargaHoras;

    @ManyToOne
    @JoinColumn(name = "ID_BATERIA", nullable = true)
    private Bateria bateria;

    public AutomovilElectrico() {}

    public AutomovilElectrico(String id, String marca, String modelo, int anio, String color,
                               double precio, double autonomiaKm, double tiempoCargaHoras, Bateria bateria) {
        super(id, marca, modelo, anio, color, precio);
        this.autonomiaKm = autonomiaKm;
        this.tiempoCargaHoras = tiempoCargaHoras;
        this.bateria = bateria;
    }

    @Override
    public double calcularCostoOperacion() {
        double tarifaKwh = 800.0;
        return bateria != null ? bateria.getCapacidadKwh() * tarifaKwh : 0.0;
    }

    @Override
    public String getTipoAutomovil() { return "Eléctrico"; }

    public double getAutonomiaKm() { return autonomiaKm; }
    public void setAutonomiaKm(double autonomiaKm) { this.autonomiaKm = autonomiaKm; }
    public double getTiempoCargaHoras() { return tiempoCargaHoras; }
    public void setTiempoCargaHoras(double tiempoCargaHoras) { this.tiempoCargaHoras = tiempoCargaHoras; }
    public Bateria getBateria() { return bateria; }
    public void setBateria(Bateria bateria) { this.bateria = bateria; }
}