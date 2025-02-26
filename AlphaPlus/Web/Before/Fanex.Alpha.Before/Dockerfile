FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm && npm install -g yarn gulp-cli

WORKDIR /src
EXPOSE 5000

COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" "AlphaPlus/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj"
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" --packages /root/.nuget/packages -v d

COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Web/Before/Fanex.Alpha.Before" "AlphaPlus/Web/Before/Fanex.Alpha.Before"

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
		&& cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json AlphaPlus/Web/Before/Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
then \
  cp "AlphaPlus/Web/Before/Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json" "AlphaPlus/Web/Before/Fanex.Alpha.Before/domainmappings.json"; \
fi

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
  || [ "$TRANSFORM_CONFIG" = "SIT2" ] \
  || [ "$TRANSFORM_CONFIG" = "UAT2" ] \
  || [ "$TRANSFORM_CONFIG" = "STG2" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO2" ] \
  || [ "$TRANSFORM_CONFIG" = "DR2" ]; \
then \
	cp "AlphaPlus/Web/Before/Fanex.Alpha.Before/wwwroot/favicon2.ico" "AlphaPlus/Web/Before/Fanex.Alpha.Before/wwwroot/favicon.ico"; \
  cp "AlphaPlus/Web/Before/Fanex.Alpha.Before/bundleconfig2.json" "AlphaPlus/Web/Before/Fanex.Alpha.Before/bundleconfig.json"; \
fi

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL3" ] \
  || [ "$TRANSFORM_CONFIG" = "SIT3" ] \
  || [ "$TRANSFORM_CONFIG" = "UAT3" ] \
  || [ "$TRANSFORM_CONFIG" = "STG3" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO3" ] \
  || [ "$TRANSFORM_CONFIG" = "DR3" ]; \
then \
  cp "AlphaPlus/Web/Before/Fanex.Alpha.Before/bundleconfig3.json" "AlphaPlus/Web/Before/Fanex.Alpha.Before/bundleconfig.json"; \
fi

WORKDIR /src/AlphaPlus/Web/Before/Fanex.Alpha.Before
RUN yarn && yarn build

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
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Alpha.Before.dll"]