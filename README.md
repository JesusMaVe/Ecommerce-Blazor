# Documentación Técnica - ECommerce Blazor

## 📋 Información General del Proyecto

- **Nombre**: CreatorMarket - Plataforma de ECommerce
- **Tecnología Principal**: ASP.NET Core 9.0 con Blazor Server
- **Base de Datos**: SQL Server 2022 Express
- **ORM**: Entity Framework Core 9.0
- **Autenticación**: ASP.NET Core Identity
- **Estilos**: Tailwind CSS 4.1.10
- **Containerización**: Docker & Docker Compose

## 🏗️ Arquitectura del Sistema

### Estructura de Directorios

```
Ecommerce.App/
├── Components/
│   ├── App.razor                    # Componente raíz de la aplicación
│   ├── Routes.razor                 # Configuración de rutas
│   ├── Layout/
│   │   └── MainLayout.razor        # Layout principal con header/footer
│   ├── Pages/
│   │   ├── Auth/
│   │   │   ├── Login.razor         # Página de inicio de sesión
│   │   │   ├── Register.razor      # Página de registro
│   │   │   └── Logout.razor        # Página de cierre de sesión
│   │   ├── Cart.razor              # Carrito de compras
│   │   ├── Home.razor              # Página principal
│   │   ├── Products.razor          # Catálogo de productos
│   │   ├── Wishlist.razor          # Lista de deseos
│   │   └── Seller/
│   │       └── Dashboard.razor     # Panel de control vendedor
│   └── Styles/
│       └── input.css               # Archivo de entrada Tailwind
├── Data/
│   └── ApplicationDbContext.cs     # Contexto de Entity Framework
├── Models/
│   ├── ApplicationUser.cs          # Usuario extendido de Identity
│   ├── Products.cs                 # Entidades Product y Category
│   └── CartAndWishlist.cs         # Entidades Cart y Wishlist
├── ViewModels/
│   ├── AuthViewModels.cs          # ViewModels para autenticación
│   └── ProductViewModels.cs       # ViewModels para productos
├── wwwroot/
│   ├── css/
│   │   └── output.css             # CSS compilado de Tailwind
│   └── app.css                    # CSS adicional
├── Program.cs                     # Punto de entrada y configuración
├── Dockerfile                     # Configuración de contenedor
├── package.json                   # Dependencias de Node.js
└── appsettings.json              # Configuración de la aplicación
```

## 🗄️ Base de Datos y Modelos

### Configuración de Entity Framework

**ApplicationDbContext.cs:**
```csharp
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
}
```

### Modelos de Datos

#### 1. ApplicationUser (Hereda de IdentityUser)
```csharp
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; init; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; init; } = string.Empty;
    
    [Required]
    public UserType UserType { get; init; } = UserType.Customer;
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public string FullName => $"{FirstName} {LastName}";
}

public enum UserType
{
    Customer = 0,
    Seller = 1
}
```

#### 2. Product
```csharp
public class Product
{
    public int Id { get; init; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    [Required]
    public string SellerId { get; init; } = string.Empty;
    
    [Required]
    public int CategoryId { get; set; }
    
    // Navigation Properties
    public ApplicationUser Seller { get; init; } = null!;
    public Category Category { get; init; } = null!;
}
```

#### 3. Category
```csharp
public class Category
{
    public int Id { get; init; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; init; } = string.Empty;
    
    [StringLength(200)]
    public string? Description { get; init; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    // Foreign Key
    [Required]
    public string SellerId { get; init; } = string.Empty;
    
    // Navigation Properties
    public ApplicationUser Seller { get; init; } = null!;
    public ICollection<Product> Products { get; init; } = new List<Product>();
}
```

#### 4. CartItem
```csharp
public class CartItem
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; init; } = string.Empty;
    
    [Required]
    public int ProductId { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    // Navigation Properties
    public ApplicationUser User { get; init; } = null!;
    public Product Product { get; init; } = null!;
}
```

#### 5. WishlistItem
```csharp
public class WishlistItem
{
    public int Id { get; init; }
    
    [Required]
    public string UserId { get; init; } = string.Empty;
    
    [Required]
    public int ProductId { get; init; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    // Navigation Properties
    public ApplicationUser User { get; init; } = null!;
    public Product Product { get; init; } = null!;
}
```

