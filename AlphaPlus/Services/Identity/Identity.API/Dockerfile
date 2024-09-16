FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY "AlphaPlus/Services/Identity/Identity.API/Identity.API.csproj" "AlphaPlus/Services/Identity/Identity.API/Identity.API.csproj"
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Services/Identity/Identity.API/Identity.API.csproj" -v n

COPY "AlphaPlus/Services/Identity/Identity.API" "AlphaPlus/Services/Identity/Identity.API"
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"
WORKDIR /src/AlphaPlus/Services/Identity/Identity.API
RUN dotnet publish -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Identity.API.dll"]