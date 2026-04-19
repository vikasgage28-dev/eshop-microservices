# ================================
# STAGE 1 — BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy ALL project files first (for layer caching)
COPY EShop.API/EShop.API.csproj                        EShop.API/
COPY EShop.Core/EShop.Core.csproj                      EShop.Core/
COPY EShop.Infrastructure/EShop.Infrastructure.csproj  EShop.Infrastructure/
COPY EShop.Shared/EShop.Shared.csproj                  EShop.Shared/

# Restore ONLY API project (pulls all dependencies!)
RUN dotnet restore "EShop.API/EShop.API.csproj"

# Copy remaining source code
COPY EShop.API/            EShop.API/
COPY EShop.Core/           EShop.Core/
COPY EShop.Infrastructure/ EShop.Infrastructure/
COPY EShop.Shared/         EShop.Shared/

# Build and publish in Release mode
WORKDIR /src/EShop.API
RUN dotnet publish -c Release -o /app/publish

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Image metadata labels
LABEL maintainer="Vikas Gage"
LABEL org.opencontainers.image.title="EShop API"
LABEL org.opencontainers.image.description="EShop Microservices API"
LABEL org.opencontainers.image.version="1.0.0"
LABEL org.opencontainers.image.authors="Vikas Gage"
LABEL org.opencontainers.image.source="https://github.com/vikasgage28-dev/eshop-microservices"

# Install curl for health checks
RUN apt-get update && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

# Copy published files with correct ownership
COPY --from=build --chown=appuser:appgroup /app/publish .

# Switch to non-root user BEFORE running app!
USER appuser

EXPOSE 80

# Health check to verify app is running
HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "EShop.API.dll"]