package com.autogestion.AutomovilApp.model;

import jakarta.persistence.*;

@Entity
@Table(name = "AUTOMOVIL_GASOLINA")
public class AutomovilGasolina extends Automovil implements IAplicable {

    @Column(name = "CONSUMO_LITROS_100KM", nullable = false)
    private double consumoLitrosPor100Km;

    @Column(name = "CAPACIDAD_TANQUE_LITROS", nullable = false)
    private double capacidadTanqueLitros;

    @Column(name = "CILINDRAJE", nullable = false)
    private int cilindraje;

    @Column(name = "TIPO_COMBUSTIBLE", nullable = false)
    private String tipoCombustible;

    @Column(name = "TRANSMISION")
    private String transmision;

    public AutomovilGasolina() {}

    public AutomovilGasolina(String id, String marca, String modelo, int anio, String color,
                              double precio, double consumoLitrosPor100Km, double capacidadTanqueLitros,
                              int cilindraje, String tipoCombustible) {
        super(id, marca, modelo, anio, color, precio);
        this.consumoLitrosPor100Km = consumoLitrosPor100Km;
        this.capacidadTanqueLitros = capacidadTanqueLitros;
        this.cilindraje = cilindraje;
        this.tipoCombustible = tipoCombustible;
        this.transmision = "Manual";
    }

    public AutomovilGasolina(String id, String marca, String modelo, int anio, String color,
                              double precio, double consumoLitrosPor100Km, double capacidadTanqueLitros,
                              int cilindraje, String tipoCombustible, String transmision) {
        super(id, marca, modelo, anio, color, precio);
        this.consumoLitrosPor100Km = consumoLitrosPor100Km;
        this.capacidadTanqueLitros = capacidadTanqueLitros;
        this.cilindraje = cilindraje;
        this.tipoCombustible = tipoCombustible;
        this.transmision = transmision;
    }

    @Override
    public double calcularCostoOperacion() {
        return consumoLitrosPor100Km * 9500.0;
    }

    @Override
    public String getTipoAutomovil() { return "Gasolina"; }

    @Override
    public double calcular() {
        if (consumoLitrosPor100Km == 0) return 0;
        return (capacidadTanqueLitros / consumoLitrosPor100Km) * 100;
    }

    @Override
    public String getDescripcionCalculo() {
        return String.format("Autonomía máx: %.1f km (tanque: %.1f L, consumo: %.1f L/100km)",
                calcular(), capacidadTanqueLitros, consumoLitrosPor100Km);
    }

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