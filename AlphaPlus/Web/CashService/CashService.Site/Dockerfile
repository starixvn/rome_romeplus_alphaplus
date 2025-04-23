FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder
WORKDIR /client
COPY /AlphaPlus/Web/CashService/CashService.Site/ClientApp .
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    yarn build

FROM builder AS server-build
WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj" "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj"

# Restore NuGet packages with cache
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj" -v d

# copy all source code
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"
COPY "AlphaPlus/Modules/AlphaPlus.Logging" "AlphaPlus/Modules/AlphaPlus.Logging"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Web/CashService/CashService.Site" "AlphaPlus/Web/CashService/CashService.Site"

ARG TRANSFORM_CONFIG

# Handle configuration files
RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then \
    cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/CashService/CashService.Site/sharedSettings.json; \
    else \
    cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/CashService/CashService.Site/sharedSettings.json; \
    fi

# Copy the client build output
COPY --from=builder /client/dist /src/AlphaPlus/Web/CashService/CashService.Site/wwwroot

WORKDIR /src/AlphaPlus/Web/CashService/CashService.Site
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# Set up non-root user
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# Configure the application
ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENV TZ="America/La_Paz"

WORKDIR /app
COPY --from=server-build /app ./

ENTRYPOINT ["dotnet", "AlphaPlus.CashService.Site.dll"]
