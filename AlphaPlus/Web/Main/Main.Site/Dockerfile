FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

WORKDIR /src

# copy .csproj files
COPY "Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Web/Main/Main.Site/Main.Site.csproj" "Web/Main/Main.Site/Main.Site.csproj"

# restore NuGet packages
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Main/Main.Site/Main.Site.csproj" -v n

# copy all source code
COPY "Shared" "Shared"
COPY "Modules/AlphaPlus.Logging" "Modules/AlphaPlus.Logging"
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"
COPY "Web/Main/Main.Site" "Web/Main/Main.Site"

ARG TRANSFORM_CONFIG
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
		&& cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
then \
  cp "Web/Main/Main.Site/Domains/domainmappings.$TRANSFORM_CONFIG.json" "Web/Main/Main.Site/Domains/domainmappings.json"; \
fi

WORKDIR /src/Web/Main/Main.Site

RUN yarn && yarn build
RUN dotnet publish --use-current-runtime --self-contained -o /app

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
