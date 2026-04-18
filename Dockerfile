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

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "EShop.API.dll"]