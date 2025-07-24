FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder
WORKDIR /client
COPY /src/Web/CashService/CashService.Site/ClientApp .
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    yarn build

FROM builder AS server-build
WORKDIR /build

# copy .csproj files
COPY "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "src/Web/CashService/CashService.Site/CashService.Site.csproj" "src/Web/CashService/CashService.Site/CashService.Site.csproj"

# Restore NuGet packages with cache
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/CashService/CashService.Site/CashService.Site.csproj" -v d

# copy all source code
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.SecurityCode" "src/Modules/AlphaPlus.SecurityCode"
COPY "src/Modules/AlphaPlus.NetThrottling" "src/Modules/AlphaPlus.NetThrottling"
COPY "src/Web/CashService/CashService.Site" "src/Web/CashService/CashService.Site"

ARG TRANSFORM_CONFIG

# Handle configuration files
RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then \
    cp -r src/Shared/Configuration/sharedSettings.json src/Web/CashService/CashService.Site/sharedSettings.json; \
    else \
    cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/CashService/CashService.Site/sharedSettings.json; \
    fi

# Copy the client build output
COPY --from=builder /client/dist /build/src/Web/CashService/CashService.Site/wwwroot

WORKDIR /build/src/Web/CashService/CashService.Site
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# Set up non-root user
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser

# Configure the application
ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENV TZ="America/La_Paz"

WORKDIR /app
COPY --from=server-build /app ./

# Create wwwroot directory and set proper permissions
RUN mkdir -p /app/wwwroot/PromotionImages && \
    chown -R dotnetuser:dotnetuser /app && \
    chmod -R 755 /app/wwwroot && \
    chmod -R 777 /app/wwwroot/PromotionImages

USER dotnetuser

ENTRYPOINT ["dotnet", "AlphaPlus.CashService.Site.dll"]
