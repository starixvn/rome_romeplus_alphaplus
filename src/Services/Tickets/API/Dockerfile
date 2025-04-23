FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /build

COPY "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "src/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "src/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"
COPY "src/Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj" "src/Services/Tickets/BetsForecast/Infrastructure/BetsForecast.Infrastructure.csproj"
COPY "src/Services/Tickets/API/Tickets.API.csproj" "src/Services/Tickets/API/Tickets.API.csproj"
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Services/Tickets/API/Tickets.API.csproj" -v d

COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Services/Tickets/BetsForecast/Domain" "src/Services/Tickets/BetsForecast/Domain"
COPY "src/Services/Tickets/BetsForecast/Infrastructure" "src/Services/Tickets/BetsForecast/Infrastructure"
COPY "src/Services/Tickets/API" "src/Services/Tickets/API"
COPY "src/Shared" "src/Shared"

WORKDIR /build/src/Services/Tickets/API
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.API.dll"]