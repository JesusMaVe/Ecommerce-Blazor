# Ecommerce App - Blazor Server con SQL Server

Una aplicación de comercio electrónico desarrollada con Blazor Server, ASP.NET Core Identity, Entity Framework Core y SQL Server, completamente dockerizada.

## 🛠️ Tecnologías

- **Backend**: ASP.NET Core 9.0 con Blazor Server
- **Base de Datos**: SQL Server 2022 Express
- **ORM**: Entity Framework Core 9.0
- **Autenticación**: ASP.NET Core Identity
- **Frontend**: Blazor Server Components + Tailwind CSS
- **Containerización**: Docker & Docker Compose

## 🏗️ Arquitectura

La aplicación utiliza:
- **Blazor Server** para componentes interactivos del lado del servidor
- **Identity Framework** para autenticación y autorización de usuarios
- **Entity Framework Core** con Code First para el manejo de datos
- **SQL Server** como base de datos principal
- **Docker Compose** para orquestación de servicios

## 📋 Características

- ✅ Sistema de autenticación y registro de usuarios
- ✅ Gestión de productos y categorías
- ✅ Carrito de compras
- ✅ Lista de deseos (Wishlist)
- ✅ Roles de usuario (Buyer/Seller)
- ✅ Panel de administración para vendedores
- ✅ Interfaz responsive con Tailwind CSS

## 🚀 Instalación y Configuración

### Prerrequisitos

- Docker Desktop
- .NET 9.0 SDK (para desarrollo local)
- Git

### Ejecución con Docker (Recomendado)

1. **Clonar el repositorio**
   ```bash
   git clone <url-del-repositorio>
   cd Ecommerce.App
   ```

2. **Ejecutar con Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Acceder a la aplicación**
   - Aplicación web: http://localhost:8080
   - HTTPS: http://localhost:8081

### Ejecución en desarrollo local

1. **Configurar SQL Server**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Contra123#" \
      -p 1433:1433 --name sql-server \
      mcr.microsoft.com/mssql/server:2022-latest
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   npm install
   ```

3. **Aplicar migraciones**
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder localmente**
   - HTTP: http://localhost:5160
   - HTTPS: https://localhost:7149

## 🗄️ Base de Datos

### Esquema Principal

- **AspNetUsers**: Usuarios del sistema con Identity
- **Products**: Catálogo de productos
- **Categories**: Categorías de productos
- **CartItems**: Items del carrito de compras
- **WishlistItems**: Lista de deseos

### Cadenas de Conexión

- **Desarrollo**: `Server=localhost,1433;Database=EcommerceDB;User Id=sa;Password=Contra123#;TrustServerCertificate=true;`
- **Docker**: `Server=sqlserver,1433;Database=EcommerceDB;User Id=sa;Password=Contra123#;TrustServerCertificate=true;`

## 🐳 Docker

### Servicios

1. **ecommerce.app**: Aplicación Blazor
   - Puertos: 8080 (HTTP), 8081 (HTTPS)
   - Dependencias: SQL Server

2. **sqlserver**: Base de datos SQL Server 2022 Express
   - Puerto: 1433
   - Volumen persistente: `sqlserver_data`

### Comandos útiles

```bash
# Construir y ejecutar
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d

# Ver logs
docker-compose logs -f

# Detener servicios
docker-compose down

# Limpiar volúmenes
docker-compose down -v
```

## 🛡️ Seguridad

- Contraseñas hasheadas con Identity
- Validación de formularios
- Protección CSRF con AntiForgery
- HTTPS habilitado en producción
- Validación de modelos

## 📁 Estructura del Proyecto

```
Ecommerce.App/
├── Components/          # Componentes Blazor
├── Data/               # Contexto de base de datos
├── Models/             # Modelos de datos
├── ViewModels/         # ViewModels
├── wwwroot/           # Archivos estáticos
├── Migrations/        # Migraciones EF Core
├── Properties/        # Configuración del proyecto
├── appsettings.json   # Configuración
├── Program.cs         # Punto de entrada
└── Dockerfile         # Configuración Docker
```

## ⚙️ Configuración

### Variables de Entorno

- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: Cadena de conexión DB

### Configuración de Identity

- Contraseña mínima: 6 caracteres
- Requiere dígito: Sí
- Email único: Sí
- Rutas personalizadas: /Login, /Logout

## 🔧 Desarrollo

### Migraciones

```bash
# Crear migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir migración
dotnet ef database update MigracionAnterior
```

### Tailwind CSS

```bash
# Compilar estilos
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch
```

## 📝 Scripts

### Package.json
```json
{
  "dependencies": {
    "@tailwindcss/cli": "^4.1.10",
    "tailwindcss": "^4.1.10"
  }
}
```

## 🤝 Contribución

1. Fork el repositorio
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles.

