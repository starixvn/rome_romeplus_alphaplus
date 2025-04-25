FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder
WORKDIR /build

# Create a dedicated layer for NuGet packages
COPY "src/NuGet.config" "src/NuGet.config"
COPY "src/Services/CashService/CashService.API/CashService.API.csproj" "src/Services/CashService/CashService.API/CashService.API.csproj"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Services/CashService/CashService.API/CashService.API.csproj" -v d

# Copy the rest of the source code
COPY "src/Services/CashService/CashService.API" "src/Services/CashService/CashService.API"
WORKDIR /build/src/Services/CashService/CashService.API
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

# Don't run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.CashService.API.dll"]
