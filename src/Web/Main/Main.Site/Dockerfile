FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /build

# copy .csproj files
COPY "src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "src/Web/Main/Main.Site/Main.Site.csproj" "src/Web/Main/Main.Site/Main.Site.csproj"

# restore NuGet packages
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Main/Main.Site/Main.Site.csproj" -v d

# copy all source code
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.SecurityCode" "src/Modules/AlphaPlus.SecurityCode"
COPY "src/Modules/AlphaPlus.NetThrottling" "src/Modules/AlphaPlus.NetThrottling"
COPY "src/Web/Main/Main.Site" "src/Web/Main/Main.Site"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Main/Main.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Main/Main.Site/sharedSettings.json; fi

ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /build/tmp
RUN if [ "$TRANSFORM_CONFIG" = "PRO" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO2" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO3" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO4" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO5" ] \
  || [ "$TRANSFORM_CONFIG" = "INT" ] \
  || [ "$TRANSFORM_CONFIG" = "INT2" ] \
  || [ "$TRANSFORM_CONFIG" = "INT3" ] \
  || [ "$TRANSFORM_CONFIG" = "INT4" ] \
  || [ "$TRANSFORM_CONFIG" = "INT5" ] \
  || [ "$TRANSFORM_CONFIG" = "DR" ] \
  || [ "$TRANSFORM_CONFIG" = "DR2" ] \
  || [ "$TRANSFORM_CONFIG" = "DR3" ] \
  || [ "$TRANSFORM_CONFIG" = "DR4" ] \
  || [ "$TRANSFORM_CONFIG" = "DR5" ] \
  || [ "$TRANSFORM_CONFIG" = "STG" ] \
  || [ "$TRANSFORM_CONFIG" = "STG2" ] \
  || [ "$TRANSFORM_CONFIG" = "STG3" ] \
  || [ "$TRANSFORM_CONFIG" = "STG4" ] \
  || [ "$TRANSFORM_CONFIG" = "STG5" ]; \
  then cd /build/tmp && curl --fail -o $TRANSFORM_CONFIG.domainmappings.json --header "PRIVATE-TOKEN:$CI_JOB_TOKEN" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F$TRANSFORM_CONFIG.domainmappings.json/raw?ref=main \
  && cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json src/Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
  then \
  cp "src/Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json" "src/Web/Main/Main.Site/Domains/domainmappings.json"; \
  fi

WORKDIR /build/src/Web/Main/Main.Site

# Install yarn dependencies and build
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Main/Main.Site/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Main/Main.Site/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn build

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# Don’t run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.Main.Site.dll"]
