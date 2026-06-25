# ================================
# STAGE 1 — BUILD
# Build context: ./EShopMicroservices
# ================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Customer.API/Customer.API.csproj                                             Customer.API/
COPY Customer.Core/Customer.Core.csproj                                           Customer.Core/
COPY Customer.Infrastructure/Customer.Infrastructure.csproj                       Customer.Infrastructure/
COPY EShop.Contracts/EShop.Contracts.csproj                                       EShop.Contracts/
COPY EShopMicroservices.ServiceDefaults/EShopMicroservices.ServiceDefaults.csproj EShopMicroservices.ServiceDefaults/

RUN dotnet restore "Customer.API/Customer.API.csproj"

COPY Customer.API/                         Customer.API/
COPY Customer.Core/                        Customer.Core/
COPY Customer.Infrastructure/              Customer.Infrastructure/
COPY EShop.Contracts/                      EShop.Contracts/
COPY EShopMicroservices.ServiceDefaults/   EShopMicroservices.ServiceDefaults/

RUN dotnet publish "Customer.API/Customer.API.csproj" -c Release -o /app/publish --no-restore

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

LABEL maintainer="Vikas Gage"
LABEL org.opencontainers.image.title="Customer.API"
LABEL org.opencontainers.image.description="eShop Microservices — Customer Service"

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appgroup && useradd -r -g appgroup -m appuser
COPY --from=build --chown=appuser:appgroup /app/publish .
USER appuser

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Customer.API.dll"]
