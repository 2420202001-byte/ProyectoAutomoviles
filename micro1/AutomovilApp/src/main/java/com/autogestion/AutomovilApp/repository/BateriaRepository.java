package com.autogestion.AutomovilApp.repository;

import com.autogestion.AutomovilApp.model.Bateria;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface BateriaRepository extends JpaRepository<Bateria, String> {

    // Consulta personalizada 1: buscar baterías por marca
    @Query("SELECT b FROM Bateria b WHERE b.marca = :marca")
    List<Bateria> findByMarca(String marca);

    // Consulta personalizada 2: buscar baterías con capacidad mayor a X
    @Query("SELECT b FROM Bateria b WHERE b.capacidadKwh >= :capacidad")
    List<Bateria> findByCapacidadMinima(double capacidad);
}