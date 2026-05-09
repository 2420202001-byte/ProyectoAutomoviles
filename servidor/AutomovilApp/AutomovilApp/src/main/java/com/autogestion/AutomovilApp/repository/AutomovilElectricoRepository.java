package com.autogestion.AutomovilApp.repository;

import com.autogestion.AutomovilApp.model.AutomovilElectrico;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface AutomovilElectricoRepository extends JpaRepository<AutomovilElectrico, String> {

    // Consulta personalizada 1: buscar por marca y año
    @Query("SELECT a FROM AutomovilElectrico a WHERE " +
           "(:marca IS NULL OR a.marca = :marca) AND " +
           "(:anio IS NULL OR a.anio = :anio)")
    List<AutomovilElectrico> filtrar(String marca, Integer anio);

    // Consulta personalizada 2: mostrar auto eléctrico con datos de su batería
    @Query("SELECT a FROM AutomovilElectrico a LEFT JOIN FETCH a.bateria WHERE a.id = :id")
    AutomovilElectrico findByIdConBateria(String id);
}