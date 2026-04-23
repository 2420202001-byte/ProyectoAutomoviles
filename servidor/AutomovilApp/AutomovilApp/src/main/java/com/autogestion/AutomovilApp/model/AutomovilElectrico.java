package com.autogestion.AutomovilApp.model;

public class AutomovilElectrico extends Automovil {
    private double autonomiaKm;
    private double tiempoCargaHoras;
    private Bateria bateria;

    public AutomovilElectrico() {}

    public AutomovilElectrico(String id, String marca, String modelo, int anio, String color,
                               double precio, double autonomiaKm, double tiempoCargaHoras, Bateria bateria) {
        super(id, marca, modelo, anio, color, precio);
        this.autonomiaKm = autonomiaKm;
        this.tiempoCargaHoras = tiempoCargaHoras;
        this.bateria = bateria;
    }

    // Polimorfismo: costo = precio electricidad * capacidad batería
    @Override
    public double calcularCostoOperacion() {
        double tarifaKwh = 800.0; // COP por kWh
        if (bateria != null) {
            return bateria.getCapacidadKwh() * tarifaKwh;
        }
        return 0.0;
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
