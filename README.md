# 🚗 AutoGestión S.A.S
### Sistema de Gestión de Automóviles

> Sistema desarrollado con **arquitectura de microservicios** usando Servicios Web REST, patrones de diseño y persistencia en bases de datos Oracle independientes.

---

## 👥 Integrantes

| Nombre | Universidad |
|---|---|
| Yaser Rondón | Universidad de Ibagué |
| Ismael Cardozo | Universidad de Ibagué |
| Juan Mancipe | Universidad de Ibagué |

**Desarrollo de Aplicaciones Empresariales — 2026A**

---

## 🏗️ Arquitectura

```
ProyectoAutomoviles/
├── micro1/      → Microservicio 1 · Spring Boot · puerto 8080
│                  Gestiona: AutomovilElectrico + Bateria
│                  Actúa como proxy hacia Microservicio 2
├── micro2/      → Microservicio 2 · Spring Boot · puerto 8081
│                  Gestiona: AutomovilGasolina + Mantenimiento
├── cliente1/    → Cliente de escritorio · C# Windows Forms
└── cliente2/    → Cliente de escritorio · JavaScript + Electron
```

### 🔄 Flujo de comunicación

```
 [cliente1 - C#]  [cliente2 - Electron]
         \              /
          ↓            ↓
     micro1 :8080  ──────────── Oracle DB 1
     (AutomovilElectrico + Bateria)
          ↓  RestTemplate
     micro2 :8081  ──────────── Oracle DB 2
     (AutomovilGasolina + Mantenimiento)
```

> 💡 Los clientes **solo se comunican con el puerto 8080**. El micro1 actúa como **proxy transparente** hacia micro2 para las peticiones de gasolina y mantenimiento.

---

## 🛠️ Tecnologías

| Componente | Tecnología |
|---|---|
| micro1 / micro2 | Java + Spring Boot |
| Persistencia | JPA / Hibernate + Oracle 21c XE |
| Comunicación entre microservicios | RestTemplate (Spring) |
| cliente1 | C# + Windows Forms (.NET Framework 4.7.2) |
| cliente2 | JavaScript + Electron.js |
| Protocolo cliente-servidor | REST API + JSON |
| HTTP client C# | RestSharp 114.0.0 |
| HTTP client JS | Axios |

---

## ⚙️ Requisitos previos

- ☕ Java 21 o superior
- 🟢 Node.js LTS
- 🪟 Visual Studio (para cliente1)
- 🗄️ Oracle Database 21c XE
- 🔍 SQL Developer *(opcional)*

---

## 🗄️ Configuración de Oracle

### 1. Crear usuario

Abre `cmd` como **Administrador**:

```sql
sqlplus / as sysdba
alter session set "_ORACLE_SCRIPT"=true;
CREATE USER DAE2026 IDENTIFIED BY DAE2026;
GRANT DBA TO DAE2026;
CONNECT DAE2026/DAE2026@XE;
```

> El usuario `DAE2026` es compartido por ambos microservicios. Las tablas se crean automáticamente con Hibernate al arrancar.

### 2. Verificar servicios Windows *(en este orden)*

```cmd
net start OracleORADB21Home1TNSListener
net start OracleServiceXE
```

---

## 🚀 Cómo ejecutar el proyecto

> ⚠️ **Orden obligatorio:** `Oracle` → `micro2` → `micro1` → `clientes`

### 1️⃣ micro2 — GasolinaApp `http://localhost:8081`

```bash
cd micro2/GasolinaApp
.\mvnw.cmd spring-boot:run
```

✅ Verificar: `GET http://localhost:8081/gasolina/healthCheck`

---

### 2️⃣ micro1 — AutoGestionApp `http://localhost:8080`

```bash
cd micro1/AutomovilApp
.\mvnw.cmd spring-boot:run
```

✅ Verificar: `GET http://localhost:8080/electricos/healthCheck`

---

### 3️⃣ cliente1 — C# Windows Forms

1. Abre **Visual Studio**
2. Navega a `cliente1/AutomovilAppCliente/AutomovilAppCliente`
3. Abre `AutoMovilAppCliente.slnx`
4. Presiona **F5**

---

### 4️⃣ cliente2 — Electron

