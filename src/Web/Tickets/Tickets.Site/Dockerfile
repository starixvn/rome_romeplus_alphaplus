FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /src

COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "src/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "src/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"

COPY "src/Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj" "src/Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj"
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj" "src/Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj"
COPY "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "src/Services/Tickets/BetLists/AlphaPlus.BetLists.csproj" "src/Services/Tickets/BetLists/AlphaPlus.BetLists.csproj"
COPY "src/Services/Tickets/Results/AlphaPlus.Results.csproj" "src/Services/Tickets/Results/AlphaPlus.Results.csproj"

COPY "src/Web/Tickets/BetLists.Builders/BetLists.Builders.csproj" "src/Web/Tickets/BetLists.Builders/BetLists.Builders.csproj"
COPY "src/Web/Tickets/Tickets.Site/Tickets.Site.csproj" "src/Web/Tickets/Tickets.Site/Tickets.Site.csproj"
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Tickets/Tickets.Site/Tickets.Site.csproj" -v d

COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.NetThrottling" "src/Modules/AlphaPlus.NetThrottling"
COPY "src/Services/Tickets/BetsForecast/Domain" "src/Services/Tickets/BetsForecast/Domain"

COPY "src/Modules/AlphaPlus.IPInfo" "src/Modules/AlphaPlus.IPInfo"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.SmartPunter" "src/Modules/AlphaPlus.SmartPunter"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Modules/AlphaPlus.SecurityCode" "src/Modules/AlphaPlus.SecurityCode"
COPY "src/Services/Tickets/BetLists" "src/Services/Tickets/BetLists"
COPY "src/Services/Tickets/Results" "src/Services/Tickets/Results"

COPY "src/Web/Tickets/BetLists.Builders" "src/Web/Tickets/BetLists.Builders"
COPY "src/Web/Tickets/Tickets.Site" "src/Web/Tickets/Tickets.Site/"
COPY "src/Shared" "src/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Tickets/Tickets.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Tickets/Tickets.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" = "SIT" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT2" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT3" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT4" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT5" ]; then \
    cp -r src/Web/Tickets/Tickets.Site/results.SIT.json src/Web/Tickets/Tickets.Site/results.json; \
fi

RUN if [ "${TRANSFORM_CONFIG}" = "UAT" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT2" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT3" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT4" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT5" ]; then \
    cp -r src/Web/Tickets/Tickets.Site/results.UAT.json src/Web/Tickets/Tickets.Site/results.json; \
fi

WORKDIR /src/src/Web/Tickets/Tickets.Site

# copy package files for better caching
COPY "src/Web/Tickets/Tickets.Site/package.json" "src/Web/Tickets/Tickets.Site/package.json"
COPY "src/Web/Tickets/Tickets.Site/yarn.lock" "src/Web/Tickets/Tickets.Site/yarn.lock"

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/src/src/Web/Tickets/Tickets.Site/node_modules \  
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    gulp default

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
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.Site.dll"]
