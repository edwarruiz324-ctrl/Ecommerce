# Guía de Ejecución de Pruebas y Despliegue Local

Esta guía describe cómo ejecutar pruebas unitarias, generar reportes de cobertura y levantar la solución mediante Docker para los proyectos **OrderService** y **ProductService**.

---

## 1. Ejecutar Pruebas Unitarias

Las siguientes instrucciones aplican para ambos proyectos de test.

### 1.1 Rutas de los proyectos de prueba

cd D:\Ecommerce\Ecommerce\tests\OrderService.Tests
cd D:\Ecommerce\Ecommerce\tests\ProductService.Tests

### 1.2 Ejecutar pruebas con cobertura de código

dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
El archivo coverlet.runsettings debe estar configurado para excluir migraciones y snapshots de EF.

### 1.3 Instalar ReportGenerator (si no está instalado)

dotnet tool install -g dotnet-reportgenerator-globaltool

### 1.4 Generar reporte de cobertura en formato HTML
Se recomienda ejecutar desde la carpeta raíz de tests:

cd D:\Ecommerce\Ecommerce\tests

reportgenerator -reports:"*/TestResults/*/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html

El reporte HTML se generará en ./coverage-report/index.html.

## 2. Ejecutar la Solución

### 2.1 Compilar la solución
Abrir la solución en Visual Studio.

Compilar la solución.

###  Requisitos
Docker Desktop para Windows instalado y en ejecución.

### 2.3 Comandos Docker

#### Limpiar recursos
docker builder prune -af

#### Construir imágenes

docker compose build --no-cache

#### Levantar contenedores en segundo plano

docker compose up -d

#### Detener y eliminar contenedores

docker compose down

#### Reconstruir y levantar contenedores

docker compose up -d --build

## 3. Acceso a las APIs

Una vez levantados los contenedores, las APIs estarán disponibles en:

OrderService: http://localhost:5001/swagger/index.html

ProductService: http://localhost:5002/swagger/index.html

## 4. Notas

Asegúrese de ejecutar los comandos con permisos de administrador si es necesario.

Las rutas de los proyectos pueden variar según la instalación local; ajustar según corresponda.

Para pruebas de cobertura, asegúrese de que coverlet.runsettings está configurado correctamente para excluir migraciones y snapshots.



# Arquitectura del Proyecto Ecommerce (.NET 8)

---

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


