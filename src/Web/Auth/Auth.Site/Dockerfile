FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base
WORKDIR /app
EXPOSE 5000

FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS build
WORKDIR /build

# Install Node.js and Yarn
RUN apk add --no-cache nodejs npm && \
    npm install -g yarn


# Copy project files
COPY ["src/Web/Auth/Auth.Site/Auth.Site.csproj", "src/Web/Auth/Auth.Site/"]
COPY ["src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj", "src/Modules/AlphaPlus.Core/"]
COPY ["src/Modules/AlphaPlus.Core.Authorization/AlphaPlus.Core.Authorization.csproj", "src/Modules/AlphaPlus.Core.Authorization/"]
COPY ["src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj", "src/Modules/AlphaPlus.Logging/"]
COPY ["src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj", "src/Modules/AlphaPlus.PartialUM/"]
COPY ["src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj", "src/Shared/AlphaPlus.Shared.AppResources/"]
COPY ["src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj", "src/Modules/AlphaPlus.RequestInfo/"]
COPY ["src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj", "src/Modules/AlphaPlus.Utilities.IP/"]
COPY ["src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj", "src/Modules/AlphaPlus.NetThrottling/"]

# Restore dependencies
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Auth/Auth.Site/Auth.Site.csproj" -v d

# copy all source code
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.Core.Authorization" "src/Modules/AlphaPlus.Core.Authorization"
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Shared/AlphaPlus.Shared.AppResources" "src/Shared/AlphaPlus.Shared.AppResources"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.NetThrottling" "src/Modules/AlphaPlus.NetThrottling"
COPY "src/Web/Auth/Auth.Site" "src/Web/Auth/Auth.Site"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Auth/Auth.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Auth/Auth.Site/sharedSettings.json; fi

ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /build/tmp
RUN if [ "$TRANSFORM_CONFIG" = "ALPHAKLPRODINT" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPRODINT2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPRODINT3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD3" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG2" ] \
  || [ "$TRANSFORM_CONFIG" = "ALPHAKLSTG3" ]; \
  then cd /build/tmp && curl --fail -o $TRANSFORM_CONFIG.domainmappings.json --header "PRIVATE-TOKEN:$CI_JOB_TOKEN" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F$TRANSFORM_CONFIG.domainmappings.json/raw?ref=main \
  && cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json src/Web/Auth/Auth.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
  then \
  cp "src/Web/Auth/Auth.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json" "src/Web/Auth/Auth.Site/Domains/domainmappings.json"; \
  fi

# Build
WORKDIR "/build/src/Web/Auth/Auth.Site"

# Install yarn dependencies and build
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Auth/Auth.Site/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Auth/Auth.Site/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn build

# Publish directly without build step
RUN dotnet publish "Auth.Site.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN cp -r /build/src/Web/Auth/Auth.Site/wwwroot /app/publish/wwwroot

# Final stage
FROM base AS final
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=build /app/publish .
ENV TZ="America/La_Paz"
ENTRYPOINT ["dotnet", "Auth.Site.dll"] 