### Configuración de Relaciones en OnModelCreating

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // ApplicationUser configuration
    builder.Entity<ApplicationUser>(entity =>
    {
        entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
        entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
        entity.Property(e => e.UserType).HasConversion<int>();
    });

    // Product configuration
    builder.Entity<Product>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
        entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
        entity.Property(e => e.ImageUrl).HasMaxLength(500);
        
        // Relationship with Seller
        entity.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship with Category
        entity.HasOne(e => e.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    // Category configuration
    builder.Entity<Category>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Description).HasMaxLength(200);
        
        // Relationship with Seller
        entity.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Unique constraint for seller-category name
        entity.HasIndex(e => new { e.SellerId, e.Name })
            .IsUnique();
    });
}
```

## 🔧 Configuración de la Aplicación

### Program.cs - Configuración Principal

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database Configuration
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/AccessDenied";
});
```

### Cadenas de Conexión

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EcommerceDB;User Id=sa;Password=Contra123#;TrustServerCertificate=true;"
  }
}
```

**appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EcommerceDB;User Id=sa;Password=Contra123#;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## 📦 Migraciones de Entity Framework

### Paso Completo para Crear y Aplicar Migraciones

#### 1. Preparación del Entorno

**Instalar herramientas EF Core:**
```bash
# Instalar globalmente
dotnet tool install --global dotnet-ef

# O actualizar si ya está instalado
dotnet tool update --global dotnet-ef

# Verificar instalación
dotnet ef --version
```

#### 2. Crear Primera Migración

```bash
# Navegar al directorio del proyecto
cd Ecommerce.App

# Crear migración inicial
dotnet ef migrations add InitialCreate

# Crear migración con descripción específica
dotnet ef migrations add "CreateUserProductCartWishlistTables"
```

#### 3. Verificar la Migración Creada

La migración se crea en la carpeta `Migrations/` con archivos:
- `[Timestamp]_InitialCreate.cs` - Clase de migración
- `[Timestamp]_InitialCreate.Designer.cs` - Metadatos
- `ApplicationDbContextModelSnapshot.cs` - Snapshot del modelo

**Ejemplo de archivo de migración generado:**
```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Código para crear tablas
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                // ... más columnas
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Código para revertir cambios
        migrationBuilder.DropTable(name: "Categories");
    }
}
```

#### 4. Aplicar Migraciones

**Aplicar todas las migraciones pendientes:**
```bash
dotnet ef database update
```

**Aplicar hasta una migración específica:**
```bash
dotnet ef database update InitialCreate
```

**Aplicar en un entorno específico:**
```bash
dotnet ef database update --environment Production
```

#### 5. Verificar Estado de Migraciones

```bash
# Ver migraciones aplicadas y pendientes
dotnet ef migrations list

# Ver el SQL que se ejecutará
dotnet ef migrations script

# Ver SQL de una migración específica
dotnet ef migrations script InitialCreate
```

#### 6. Comandos de Migración Adicionales

**Revertir a migración anterior:**
```bash
# Revertir la última migración (sin eliminar archivo)
dotnet ef database update [NombreMigracionAnterior]

# Eliminar la última migración (si no se ha aplicado)
dotnet ef migrations remove
```

**Crear migración vacía:**
```bash
dotnet ef migrations add NombreMigracion --output-dir Data/Migrations
```

**Generar script SQL:**
```bash
# Script de toda la base de datos
dotnet ef migrations script --output schema.sql

# Script de migración específica
dotnet ef migrations script InitialCreate CreateUserTables --output update.sql
```

#### 7. Migración Automática en Program.cs

El proyecto está configurado para aplicar migraciones automáticamente al iniciar:

```csharp
var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migraciones aplicadas exitosamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error aplicando migraciones: {ex.Message}");
    }
}
```

#### 8. Migración en Docker

Para aplicar migraciones en contenedor Docker:

```bash
# Ejecutar migración dentro del contenedor
docker exec -it ecommerce-app dotnet ef database update

# O reconstruir el contenedor (aplica migraciones automáticamente)
docker-compose down
docker-compose up --build
```

#### 9. Solución de Problemas Comunes

**Error de conexión:**
```bash
# Verificar cadena de conexión
dotnet ef dbcontext info

# Usar cadena de conexión específica
dotnet ef database update --connection "Server=localhost;Database=EcommerceDB;Trusted_Connection=true;"
```

**Conflictos de migración:**
```bash
# Ver diferencias
dotnet ef migrations list
dotnet ef database update --verbose

