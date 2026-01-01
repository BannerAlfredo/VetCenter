# 🐾 VetCenter – Sistema de Control de Citas Veterinarias en ASP.NET Core

VetCenter es un **sistema web para la gestión integral de una veterinaria**, desarrollado con **ASP.NET Core MVC**, **MySQL** y **Bootstrap**.  
Permite administrar citas, clientes, mascotas, medicamentos, usuarios y reportes desde un panel administrativo moderno y funcional.

Este proyecto está orientado a **aprendizaje, práctica profesional y uso real**, aplicando buenas prácticas, arquitectura MVC y conexión a base de datos relacional.

---

## 🚀 Características Principales

- 📅 Gestión de citas veterinarias
- 👤 Administración de clientes
- 🐶 Registro y control de mascotas
- 💊 Gestión de medicamentos
- 👨‍⚕️ Control de usuarios y roles
- 🔐 Autenticación y cierre de sesión
- 🖥️ Panel administrativo
- 🎨 Interfaz responsive con Bootstrap

---

## 🛠️ Tecnologías Utilizadas
| Categoría            | Tecnologías              |
| -------------------- | ------------------------ |
| Lenguaje Backend     | C#                       |
| Framework            | ASP.NET Core MVC         |
| Base de Datos        | MySQL                    |
| ORM                  | Entity Framework Core    |
| Frontend             | HTML5, CSS3, Bootstrap 5 |
| Arquitectura         | MVC                      |
| IDE                  | Visual Studio 2022       |
| Control de Versiones | Git & GitHub             |

---

## 📂 Estructura del Proyecto

```
VetCenter/
│── Controllers/
│   ├── CitaController.cs
│   ├── ClienteController.cs
│   ├── MascotaController.cs
│   ├── MedicamentoController.cs
│   ├── UsuarioController.cs
│   └── PanelController.cs
│
│── Models/
│   ├── AppDbContext.cs
│   ├── Cita.cs
│   ├── Cliente.cs
│   ├── Mascota.cs
│   ├── Medicamento.cs
│   └── Usuario.cs
│
│── Views/
│   ├── Cita/
│   ├── Cliente/
│   ├── Mascota/
│   ├── Medicamento/
│   ├── Panel/
│   └── Shared/
│
│── wwwroot/
│── appsettings.json
└── Program.cs

```

---

## ⚙️ Configuración del Proyecto

### 1️⃣ Clonar el repositorio
```
git clone https://github.com/BannerRodriguez/vetcenter.git
```

### 2️⃣ Configurar la base de datos

Editar el archivo `appsettings.json:`

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=vetcenter;user=root;password=;"
}
```

### 3️⃣ Restaurar dependencias
```
dotnet restore
```

### 4️⃣ Ejecutar el proyecto
```
dotnet run
```

---

## 📈 Mejoras futuras:
- Historial clínico de mascotas
- Notificaciones de citas
- Roles avanzados
- Exportación de reportes

---

## 🧑‍💻 Autor

Desarrollado por Banner Rodriguez

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT.

---
