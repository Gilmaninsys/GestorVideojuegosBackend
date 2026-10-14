# 🎮 Gestor de Videojuegos - Backend API

Esta es la API RESTful construida en **.NET 8** bajo los principios de **Clean Architecture** para el proyecto "Gestor de Videojuegos". Está diseñada con un enfoque estricto en la seguridad y el rendimiento, sirviendo como núcleo de datos y validación para la PWA del frontend.

## 🚀 Tecnologías y Arquitectura
- **Framework:** ASP.NET Core 8
- **Arquitectura:** Clean Architecture (Separación estricta de Domain, Application, Infrastructure y Presentation).
- **Base de Datos:** SQL Server + Entity Framework Core.
- **Documentación:** Scalar (OpenAPI) integrado para pruebas de endpoints.
- **Seguridad Integral:**
  - Autenticación JWT enviada exclusivamente en cookies `HttpOnly` (SameSite=None/Lax).
  - Prevención nativa de CSRF mediante Anti-Forgery Tokens.
  - Prevención XSS vía Data Annotations (`[RegularExpression]`) en DTOs.
  - Políticas estrictas de CSP (Content-Security-Policy).
  - Endpoint externo blindado por validación de API Key mediante Middleware personalizado.
- **Manejo de Errores:** Middleware global de excepciones para garantizar respuestas JSON estructuradas (sin exponer errores del servidor).

## ⚙️ Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- SQL Server (LocalDB o instancia de servidor completa).

## 🛠️ Instalación y Ejecución Local

### 1. Clonar el repositorio
```bash
git clone [https://github.com/Gilmaninsys/GestorVideojuegosBackend.git](https://github.com/Gilmaninsys/GestorVideojuegosBackend.git)
cd GestorVideojuegosBackend

2. Configurar Variables de Entorno (.env)
Crea un archivo .env en la carpeta raíz del proyecto (donde se ubica la API) con la siguiente estructura. Nunca subas secretos al appsettings.json:

Fragmento de código
DB_CONNECTION_STRING="Server=(localdb)\\MSSQLLocalDB;Database=GVIDEOJUEGOS;Trusted_Connection=True;TrustServerCertificate=True;"
JWT_SECRET="MiClaveSuperSecretaYMuyLargaParaElGestorDeVideojuegos2026**"
API_KEY="MiApiKeyPublicaParaGestorVideojuegos123"
3. Aplicar Migraciones a la Base de Datos
Abre tu terminal en la carpeta principal de la solución y ejecuta:

Bash
dotnet ef database update --project GestorVideojuegos.Infrastructure
4. Ejecutar la API
Bash
dotnet run --project GestorVideojuegos.Api
La API y su documentación interactiva con Scalar estarán disponibles (por defecto) en https://localhost:7036.
