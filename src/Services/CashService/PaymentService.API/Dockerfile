FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder
WORKDIR /build

# Copy only necessary files for restoring dependencies
COPY ["src/NuGet.config", "src/NuGet.config"]
COPY ["src/Services/CashService/PaymentService.API/PaymentService.API.csproj", "src/Services/CashService/PaymentService.API/PaymentService.API.csproj"]
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore "src/Services/CashService/PaymentService.API/PaymentService.API.csproj" -v d

# Copy the entire project and publish it
COPY ["src/Services/CashService/PaymentService.API", "src/Services/CashService/PaymentService.API"]
COPY ["src/Shared/Extensions/FanexLoggingExtensions.cs", "src/Shared/Extensions/FanexLoggingExtensions.cs"]
COPY ["src/Shared/ExceptionHelper.cs", "src/Shared/ExceptionHelper.cs"]
WORKDIR /build/src/Services/CashService/PaymentService.API
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet publish -c Release -o /app --no-restore

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

ENTRYPOINT ["dotnet", "AlphaPlus.PaymentService.API.dll"]
