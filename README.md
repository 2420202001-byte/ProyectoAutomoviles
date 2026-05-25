🚗 AutoGestión S.A.S - Sistema de Gestión de Automóviles

Sistema de gestión de automóviles desarrollado con arquitectura de microservicios usando Servicios Web REST, patrones de diseño y persistencia en bases de datos Oracle independientes.

👥 Integrantes
Yaser Rondón
Ismael Cardozo
Juan Mancipe

Universidad de Ibagué — Desarrollo de Aplicaciones Empresariales 2026A

🏗️ Arquitectura
ProyectoAutomoviles/
├── micro1/             → Microservicio 1 - Spring Boot (puerto 8080)
│                         Gestiona: AutomovilElectrico + Bateria
│                         Proxy transparente hacia Microservicio 2
├── micro2/             → Microservicio 2 - Spring Boot (puerto 8081)
│                         Gestiona: AutomovilGasolina + Mantenimiento
├── cliente1/           → Windows Forms (C#) - Cliente de escritorio
└── cliente2/           → Electron (JavaScript) - Cliente web de escritorio

Flujo de comunicación
Cliente C# / Electron
        ↓
  Microservicio 1 (8080)   ←→   AutomovilElectrico + Bateria (Oracle DB 1)
        ↓ RestTemplate
  Microservicio 2 (8081)   ←→   AutomovilGasolina + Mantenimiento (Oracle DB 2)


Los clientes solo se comunican con el puerto 8080. El Microservicio 1 actúa como proxy transparente para las peticiones de gasolina y mantenimiento, invocando internamente al Microservicio 2.

🛠️ Tecnologías
Componente	Tecnología
Microservicio 1	Java + Spring Boot
Microservicio 2	Java + Spring Boot
Persistencia	JPA/Hibernate + Oracle 21c XE
Comunicación entre microservicios	RestTemplate (Spring)
Cliente 1	C# + Windows Forms (.NET Framework 4.7.2)
Cliente 2	JavaScript + Electron.js
Comunicación cliente-servidor	REST API + JSON
Librería HTTP C#	RestSharp 114.0.0
Librería HTTP JS	Axios
⚙️ Requisitos previos
Java 21 o superior
Node.js (versión LTS)
Visual Studio (para el cliente C#)
Oracle Database 21c XE instalado y configurado
SQL Developer (opcional, para ver la BD)

📌 Notas importantes antes de ejecutar
- El proyecto usa los wrappers Maven de cada microservicio, por lo que no necesitas tener Maven global instalado para levantar los servicios.
- El cliente 2 (Electron) requiere la instalación de dependencias con `npm install` antes del primer arranque.
- El cliente 1 se ejecuta desde la solución de Visual Studio y consume el Microservicio 1 en `http://localhost:8080`.
- El Microservicio 1 actúa como proxy para las rutas de gasolina y mantenimiento; los clientes no deben apuntar directamente a `http://localhost:8081`.

🛠️ Solución rápida de problemas
- Si `spring-boot:run` falla, verifica que Java 21 esté instalado y que `java -version` responda correctamente.
- Confirma que Oracle está arriba y que el usuario `DAE2026` existe antes de iniciar los microservicios.
- Si el cliente 2 no abre, ejecuta `npm install` dentro de `cliente2` y vuelve a correr `npm start`.
- Si el cliente 1 no conecta, revisa que el Microservicio 1 esté corriendo en el puerto 8080.

🗄️ Configuración de Oracle
1. Crear usuario en Oracle

Abre cmd como Administrador y ejecuta:

sqlplus / as sysdba
alter session set "_ORACLE_SCRIPT"=true;
CREATE USER DAE2026 IDENTIFIED BY DAE2026;
GRANT DBA TO DAE2026;
CONNECT DAE2026/DAE2026@XE;


El mismo usuario DAE2026 es usado por ambos microservicios. Las tablas se crean automáticamente con JPA/Hibernate al iniciar cada microservicio.

2. Verificar servicios Oracle

Asegúrate que estos servicios estén corriendo en Windows (en este orden):

OracleORADB21Home1TNSListener
OracleServiceXE

Si alguno está caído, desde cmd como administrador:

lsnrctl start
net start OracleServiceXE

🚀 Cómo ejecutar el proyecto

⚠️ Orden de arranque obligatorio: Oracle → micro2 → micro1 → Clientes

1️⃣ micro2 — GasolinaApp (puerto 8081)
cd micro2/GasolinaApp
.\mvnw.cmd spring-boot:run


Verificar: http://localhost:8081/gasolina/healthCheck

2️⃣ micro1 — AutoGestionApp (puerto 8080)
cd micro1/AutomovilApp
.\mvnw.cmd spring-boot:run


Verificar: http://localhost:8080/electricos/healthCheck

3️⃣ cliente1 — C# Windows Forms
Abre Visual Studio
Ve a cliente1/AutomovilAppCliente/AutomovilAppCliente
Abre AutoMovilAppCliente.slnx
Presiona F5 para ejecutar
4️⃣ cliente2 — Electron (JavaScript)
cd cliente2
npm install   # solo la primera vez
npm start

📡 Endpoints REST

Todos los endpoints son accesibles desde el puerto 8080. Los de gasolina y mantenimiento son redirigidos transparentemente al puerto 8081.

🔋 Baterías /baterias
Método	URL	Descripción
GET	/baterias/healthCheck	Estado del servicio
GET	/baterias/	Listar todas
GET	/baterias/{id}	Buscar por ID
GET	/baterias/filtrar?marca=X	Filtrar por marca
GET	/baterias/capacidad?capacidad=X	Filtrar por capacidad mínima
POST	/baterias/	Agregar
PUT	/baterias/{id}	Actualizar
DELETE	/baterias/{id}	Eliminar
⚡ Automóviles Eléctricos /electricos

Método	URL	Descripción
GET	/electricos/healthCheck	Estado del servicio
GET	/electricos/	Listar todos
GET	/electricos/{id}	Buscar por ID con batería
GET	/electricos/filtrar?marca=X&anio=X	Filtrar por marca y año
POST	/electricos/	Agregar
PUT	/electricos/{id}	Actualizar
DELETE	/electricos/{id}	Eliminar

⛽ Automóviles Gasolina /gasolina (proxy → 8081)
Método	URL	Descripción
GET	/gasolina/healthCheck	Estado del servicio
GET	/gasolina/	Listar todos
GET	/gasolina/{id}	Buscar por ID
GET	/gasolina/filtrar?marca=X&anio=X	Filtrar por marca y año
GET	/gasolina/combustible?tipoCombustible=X	Filtrar por tipo combustible
POST	/gasolina/	Agregar
PUT	/gasolina/{id}	Actualizar
DELETE	/gasolina/{id}	Eliminar

🔧 Mantenimientos /mantenimientos (proxy → 8081)
Método	URL	Descripción
GET	/mantenimientos/healthCheck	Estado del servicio
GET	/mantenimientos/	Listar todos
GET	/mantenimientos/{id}	Buscar por ID
GET	/mantenimientos/filtrar?tipo=X	Filtrar por tipo
GET	/mantenimientos/auto?idAuto=X	Filtrar por auto
POST	/mantenimientos/	Agregar
PUT	/mantenimientos/{id}	Actualizar
DELETE	/mantenimientos/{id}	Eliminar

📦 Modelo de datos
micro1 — Base de datos Oracle (puerto 8080)
Bateria
Campo	Tipo	Restricción
idBateria	String	PK
marca	String	NOT NULL
capacidadKwh	double	NOT NULL
ciclosVida	int	NOT NULL
voltaje	double	NOT NULL
AutomovilElectrico
Campo	Tipo	Restricción
id	String	PK
marca	String	NOT NULL
modelo	String	NOT NULL
anio	int	NOT NULL
color	String	-
precio	double	NOT NULL
fechaRegistro	LocalDateTime	-
autonomiaKm	double	NOT NULL
tiempoCargaHoras	double	NOT NULL
idBateria	String	FK → Bateria
micro2 — Base de datos Oracle independiente (puerto 8081)
AutomovilGasolina
Campo	Tipo	Restricción
id	String	PK
marca	String	NOT NULL
modelo	String	NOT NULL
anio	int	NOT NULL
color	String	-
precio	double	NOT NULL
fechaRegistro	LocalDateTime	-
consumoLitrosPor100Km	double	NOT NULL
capacidadTanqueLitros	double	NOT NULL
cilindraje	int	NOT NULL
tipoCombustible	String	NOT NULL
transmision	String	-
Mantenimiento
Campo	Tipo	Restricción
idMantenimiento	String	PK
tipo	String	NOT NULL
descripcion	String	-
costo	double	NOT NULL
fechaMantenimiento	LocalDateTime	-
kilometraje	int	-
automovilGasolina	AutomovilGasolina	FK → AutomovilGasolina

⚠️ Integridad referencial: No se puede agregar un Mantenimiento si el AutomovilGasolina referenciado no existe. El sistema valida esto antes de persistir.

🎨 Patrones de diseño implementados
Patrón	Dónde	Descripción
MVC	Arquitectura completa	Model / Service / Controller en ambos microservicios
Singleton	AutomovilElectricoService, BateriaService, AutomovilGasolinaService, MantenimientoService	Una única instancia por servicio
Builder	AutomovilElectricoBuilder, AutomovilGasolinaBuilder	Construcción fluida de objetos
Observer	AutoObserver.cs en cliente C#, funciones observer en Electron	Notificación de cambios en la UI
Proxy	AutomovilGasolinaController, MantenimientoController del micro 1	El micro 1 redirige peticiones al micro 2 de forma transparente
🗃️ Consultas JPA personalizadas
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

// Buscar mantenimientos por tipo
SELECT m FROM Mantenimiento m WHERE m.tipo = :tipo

// Buscar mantenimientos por auto
SELECT m FROM Mantenimiento m WHERE m.automovilGasolina.id = :idAuto

✅ Funcionalidades
✅ CRUD completo Automóvil Eléctrico
✅ CRUD completo Batería
✅ CRUD completo Automóvil Gasolina
✅ CRUD completo Mantenimiento
✅ Relación maestro-detalle (Batería → AutomovilElectrico)
✅ Integridad referencial (AutomovilGasolina → Mantenimiento)
✅ Dos microservicios independientes con bases de datos separadas
✅ Comunicación transparente entre microservicios via RestTemplate
✅ Clientes GUI apuntan únicamente al Microservicio 1
✅ Persistencia en Oracle con JPA/Hibernate
✅ DTOs para transferencia de datos
✅ Manejo centralizado de excepciones (GlobalExceptionHandler)
✅ LocalDateTime en todos los modelos
✅ Patrón Observer en ambos clientes
✅ Patrón Builder en ambos microservicios
✅ Dos clientes en tecnologías diferentes (C# y Electron.js)
✅ Menú principal con Ayuda → Acerca de
📝 Versión

v3.0.0 — Arquitectura de microservicios con bases de datos independientes y comunicación inter-servicios