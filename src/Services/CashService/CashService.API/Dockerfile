FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder
WORKDIR /build

# Create a dedicated layer for NuGet packages with improved caching
COPY "src/NuGet.config" "src/NuGet.config"
COPY "src/Services/CashService/CashService.API/CashService.API.csproj" "src/Services/CashService/CashService.API/CashService.API.csproj"
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore "src/Services/CashService/CashService.API/CashService.API.csproj" -v d

# Copy only the necessary source code and publish
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Shared" "src/Modules/AlphaPlus.Shared"
COPY "src/Services/CashService/CashService.API" "src/Services/CashService/CashService.API"
WORKDIR /build/src/Services/CashService/CashService.API
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

# Set environment variables in a single layer
ENV ASPNETCORE_URLS=http://0.0.0.0:5000 \
  TZ="America/La_Paz" \
  COMPlus_EnableDiagnostics=0

# Don't run as root (security best practice)
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp

WORKDIR /app
COPY --from=builder /app ./

# Switch to non-root user
USER dotnetuser
ENTRYPOINT ["dotnet", "AlphaPlus.CashService.API.dll"]