```bash
cd cliente2
npm install    # solo la primera vez
npm start
```

---

## 🛠️ Solución rápida de problemas

| Problema | Solución |
|---|---|
| `spring-boot:run` falla | Verifica `java -version` → debe ser Java 21+ |
| Microservicio no conecta a Oracle | Confirma que `OracleServiceXE` está corriendo y el usuario `DAE2026` existe |
| cliente2 no abre | Ejecuta `npm install` dentro de `cliente2` y vuelve a correr `npm start` |
| cliente1 no conecta | Verifica que micro1 esté corriendo en el puerto 8080 |
| cliente apunta a 8081 | ❌ Los clientes solo deben apuntar a **8080** |

---

## 📡 Endpoints REST

> Todos los endpoints se consumen desde el **puerto 8080**. Los de `/gasolina` y `/mantenimientos` son redirigidos internamente al puerto 8081.

### 🔋 Baterías — `/baterias`

| Método | URL | Descripción |
|---|---|---|
| `GET` | `/baterias/healthCheck` | Estado del servicio |
| `GET` | `/baterias/` | Listar todas |
| `GET` | `/baterias/{id}` | Buscar por ID |
| `GET` | `/baterias/filtrar?marca=X` | Filtrar por marca |
| `GET` | `/baterias/capacidad?capacidad=X` | Filtrar por capacidad mínima |
| `POST` | `/baterias/` | Agregar |
| `PUT` | `/baterias/{id}` | Actualizar |
| `DELETE` | `/baterias/{id}` | Eliminar |

### ⚡ Automóviles Eléctricos — `/electricos`

| Método | URL | Descripción |
|---|---|---|
| `GET` | `/electricos/healthCheck` | Estado del servicio |
| `GET` | `/electricos/` | Listar todos |
| `GET` | `/electricos/{id}` | Buscar por ID con batería |
| `GET` | `/electricos/filtrar?marca=X&anio=X` | Filtrar por marca y año |
| `POST` | `/electricos/` | Agregar |
| `PUT` | `/electricos/{id}` | Actualizar |
| `DELETE` | `/electricos/{id}` | Eliminar |

### ⛽ Automóviles Gasolina — `/gasolina` *(proxy → 8081)*

| Método | URL | Descripción |
|---|---|---|
| `GET` | `/gasolina/healthCheck` | Estado del servicio |
| `GET` | `/gasolina/` | Listar todos |
| `GET` | `/gasolina/{id}` | Buscar por ID |
| `GET` | `/gasolina/filtrar?marca=X&anio=X` | Filtrar por marca y año |
| `GET` | `/gasolina/combustible?tipoCombustible=X` | Filtrar por tipo de combustible |
| `POST` | `/gasolina/` | Agregar |
| `PUT` | `/gasolina/{id}` | Actualizar |
| `DELETE` | `/gasolina/{id}` | Eliminar |

### 🔧 Mantenimientos — `/mantenimientos` *(proxy → 8081)*

| Método | URL | Descripción |
|---|---|---|
| `GET` | `/mantenimientos/healthCheck` | Estado del servicio |
| `GET` | `/mantenimientos/` | Listar todos |
| `GET` | `/mantenimientos/{id}` | Buscar por ID |
| `GET` | `/mantenimientos/filtrar?tipo=X` | Filtrar por tipo |
| `GET` | `/mantenimientos/auto?idAuto=X` | Filtrar por auto |
| `POST` | `/mantenimientos/` | Agregar |
| `PUT` | `/mantenimientos/{id}` | Actualizar |
| `DELETE` | `/mantenimientos/{id}` | Eliminar |

---

## 📦 Modelo de datos

### micro1 — Oracle DB 1 (puerto 8080)

#### `Bateria`
| Campo | Tipo | Restricción |
|---|---|---|
| idBateria | String | PK |
| marca | String | NOT NULL |
| capacidadKwh | double | NOT NULL |
| ciclosVida | int | NOT NULL |
| voltaje | double | NOT NULL |

#### `AutomovilElectrico`
| Campo | Tipo | Restricción |
|---|---|---|
| id | String | PK |
| marca | String | NOT NULL |
| modelo | String | NOT NULL |
| anio | int | NOT NULL |
| color | String | — |
| precio | double | NOT NULL |
| fechaRegistro | LocalDateTime | — |
| autonomiaKm | double | NOT NULL |
| tiempoCargaHoras | double | NOT NULL |
| idBateria | String | FK → Bateria |

