package com.autogestion.AutomovilApp.model;

import jakarta.persistence.*;
import java.util.List;
import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "BATERIA")
public class Bateria {

    @Id
    @Column(name = "ID_BATERIA", nullable = false, unique = true)
    private String idBateria;

    @Column(name = "MARCA", nullable = false)
    private String marca;

    @Column(name = "CAPACIDAD_KWH", nullable = false)
    private double capacidadKwh;

    @Column(name = "CICLOS_VIDA", nullable = false)
    private int ciclosVida;

    @Column(name = "VOLTAJE", nullable = false)
    private double voltaje;

    @OneToMany(mappedBy = "bateria", cascade = CascadeType.ALL)
    @JsonIgnore
    private List<AutomovilElectrico> automovilesElectricos;

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
    public List<AutomovilElectrico> getAutomovilesElectricos() { return automovilesElectricos; }
    public void setAutomovilesElectricos(List<AutomovilElectrico> automovilesElectricos) { this.automovilesElectricos = automovilesElectricos; }
}