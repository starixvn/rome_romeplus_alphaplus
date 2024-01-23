FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj" -v n

COPY "Services/Identity/Identity.API" "Services/Identity/Identity.API"
COPY "Shared" "Shared"
WORKDIR /src/Services/Identity/Identity.API
RUN dotnet publish --no-restore -o /app

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