# Resetear base de datos (CUIDADO - elimina datos)
dotnet ef database drop
dotnet ef database update
```

## 🐳 Configuración Docker

### Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ecommerce.App/Ecommerce.App.csproj", "Ecommerce.App/"]
RUN dotnet restore "Ecommerce.App/Ecommerce.App.csproj"
COPY . .
WORKDIR "/src/Ecommerce.App"
RUN dotnet build "./Ecommerce.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecommerce.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.App.dll"]
```

### Docker Compose

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: ecommerce-sqlserver
    environment:
      SA_PASSWORD: "Contra123#"
      ACCEPT_EULA: "Y"
      MSSQL_PID: "Express"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

  ecommerce.app:
    image: ecommerce.app
    build:
      context: .
      dockerfile: Ecommerce.App/Dockerfile
    container_name: ecommerce-app
    ports:
      - "8080:8080"
      - "8081:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=EcommerceDB;User Id=sa;Password=Contra123#;TrustServerCertificate=true;
    depends_on:
      - sqlserver
    restart: unless-stopped

volumes:
  sqlserver_data:
```

## 🎨 Frontend y Estilos

### Tailwind CSS

**package.json:**
```json
{
  "dependencies": {
    "@tailwindcss/cli": "^4.1.10",
    "tailwindcss": "^4.1.10"
  }
}
```

**Configuración en input.css:**
```css
@import "tailwindcss";
```

**Compilación de estilos:**
```bash
npx tailwindcss -i ./Components/Styles/input.css -o ./wwwroot/css/output.css --watch
```

## 🔐 Sistema de Autenticación

### Configuración de Identity

- **Política de contraseñas**: Mínimo 6 caracteres, requiere dígito
- **Email único**: Habilitado
- **Rutas personalizadas**: `/Login`, `/Logout`, `/AccessDenied`
- **Cookies**: Configuradas para autenticación persistente

### ViewModels de Autenticación

**LoginViewModel:**
```csharp
public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
```

**RegisterViewModel:**
```csharp
public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un tipo de usuario")]
    public UserType UserType { get; set; } = UserType.Customer;
}
```

## 🛡️ Seguridad Implementada

1. **Validación de Formularios**: DataAnnotations en todos los ViewModels
2. **Protección CSRF**: AntiForgery tokens habilitados
3. **Hashing de Contraseñas**: Identity maneja automáticamente
4. **Autorización basada en Roles**: Atributo `[Authorize]` en páginas protegidas
5. **Validación de Entrada**: Sanitización en Entity Framework
6. **HTTPS**: Configurado para producción

## 📱 Funcionalidades Principales

### 1. Gestión de Usuarios
- Registro con tipo de usuario (Customer/Seller)
- Login/Logout con autenticación persistente
- Navegación adaptativa según rol

### 2. Gestión de Productos (Sellers)
- CRUD completo de productos
- Gestión de categorías
- Control de stock y precios
- Activar/desactivar productos

### 3. Carrito de Compras (Customers)
- Agregar productos al carrito
- Modificar cantidades
- Calcular totales automáticamente
- Persistencia en base de datos

### 4. Lista de Deseos
- Guardar productos favoritos
- Mover productos a carrito
- Gestión de wishlist personal

### 5. Catálogo Público
- Filtros por categoría y búsqueda
- Ordenamiento por precio/nombre/fecha
- Paginación de resultados
- Vista detallada de productos

## 🚀 Comandos de Ejecución

### Desarrollo Local
```bash
# Restaurar dependencias
dotnet restore
npm install

# Aplicar migraciones
dotnet ef database update

# Ejecutar aplicación
dotnet run

# Compilar estilos (en paralelo)
npx tailwindcss -i ./Components/Styles/input.css -o ./wwwroot/css/output.css --watch
```

### Docker
```bash
# Construir y ejecutar
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d

# Ver logs
docker-compose logs -f ecommerce.app

# Detener servicios
docker-compose down
```

### URLs de Acceso
- **Aplicación Local**: https://localhost:7149 | http://localhost:5160
- **Aplicación Docker**: http://localhost:8080 | https://localhost:8081
- **SQL Server**: localhost:1433 (Usuario: sa, Password: Contra123#)

## 📊 Métricas del Proyecto

- **Líneas de Código**: ~2,500 líneas
- **Componentes Blazor**: 11 componentes
- **Modelos de Datos**: 5 entidades principales
- **ViewModels**: 6 ViewModels
- **Páginas**: 8 páginas principales
- **Funcionalidades CRUD**: Implementadas completamente
