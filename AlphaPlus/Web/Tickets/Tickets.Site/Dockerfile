FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm && npm install -g yarn gulp-cli

WORKDIR /src

COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "AlphaPlus/Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"

COPY "AlphaPlus/Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj" "AlphaPlus/Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj" "AlphaPlus/Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Services/Tickets/BetLists/AlphaPlus.BetLists.csproj" "AlphaPlus/Services/Tickets/BetLists/AlphaPlus.BetLists.csproj"
COPY "AlphaPlus/Services/Tickets/Results/AlphaPlus.Results.csproj" "AlphaPlus/Services/Tickets/Results/AlphaPlus.Results.csproj"

COPY "AlphaPlus/Web/Tickets/BetLists.Builders/BetLists.Builders.csproj" "AlphaPlus/Web/Tickets/BetLists.Builders/BetLists.Builders.csproj"
COPY "AlphaPlus/Web/Tickets/Tickets.Site/Tickets.Site.csproj" "AlphaPlus/Web/Tickets/Tickets.Site/Tickets.Site.csproj"
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Tickets/Tickets.Site/Tickets.Site.csproj" --packages /root/.nuget/packages -v d

COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling" "AlphaPlus/Modules/AlphaPlus.NetThrottling"
COPY "AlphaPlus/Services/Tickets/BetsForecast/Domain" "AlphaPlus/Services/Tickets/BetsForecast/Domain"

COPY "AlphaPlus/Modules/AlphaPlus.IPInfo" "AlphaPlus/Modules/AlphaPlus.IPInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.SmartPunter" "AlphaPlus/Modules/AlphaPlus.SmartPunter"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Services/Tickets/BetLists" "AlphaPlus/Services/Tickets/BetLists"
COPY "AlphaPlus/Services/Tickets/Results" "AlphaPlus/Services/Tickets/Results"

COPY "AlphaPlus/Web/Tickets/BetLists.Builders" "AlphaPlus/Web/Tickets/BetLists.Builders"
COPY "AlphaPlus/Web/Tickets/Tickets.Site" "AlphaPlus/Web/Tickets/Tickets.Site/"
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/Tickets/Tickets.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Tickets/Tickets.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" = "SIT" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT2" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT3" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT4" ] || \
  [ "${TRANSFORM_CONFIG}" = "SIT5" ]; then \
    cp -r AlphaPlus/Web/Tickets/Tickets.Site/results.SIT.json AlphaPlus/Web/Tickets/Tickets.Site/results.json; \
fi

RUN if [ "${TRANSFORM_CONFIG}" = "UAT" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT2" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT3" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT4" ] || \
  [ "${TRANSFORM_CONFIG}" = "UAT5" ]; then \
    cp -r AlphaPlus/Web/Tickets/Tickets.Site/results.UAT.json AlphaPlus/Web/Tickets/Tickets.Site/results.json; \
fi

WORKDIR /src/AlphaPlus/Web/Tickets/Tickets.Site

RUN yarn && gulp default
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# cache nuget
WORKDIR /root
COPY --from=builder /root/.nuget ./.nuget

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.Site.dll"]
