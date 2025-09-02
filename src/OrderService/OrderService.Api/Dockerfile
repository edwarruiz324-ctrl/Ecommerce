# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos solo los csproj (manteniendo estructura de carpetas)
COPY src/OrderService/OrderService.Api/OrderService.Api.csproj OrderService/OrderService.Api/
COPY src/OrderService/OrderService.Application/OrderService.Application.csproj OrderService/OrderService.Application/
COPY src/OrderService/OrderService.Domain/OrderService.Domain.csproj OrderService/OrderService.Domain/
COPY src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj OrderService/OrderService.Infrastructure/
COPY src/Common/Common.csproj Common/

# Restaurar dependencias
WORKDIR /src/OrderService/OrderService.Api
RUN dotnet restore

# Copiar el resto del código fuente
WORKDIR /src
COPY src/ .

# Publicamos en modo Release
WORKDIR /src/OrderService/OrderService.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
