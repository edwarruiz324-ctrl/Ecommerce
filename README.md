# Ecommerce
PruebaDeDesarrolloEcommerce

# Migración de Base de Datos

Este parte del documento explica cómo crear y aplicar migraciones en los servicios **ProductService** y **OrderService** usando **Entity Framework Core**.

---

## Requisitos

- .NET SDK instalado
- Entity Framework Core Tools instalado globalmente:

```bash
dotnet tool install --global dotnet-ef

##ProductService
# Crear migración inicial
dotnet ef migrations add InitialCreate -s src/ProductService/ProductService.Api -p src/ProductService/ProductService.Infrastructure

# Crear migración de cambios pendientes
dotnet ef migrations add PendingChanges -s src/ProductService/ProductService.Api -p src/ProductService/ProductService.Infrastructure

# Aplicar migraciones a la base de datos
dotnet ef database update -s src/ProductService/ProductService.Api -p src/ProductService/ProductService.Infrastructure

## OrderService
# Crear migración inicial
dotnet ef migrations add InitialCreate -s src/OrderService/OrderService.Api -p src/OrderService/OrderService.Infrastructure

# Crear migración de cambios pendientes
dotnet ef migrations add PendingChanges -s src/OrderService/OrderService.Api -p src/OrderService/OrderService.Infrastructure

# Aplicar migraciones a la base de datos
dotnet ef database update -s src/OrderService/OrderService.Api -p src/OrderService/OrderService.Infrastructure




## Arquitectura del Proyecto Ecommerce (.NET 8 / .NET 9)

Este proyecto implementa una solución de Ecommerce basada en microservicios, siguiendo los principios de Clean Architecture para garantizar mantenibilidad, escalabilidad y facilidad de pruebas.

## Microservicios Principales

- **OrderService:** Gestión de pedidos.
- **ProductService:** Gestión de productos.

## Estructura de Carpetas y Capas

Cada microservicio está organizado en las siguientes capas:

- **API:**  
  Punto de entrada HTTP. Define los controladores, middlewares y configuración de la aplicación (Swagger, autenticación, autorización).

- **Application:**  
  Lógica de negocio y casos de uso. Define contratos, servicios y DTOs.

- **Domain:**  
  Entidades y lógica de dominio. Contiene las reglas de negocio independientes de frameworks.

- **Infrastructure:**  
  Implementación de persistencia (Entity Framework Core), integración con servicios externos (HTTP clients, Polly para resiliencia), y detalles técnicos desacoplados mediante interfaces.

- **Tests:**  
  Pruebas unitarias e integración para garantizar la calidad y robustez de la solución.

## Características Técnicas

- **.NET 8 / .NET 9:** Compatibilidad con las últimas versiones de .NET.
- **Inyección de dependencias:** Uso extensivo de DI para desacoplar componentes.
- **Resiliencia:** Polly para reintentos y circuit breaker en llamadas HTTP.
- **Swagger:** Documentación automática de APIs.
- **Persistencia:** Entity Framework Core para acceso a datos.
- **Middlewares personalizados:** Manejo global de excepciones.

## Principios de Clean Architecture

- Separación clara de responsabilidades entre capas.
- Independencia de frameworks en el núcleo de negocio.
- Inversión de dependencias: la infraestructura y la API dependen de las capas internas, nunca al revés.
- Facilidad para realizar pruebas unitarias y de integración.

## Cómo ejecutar

1. Clona el repositorio.
2. Restaura los paquetes NuGet.
3. Configura las cadenas de conexión en los archivos de configuración.
4. Ejecuta los microservicios desde Visual Studio 2022 o mediante CLI.


Este proyecto es un MVP de una implementación moderna de arquitectura limpia y desacoplada en .NET, ideal para soluciones empresariales escalables.