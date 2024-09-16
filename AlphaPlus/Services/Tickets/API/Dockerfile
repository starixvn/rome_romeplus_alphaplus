FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "AlphaPlus/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj" "AlphaPlus/Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj"
COPY "AlphaPlus/Services/Tickets/API/Tickets.API.csproj" "AlphaPlus/Services/Tickets/API/Tickets.API.csproj"
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Services/Tickets/API/Tickets.API.csproj" -v n

COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Domain" "AlphaPlus/Services/Tickets/BetsForecast/Domain"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Infrastructure" "AlphaPlus/Services/Tickets/BetsForecast/Infrastructure"
COPY "AlphaPlus/Services/Tickets/API" "AlphaPlus/Services/Tickets/API"
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"

WORKDIR /src/AlphaPlus/Services/Tickets/API
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

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.API.dll"]