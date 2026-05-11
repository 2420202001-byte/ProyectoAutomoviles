# 🚗 AutoGestión S.A.S - Sistema de Gestión de Automóviles

Sistema de gestión de automóviles desarrollado con arquitectura cliente-servidor usando Servicios Web REST y persistencia en base de datos Oracle.

---

## 👥 Integrantes

- Yaser Rondón
- Ismael Cardozo
- Juan Mancipe

**Universidad de Ibagué — Desarrollo de Aplicaciones Empresariales 2026A**

---

## 🏗️ Arquitectura

```
ProyectoAutomoviles/
├── servidor/     → Spring Boot (Java) - API REST + JPA/Hibernate + Oracle
├── cliente/      → Windows Forms (C#) - Cliente de escritorio
└── cliente2/     → Electron (JavaScript) - Cliente web de escritorio
```

---

## 🛠️ Tecnologías

| Componente | Tecnología |
|---|---|
| Servidor | Java + Spring Boot 4.0.5 |
| Persistencia | JPA/Hibernate + Oracle 21c XE |
| Cliente 1 | C# + Windows Forms (.NET Framework 4.7.2) |
| Cliente 2 | JavaScript + Electron.js |
| Comunicación | REST API + JSON |
| Librería HTTP C# | RestSharp 114.0.0 |
| Librería HTTP JS | Axios |

---

## ⚙️ Requisitos previos

