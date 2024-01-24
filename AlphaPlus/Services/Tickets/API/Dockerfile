FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY "Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"
COPY "Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj" "Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj"
COPY "Services/Tickets/API/Tickets.API.csproj" "Services/Tickets/API/Tickets.API.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Services/Tickets/API/Tickets.API.csproj" -v n

COPY "Services/Tickets/BetsForecast/Domain" "Services/Tickets/BetsForecast/Domain"
COPY "Services/Tickets/BetsForecast/Infrastructure" "Services/Tickets/BetsForecast/Infrastructure"
COPY "Services/Tickets/API" "Services/Tickets/API"
COPY "Shared" "Shared"

WORKDIR /src/Services/Tickets/API
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

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.API.dll"]