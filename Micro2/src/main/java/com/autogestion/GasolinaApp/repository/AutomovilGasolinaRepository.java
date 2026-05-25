package com.autogestion.GasolinaApp.repository;

import com.autogestion.GasolinaApp.model.AutomovilGasolina;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface AutomovilGasolinaRepository extends JpaRepository<AutomovilGasolina, String> {

    // Consulta personalizada 1: buscar por marca y año
    @Query("SELECT a FROM AutomovilGasolina a WHERE " +
           "(:marca IS NULL OR a.marca = :marca) AND " +
           "(:anio IS NULL OR a.anio = :anio)")
    List<AutomovilGasolina> filtrar(String marca, Integer anio);

    // Consulta personalizada 2: buscar por tipo de combustible
    @Query("SELECT a FROM AutomovilGasolina a WHERE a.tipoCombustible = :tipoCombustible")
    List<AutomovilGasolina> findByTipoCombustible(String tipoCombustible);
}