- Java 21 o superior
- Node.js (versión LTS)
- Visual Studio (para el cliente C#)
- Oracle Database 21c XE instalado y configurado
- SQL Developer (opcional, para ver la BD)

---

## 🗄️ Configuración de Oracle

### 1. Crear usuario en Oracle

Abre `cmd` como **Administrador** y ejecuta:

```sql
sqlplus / as sysdba
alter session set "_ORACLE_SCRIPT"=true;
CREATE USER DAE2026 IDENTIFIED BY DAE2026;
GRANT DBA TO DAE2026;
CONNECT DAE2026/DAE2026@XE;
```

### 2. Verificar servicios Oracle

Asegúrate que estos servicios estén corriendo en Windows:
- `OracleORADB21Home1TNSListener` (iniciar primero)
- `OracleServiceXE` (iniciar segundo)

---

## 🚀 Cómo ejecutar el proyecto

### 1️⃣ Servidor Spring Boot

```bash
# Navegar a la carpeta del servidor
cd servidor/AutomovilApp

# Ejecutar el servidor (primera vez descarga dependencias ~2 min)
.\mvnw.cmd spring-boot:run
```

El servidor quedará corriendo en: `http://localhost:8080`

> ⚠️ Oracle debe estar corriendo antes de iniciar el servidor.

---

### 2️⃣ Cliente 1 — C# Windows Forms

1. Abre **Visual Studio**
2. Ve a `cliente/AutomovilAppCliente/AutomovilAppCliente`
3. Abre el archivo `AutoMovilAppCliente.slnx`
4. Presiona **F5** para ejecutar

> ⚠️ El servidor debe estar corriendo antes de usar el cliente.

---

### 3️⃣ Cliente 2 — Electron (JavaScript)

```bash
# Navegar a la carpeta del cliente 2
cd cliente2

# Instalar dependencias (solo la primera vez)
npm install

# Ejecutar el cliente
npm start
```

> ⚠️ El servidor debe estar corriendo antes de usar el cliente.

---

## 📡 Endpoints REST

### 🔋 Baterías `/baterias`

| Método | URL | Descripción |
|---|---|---|
| GET | `/baterias/` | Listar todas |
| GET | `/baterias/{id}` | Buscar por ID |
| GET | `/baterias/filtrar?marca=X` | Filtrar por marca |
| GET | `/baterias/capacidad?capacidad=X` | Filtrar por capacidad mínima |
| POST | `/baterias/` | Agregar |
| PUT | `/baterias/{id}` | Actualizar |
| DELETE | `/baterias/{id}` | Eliminar |

### ⚡ Automóviles Eléctricos `/electricos`

| Método | URL | Descripción |
|---|---|---|
| GET | `/electricos/` | Listar todos |
| GET | `/electricos/{id}` | Buscar por ID con batería |
| GET | `/electricos/filtrar?marca=X&anio=X` | Filtrar por marca y año |
| POST | `/electricos/` | Agregar |
| PUT | `/electricos/{id}` | Actualizar |
| DELETE | `/electricos/{id}` | Eliminar |

### ⛽ Automóviles Gasolina `/gasolina`

| Método | URL | Descripción |
|---|---|---|
| GET | `/gasolina/` | Listar todos |
| GET | `/gasolina/{id}` | Buscar por ID |
| GET | `/gasolina/filtrar?marca=X&anio=X` | Filtrar por marca y año |
| GET | `/gasolina/combustible?tipoCombustible=X` | Filtrar por combustible |
| POST | `/gasolina/` | Agregar |
| PUT | `/gasolina/{id}` | Actualizar |
| DELETE | `/gasolina/{id}` | Eliminar |

---

## 📦 Modelo de datos

### Bateria (TABLA_A - Maestro)
| Campo | Tipo | Restricción |
|---|---|---|
| idBateria | String | PK, NOT NULL, UNIQUE |
| marca | String | NOT NULL |
| capacidadKwh | double | NOT NULL |
| ciclosVida | int | NOT NULL |
| voltaje | double | NOT NULL |

### AutomovilElectrico (TABLA_B - Detalle)
| Campo | Tipo | Restricción |
|---|---|---|
| id | String | PK, NOT NULL, UNIQUE |
| marca | String | NOT NULL |
| modelo | String | NOT NULL |
| anio | int | NOT NULL |
| color | String | - |
| precio | double | NOT NULL |
| fechaRegistro | LocalDateTime | - |
| autonomiaKm | double | NOT NULL |
| tiempoCargaHoras | double | NOT NULL |
| idBateria | String | FK → Bateria |

### AutomovilGasolina
| Campo | Tipo | Restricción |
|---|---|---|
| id | String | PK, NOT NULL, UNIQUE |
| marca | String | NOT NULL |
| modelo | String | NOT NULL |
| anio | int | NOT NULL |
| color | String | - |
| precio | double | NOT NULL |
| fechaRegistro | LocalDateTime | - |
| consumoLitrosPor100Km | double | NOT NULL |
| capacidadTanqueLitros | double | NOT NULL |
| cilindraje | int | NOT NULL |
| tipoCombustible | String | NOT NULL |
| transmision | String | - |

---

## 🎨 Patrones de diseño implementados

| Patrón | Dónde |
|---|---|
| **MVC** | Arquitectura completa (Model/Service/Controller) |
| **Singleton** | `AutomovilElectricoService`, `AutomovilGasolinaService`, `BateriaService` |
| **Builder** | `AutomovilElectricoBuilder`, `AutomovilGasolinaBuilder` |
| **Observer** | `AutoObserver.cs` en cliente C#, funciones observer en cliente Electron |

---

## 🗃️ Consultas JPA personalizadas

```java
// Buscar autos eléctricos por marca y año
SELECT a FROM AutomovilElectrico a WHERE
(:marca IS NULL OR a.marca = :marca) AND
(:anio IS NULL OR a.anio = :anio)

// Buscar auto eléctrico con datos completos de su batería
SELECT a FROM AutomovilElectrico a LEFT JOIN FETCH a.bateria WHERE a.id = :id

// Buscar baterías por marca
SELECT b FROM Bateria b WHERE b.marca = :marca

// Buscar baterías con capacidad mínima
SELECT b FROM Bateria b WHERE b.capacidadKwh >= :capacidad
```

---

## ✅ Funcionalidades

- ✅ CRUD completo Automóvil Eléctrico
- ✅ CRUD completo Automóvil Gasolina
- ✅ CRUD completo Baterías
- ✅ Relación maestro-detalle (Batería → AutomovilElectrico)
- ✅ Persistencia en Oracle con JPA/Hibernate
- ✅ Filtros por marca y año
- ✅ Consultas JPA personalizadas
- ✅ LocalDateTime en modelos
- ✅ Menú principal con Ayuda → Acerca de
- ✅ Patrón Observer en ambos clientes
- ✅ Dos clientes en tecnologías diferentes

---

## 📝 Versión

**v2.0.0** — Con persistencia Oracle + JPA/Hibernate