FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /src

# copy .csproj files
COPY "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "src/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" "src/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj"

COPY "src/NuGet.config" "src/NuGet.config"

# Restore NuGet packages with cache
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" -v d

# copy all source code
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.SecurityCode" "src/Modules/AlphaPlus.SecurityCode"
COPY "src/Web/AccountInfo/AccountInfo.Site" "src/Web/AccountInfo/AccountInfo.Site"
COPY "src/Shared" "src/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/AccountInfo/AccountInfo.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/AccountInfo/AccountInfo.Site/sharedSettings.json; fi

# Copy package files for better caching
COPY "src/Web/AccountInfo/AccountInfo.Site/ClientApp/package.json" "src/Web/AccountInfo/AccountInfo.Site/ClientApp/package.json"
COPY "src/Web/AccountInfo/AccountInfo.Site/ClientApp/yarn.lock" "src/Web/AccountInfo/AccountInfo.Site/ClientApp/yarn.lock"

WORKDIR /src/src/Web/AccountInfo/AccountInfo.Site/ClientApp
# Install, build and copy dist files
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/src/src/Web/AccountInfo/AccountInfo.Site/ClientApp/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile --prefer-offline && \
    yarn build && \
    mkdir -p /src/src/Web/AccountInfo/AccountInfo.Site/wwwroot && \
    cp -r dist/* /src/src/Web/AccountInfo/AccountInfo.Site/wwwroot

WORKDIR /src/src/Web/AccountInfo/AccountInfo.Site
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