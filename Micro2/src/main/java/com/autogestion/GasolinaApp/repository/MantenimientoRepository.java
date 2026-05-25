package com.autogestion.GasolinaApp.repository;

import com.autogestion.GasolinaApp.model.Mantenimiento;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface MantenimientoRepository extends JpaRepository<Mantenimiento, String> {

    // Consulta 1: traer mantenimientos con datos del auto asociado
    @Query("SELECT m FROM Mantenimiento m JOIN FETCH m.automovilGasolina WHERE m.idMantenimiento = :id")
    Mantenimiento findByIdConAuto(String id);

    // Consulta 2: filtrar mantenimientos por tipo
    @Query("SELECT m FROM Mantenimiento m WHERE m.tipo = :tipo")
    List<Mantenimiento> findByTipo(String tipo);

    // Consulta 3: filtrar mantenimientos por ID de auto
    @Query("SELECT m FROM Mantenimiento m WHERE m.automovilGasolina.id = :idAuto")
    List<Mantenimiento> findByAutomovilGasolina(String idAuto);
}