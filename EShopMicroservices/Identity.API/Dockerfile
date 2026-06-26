# ================================
# STAGE 1 — BUILD
# Build context: ./EShopMicroservices
# ================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Identity.API/Identity.API.csproj                                             Identity.API/
COPY Identity.Core/Identity.Core.csproj                                           Identity.Core/
COPY Identity.Infrastructure/Identity.Infrastructure.csproj                       Identity.Infrastructure/
COPY EShopMicroservices.ServiceDefaults/EShopMicroservices.ServiceDefaults.csproj EShopMicroservices.ServiceDefaults/

RUN dotnet restore "Identity.API/Identity.API.csproj"

COPY Identity.API/                         Identity.API/
COPY Identity.Core/                        Identity.Core/
COPY Identity.Infrastructure/              Identity.Infrastructure/
COPY EShopMicroservices.ServiceDefaults/   EShopMicroservices.ServiceDefaults/

RUN dotnet publish "Identity.API/Identity.API.csproj" -c Release -o /app/publish --no-restore

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

LABEL maintainer="Vikas Gage"
LABEL org.opencontainers.image.title="Identity.API"
LABEL org.opencontainers.image.description="eShop Microservices — Identity Service"

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appgroup && useradd -r -g appgroup -m appuser
COPY --from=build --chown=appuser:appgroup /app/publish .

# NOTE: RS256 PEM keys are NOT baked into the image.
# In AKS (Stage 8) they are mounted at runtime via Kubernetes Secret:
#   /app/private.pem  → from secret: identity-pem-keys
#   /app/public.pem   → from secret: identity-pem-keys

USER appuser

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Identity.API.dll"]