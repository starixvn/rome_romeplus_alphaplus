FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm && npm install -g yarn gulp-cli

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "AlphaPlus/Web/Main/Main.Site/Main.Site.csproj" "AlphaPlus/Web/Main/Main.Site/Main.Site.csproj"

# restore NuGet packages
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Main/Main.Site/Main.Site.csproj" -v n

# copy all source code
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"
COPY "AlphaPlus/Modules/AlphaPlus.Logging" "AlphaPlus/Modules/AlphaPlus.Logging"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling" "AlphaPlus/Modules/AlphaPlus.NetThrottling"
COPY "AlphaPlus/Web/Main/Main.Site" "AlphaPlus/Web/Main/Main.Site"

ARG TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/Main/Main.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Main/Main.Site/sharedSettings.json; fi

ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /src/tmp
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
    || [ "$TRANSFORM_CONFIG" = "STG" ] \
    || [ "$TRANSFORM_CONFIG" = "STG2" ] \
    || [ "$TRANSFORM_CONFIG" = "STG3" ] \
    || [ "$TRANSFORM_CONFIG" = "STG4" ] \
		|| [ "$TRANSFORM_CONFIG" = "STG5" ]; \
	then cd /src/tmp && curl --fail -o $TRANSFORM_CONFIG.domainmappings.json --header "PRIVATE-TOKEN:$CI_JOB_TOKEN" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F$TRANSFORM_CONFIG.domainmappings.json/raw?ref=main \
		&& cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json AlphaPlus/Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
then \
  cp "AlphaPlus/Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json" "AlphaPlus/Web/Main/Main.Site/Domains/domainmappings.json"; \
fi

WORKDIR /src/AlphaPlus/Web/Main/Main.Site

RUN yarn && yarn build
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Main.Site.dll"]
