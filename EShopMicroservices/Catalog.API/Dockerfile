# ================================
# STAGE 1 — BUILD
# Build context: ./EShopMicroservices
# ================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Catalog.API/Catalog.API.csproj                                               Catalog.API/
COPY Catalog.Core/Catalog.Core.csproj                                             Catalog.Core/
COPY Catalog.Infrastructure/Catalog.Infrastructure.csproj                         Catalog.Infrastructure/
COPY EShop.Contracts/EShop.Contracts.csproj                                       EShop.Contracts/
COPY EShopMicroservices.ServiceDefaults/EShopMicroservices.ServiceDefaults.csproj EShopMicroservices.ServiceDefaults/

RUN dotnet restore "Catalog.API/Catalog.API.csproj"

COPY Catalog.API/                          Catalog.API/
COPY Catalog.Core/                         Catalog.Core/
COPY Catalog.Infrastructure/               Catalog.Infrastructure/
COPY EShop.Contracts/                      EShop.Contracts/
COPY EShopMicroservices.ServiceDefaults/   EShopMicroservices.ServiceDefaults/

RUN dotnet publish "Catalog.API/Catalog.API.csproj" -c Release -o /app/publish --no-restore

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

LABEL maintainer="Vikas Gage"
LABEL org.opencontainers.image.title="Catalog.API"
LABEL org.opencontainers.image.description="eShop Microservices — Catalog Service"

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appgroup && useradd -r -g appgroup -m appuser
COPY --from=build --chown=appuser:appgroup /app/publish .
USER appuser

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Catalog.API.dll"]