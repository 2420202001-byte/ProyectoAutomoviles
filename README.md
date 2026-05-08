# AutoMovilApp 🚗

Sistema de Gestión de Automóviles desarrollado con arquitectura cliente-servidor usando Servicios Web REST.

## 📋 Descripción

Aplicación empresarial que permite gestionar automóviles eléctricos y a gasolina mediante operaciones CRUD, expuestas a través de microservicios REST y consumidas por un cliente de escritorio.

## 👥 Integrantes

- Yaser Rondon
- Ismael Cardozo
- Juan Mancipe

**Universidad de Ibagué — Desarrollo de Aplicaciones Empresariales 2026A**

---

## 🏗️ Arquitectura

AutoMovilApp/
├── servidor/   → Spring Boot (Java) - API REST
└── cliente/    → Windows Forms (C#) - Interfaz gráfica

## 🛠️ Tecnologías

| Componente | Tecnología |
|---|---|
| Servidor | Java + Spring Boot 4.0.5 |
| Cliente | C# + Windows Forms (.NET Framework 4.7.2) |
| Comunicación | REST API + JSON |
| Librería HTTP | RestSharp 114.0.0 |

---

# 🚀 Cómo ejecutar el proyecto

## ⚙️ Requisitos previos
- Java 21 o superior
- Node.js (versión LTS)
- Visual Studio (para el cliente C#)

---

## 1️⃣ Servidor Spring Boot

```bash
# Navegar a la carpeta del servidor
cd servidor/AutomovilApp

# Ejecutar el servidor
.\mvnw.cmd spring-boot:run
```

El servidor quedará corriendo en: `http://localhost:8080`

---

## 2️⃣ Cliente 1 — C# Windows Forms

1. Abre Visual Studio
2. Ve a `cliente/AutomovilAppCliente/AutomovilAppCliente`
3. Abre el archivo `AutoMovilAppCliente.slnx`
4. Presiona **F5** para ejecutar

> ⚠️ El servidor debe estar corriendo antes de usar el cliente.

---

## 3️⃣ Cliente 2 — Electron (JavaScript)

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

## 🔁 Ejecutar todo junto (Windows)

Doble clic en el archivo `iniciar-todo.bat` en la raíz del proyecto.

## 📡 Endpoints REST

### Automóviles Eléctricos
| Método | URL | Descripción |
|---|---|---|
| GET | `/electricos/` | Listar todos |
| GET | `/electricos/{id}` | Buscar por ID |
| GET | `/electricos/filtrar?marca=X&anio=X` | Filtrar |
| POST | `/electricos/` | Agregar |
| PUT | `/electricos/{id}` | Actualizar |
| DELETE | `/electricos/{id}` | Eliminar |

### Automóviles a Gasolina
| Método | URL | Descripción |
|---|---|---|
| GET | `/gasolina/` | Listar todos |
| GET | `/gasolina/{id}` | Buscar por ID |
| GET | `/gasolina/filtrar?marca=X&anio=X` | Filtrar |
| POST | `/gasolina/` | Agregar |
| PUT | `/gasolina/{id}` | Actualizar |
| DELETE | `/gasolina/{id}` | Eliminar |

### Baterías
| Método | URL | Descripción |
|---|---|---|
| GET | `/baterias/` | Listar todos |
| GET | `/baterias/{id}` | Buscar por ID |
| POST | `/baterias/` | Agregar |
| PUT | `/baterias/{id}` | Actualizar |
| DELETE | `/baterias/{id}` | Eliminar |

---

## 📦 Modelo de datos

### AutomovilElectrico
| Campo | Tipo |
|---|---|
| id | String |
| marca | String |
| modelo | String |
| anio | int |
| color | String |
| precio | double |
| autonomiaKm | double |
| tiempoCargaHoras | double |
| bateria | Bateria |

### AutomovilGasolina
| Campo | Tipo |
|---|---|
| id | String |
| marca | String |
| modelo | String |
| anio | int |
| color | String |
| precio | double |
| consumoLitrosPor100Km | double |
| capacidadTanqueLitros | double |
| cilindraje | int |
| tipoCombustible | String |
| transmision | String |

### Bateria
| Campo | Tipo |
|---|---|
| idBateria | String |
| marca | String |
| capacidadKwh | double |
| ciclosVida | int |
| voltaje | double |

---

## ✅ Funcionalidades

- ✅ Insertar automóvil eléctrico y a gasolina
- ✅ Buscar por ID
- ✅ Actualizar (previa búsqueda)
- ✅ Eliminar (previa búsqueda)
- ✅ Listar todos
- ✅ Filtrar por marca y año
- ✅ Gestión de baterías
- ✅ Menú principal con Ayuda → Acerca de...

---

## 📝 Versión

**v1.0.0**
