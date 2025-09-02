# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos solo los csproj (manteniendo estructura de carpetas)
COPY src/ProductService/ProductService.Api/ProductService.Api.csproj ProductService/ProductService.Api/
COPY src/ProductService/ProductService.Application/ProductService.Application.csproj ProductService/ProductService.Application/
COPY src/ProductService/ProductService.Domain/ProductService.Domain.csproj ProductService/ProductService.Domain/
COPY src/ProductService/ProductService.Infrastructure/ProductService.Infrastructure.csproj ProductService/ProductService.Infrastructure/
COPY src/Common/Common.csproj Common/

# Restaurar dependencias
WORKDIR /src/ProductService/ProductService.Api
RUN dotnet restore

# Copiar el resto del código fuente
WORKDIR /src
COPY src/ .

# Publicamos en modo Release
WORKDIR /src/ProductService/ProductService.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.Api.dll"]