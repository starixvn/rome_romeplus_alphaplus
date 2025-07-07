FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /build
EXPOSE 5000

COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" "src/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj"
COPY "src/NuGet.config" "src/NuGet.config"

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" -v d

COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Web/Before/Fanex.Alpha.Before" "src/Web/Before/Fanex.Alpha.Before"

ARG TRANSFORM_CONFIG
ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /build/tmp
RUN if [ "$TRANSFORM_CONFIG" = "PRO" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO2" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO3" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO4" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO5" ] \
  || [ "$TRANSFORM_CONFIG" = "DR" ] \
  || [ "$TRANSFORM_CONFIG" = "DR2" ] \
  || [ "$TRANSFORM_CONFIG" = "DR3" ] \
  || [ "$TRANSFORM_CONFIG" = "DR4" ] \
  || [ "$TRANSFORM_CONFIG" = "DR5" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD4" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD5" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE4" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE5" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG4" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG5" ] \
  || [ "$TRANSFORM_CONFIG" = "STG" ] \
  || [ "$TRANSFORM_CONFIG" = "STG2" ] \
  || [ "$TRANSFORM_CONFIG" = "STG3" ] \
  || [ "$TRANSFORM_CONFIG" = "STG4" ] \
  || [ "$TRANSFORM_CONFIG" = "STG5" ]; \
  then cd /build/tmp && curl --fail -o $TRANSFORM_CONFIG.domainmappings.json --header "PRIVATE-TOKEN:$CI_JOB_TOKEN" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F$TRANSFORM_CONFIG.domainmappings.json/raw?ref=main \
  && cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json src/Web/Before/Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
  then \
  cp "src/Web/Before/Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json" "src/Web/Before/Fanex.Alpha.Before/domainmappings.json"; \
  fi

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
  || [ "$TRANSFORM_CONFIG" = "SIT2" ] \
  || [ "$TRANSFORM_CONFIG" = "UAT2" ] \
  || [ "$TRANSFORM_CONFIG" = "STG2" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE2" ] \
  || [ "$TRANSFORM_CONFIG" = "DR2" ]; \
  then \
  cp "src/Web/Before/Fanex.Alpha.Before/wwwroot/favicon2.ico" "src/Web/Before/Fanex.Alpha.Before/wwwroot/favicon.ico"; \
  cp "src/Web/Before/Fanex.Alpha.Before/bundleconfig2.json" "src/Web/Before/Fanex.Alpha.Before/bundleconfig.json"; \
  fi

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL3" ] \
  || [ "$TRANSFORM_CONFIG" = "SIT3" ] \
  || [ "$TRANSFORM_CONFIG" = "UAT3" ] \
  || [ "$TRANSFORM_CONFIG" = "STG3" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE3" ] \
  || [ "$TRANSFORM_CONFIG" = "DR3" ]; \
  then \
  cp "src/Web/Before/Fanex.Alpha.Before/bundleconfig3.json" "src/Web/Before/Fanex.Alpha.Before/bundleconfig.json"; \
  fi

WORKDIR /build/src/Web/Before/Fanex.Alpha.Before
# Install yarn dependencies
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Before/Fanex.Alpha.Before/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Before/Fanex.Alpha.Before/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn build

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app

COPY src/Web/Before/Fanex.Alpha.Before/arialbd.ttf src/Web/Before/Fanex.Alpha.Before/
RUN mkdir -p /usr/share/fonts/truetype/ && \
    install -m644 src/Web/Before/Fanex.Alpha.Before/arialbd.ttf /usr/share/fonts/truetype/ && \
    rm src/Web/Before/Fanex.Alpha.Before/arialbd.ttf

# Don't run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Alpha.Before.dll"]