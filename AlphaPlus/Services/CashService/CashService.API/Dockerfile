FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder
WORKDIR /src

COPY "AlphaPlus/Services/CashService/CashService.API/CashService.API.csproj" "AlphaPlus/Services/CashService/CashService.API/CashService.API.csproj"
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Services/CashService/CashService.API/CashService.API.csproj" --packages /root/.nuget/packages -v d

COPY "AlphaPlus/Services/CashService/CashService.API" "AlphaPlus/Services/CashService/CashService.API"
WORKDIR /src/AlphaPlus/Services/CashService/CashService.API
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# cache nuget
WORKDIR /root
COPY --from=builder /root/.nuget ./.nuget

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.CashService.API.dll"]
