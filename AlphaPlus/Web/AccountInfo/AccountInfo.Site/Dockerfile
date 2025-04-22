FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" "AlphaPlus/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj"

COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"

# Restore NuGet packages with cache
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "AlphaPlus/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" -v d

# copy all source code
COPY "AlphaPlus/Modules/AlphaPlus.Logging" "AlphaPlus/Modules/AlphaPlus.Logging"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Web/AccountInfo/AccountInfo.Site" "AlphaPlus/Web/AccountInfo/AccountInfo.Site"
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/AccountInfo/AccountInfo.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/AccountInfo/AccountInfo.Site/sharedSettings.json; fi

# Copy package files for better caching
COPY "AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp/package.json" "AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp/package.json"
COPY "AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp/yarn.lock" "AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp/yarn.lock"

WORKDIR /src/AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp
# Install, build and copy dist files
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/src/AlphaPlus/Web/AccountInfo/AccountInfo.Site/ClientApp/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile --prefer-offline && \
    yarn build && \
    mkdir -p /src/AlphaPlus/Web/AccountInfo/AccountInfo.Site/wwwroot && \
    cp -r dist/* /src/AlphaPlus/Web/AccountInfo/AccountInfo.Site/wwwroot

WORKDIR /src/AlphaPlus/Web/AccountInfo/AccountInfo.Site
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS final

# Don't run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.AccountInfo.Site.dll"]