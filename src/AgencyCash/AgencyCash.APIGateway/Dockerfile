FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder
WORKDIR /build

# Copy only necessary files for restoring dependencies
COPY ["src/NuGet.config", "src/"]
COPY ["src/AgencyCash/AgencyCash.APIGateway/AgencyCash.APIGateway.csproj", "src/AgencyCash/AgencyCash.APIGateway/AgencyCash.APIGateway.csproj"]
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore "src/AgencyCash/AgencyCash.APIGateway/AgencyCash.APIGateway.csproj" -v d

# Copy the entire project and publish it
COPY ["src/AgencyCash/AgencyCash.APIGateway", "src/AgencyCash/AgencyCash.APIGateway"]
COPY ["src/Shared/Extensions/FanexLoggingExtensions.cs", "src/Shared/Extensions/FanexLoggingExtensions.cs"]
WORKDIR /build/src/AgencyCash/AgencyCash.APIGateway
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet publish -c Release -r linux-x64 -o /app --no-restore

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

# Create and use a non-root user for security
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && \
  chown -R dotnetuser /app /tmp
USER dotnetuser

WORKDIR /app
COPY --from=builder /app ./

# Configure environment variables
ENV ASPNETCORE_URLS=http://0.0.0.0:5000 \
  TZ="America/La_Paz" \
  COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "AgencyCash.APIGateway.dll"]