---

### micro2 — Oracle DB 2 independiente (puerto 8081)

#### `AutomovilGasolina`
| Campo | Tipo | Restricción |
|---|---|---|
| id | String | PK |
| marca | String | NOT NULL |
| modelo | String | NOT NULL |
| anio | int | NOT NULL |
| color | String | — |
| precio | double | NOT NULL |
| fechaRegistro | LocalDateTime | — |
| consumoLitrosPor100Km | double | NOT NULL |
| capacidadTanqueLitros | double | NOT NULL |
| cilindraje | int | NOT NULL |
| tipoCombustible | String | NOT NULL |
| transmision | String | — |

#### `Mantenimiento`
| Campo | Tipo | Restricción |
|---|---|---|
| idMantenimiento | String | PK |
| tipo | String | NOT NULL |
| descripcion | String | — |
| costo | double | NOT NULL |
| fechaMantenimiento | LocalDateTime | — |
| kilometraje | int | — |
| automovilGasolina | AutomovilGasolina | FK → AutomovilGasolina |

> ⚠️ **Integridad referencial:** No se puede registrar un `Mantenimiento` si el `AutomovilGasolina` referenciado no existe. El sistema valida esto antes de persistir.

---

## 🎨 Patrones de diseño

| Patrón | Ubicación | Descripción |
|---|---|---|
| **MVC** | Ambos microservicios | Model / Service / Controller |
| **Singleton** | `*Service.java` en micro1 y micro2 | Una única instancia por servicio |
| **Builder** | `AutomovilElectricoBuilder`, `AutomovilGasolinaBuilder` | Construcción fluida de objetos |
| **Observer** | `AutoObserver.cs` (cliente1), observer functions (cliente2) | Notificación de cambios en la UI |
| **Proxy** | Controllers de gasolina/mantenimiento en micro1 | micro1 redirige transparentemente a micro2 |

---

## 🗃️ Consultas JPA personalizadas

```java
// Eléctricos por marca y año
SELECT a FROM AutomovilElectrico a
WHERE (:marca IS NULL OR a.marca = :marca)
AND   (:anio  IS NULL OR a.anio  = :anio)

// Eléctrico con datos completos de su batería
SELECT a FROM AutomovilElectrico a
LEFT JOIN FETCH a.bateria WHERE a.id = :id

// Baterías por marca
SELECT b FROM Bateria b WHERE b.marca = :marca

// Baterías con capacidad mínima
SELECT b FROM Bateria b WHERE b.capacidadKwh >= :capacidad

// Mantenimientos por tipo
SELECT m FROM Mantenimiento m WHERE m.tipo = :tipo

// Mantenimientos por auto
SELECT m FROM Mantenimiento m WHERE m.automovilGasolina.id = :idAuto
```

---

## ✅ Funcionalidades implementadas

- ✅ CRUD completo — AutomovilElectrico
- ✅ CRUD completo — Bateria
- ✅ CRUD completo — AutomovilGasolina
- ✅ CRUD completo — Mantenimiento
- ✅ Relación maestro-detalle (Bateria → AutomovilElectrico)
- ✅ Integridad referencial (AutomovilGasolina → Mantenimiento)
- ✅ Dos microservicios independientes con BDs separadas
- ✅ Comunicación inter-microservicio via RestTemplate (proxy transparente)
- ✅ Clientes apuntan únicamente a micro1 (puerto 8080)
- ✅ Persistencia Oracle con JPA / Hibernate
- ✅ DTOs para transferencia de datos
- ✅ Manejo centralizado de excepciones (`GlobalExceptionHandler`)
- ✅ `LocalDateTime` en todos los modelos
- ✅ Patrón Observer en cliente1 y cliente2
- ✅ Patrón Builder en micro1 y micro2
- ✅ Dos clientes en tecnologías distintas (C# y Electron.js)
- ✅ Menú principal con Ayuda → Acerca de

---

## 📝 Versión

**v3.0.0** — Arquitectura de microservicios · BDs independientes · Comunicación inter-servicios
