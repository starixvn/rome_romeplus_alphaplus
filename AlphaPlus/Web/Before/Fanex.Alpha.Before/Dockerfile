FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src
EXPOSE 5000

COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" "Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Before/Fanex.Alpha.Before/Fanex.Alpha.Before.csproj" -v n

COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Web/Before/Fanex.Alpha.Before" "Web/Before/Fanex.Alpha.Before"

ARG TRANSFORM_CONFIG
ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /src/tmp
RUN if [ "$TRANSFORM_CONFIG" = "PRO" ] \
		|| [ "$TRANSFORM_CONFIG" = "PRO2" ] \
    || [ "$TRANSFORM_CONFIG" = "PRO3" ] \
    || [ "$TRANSFORM_CONFIG" = "PRO5" ] \
    || [ "$TRANSFORM_CONFIG" = "DR" ] \
    || [ "$TRANSFORM_CONFIG" = "DR2" ] \
    || [ "$TRANSFORM_CONFIG" = "DR3" ] \
    || [ "$TRANSFORM_CONFIG" = "DR4" ] \
    || [ "$TRANSFORM_CONFIG" = "DR5" ] \
    || [ "$TRANSFORM_CONFIG" = "STG" ] \
    || [ "$TRANSFORM_CONFIG" = "STG2" ] \
		|| [ "$TRANSFORM_CONFIG" = "STG3" ]; \
	then cd /src/tmp && curl --fail -o $TRANSFORM_CONFIG.domainmappings.json --header "PRIVATE-TOKEN:$CI_JOB_TOKEN" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F$TRANSFORM_CONFIG.domainmappings.json/raw?ref=main \
		&& cd - && cp -r tmp/$TRANSFORM_CONFIG.domainmappings.json Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json; fi

RUN if [ "$TRANSFORM_CONFIG" != "" ]; \
then \
  cp "Web/Before/Fanex.Alpha.Before/domainmappings.$TRANSFORM_CONFIG.json" "Web/Before/Fanex.Alpha.Before/domainmappings.json"; \
fi

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
  || [ "$TRANSFORM_CONFIG" = "SIT2" ] \
  || [ "$TRANSFORM_CONFIG" = "UAT2" ] \
  || [ "$TRANSFORM_CONFIG" = "STG2" ] \
  || [ "$TRANSFORM_CONFIG" = "PRO2" ] \
  || [ "$TRANSFORM_CONFIG" = "AWS2" ] \
  || [ "$TRANSFORM_CONFIG" = "DR2" ]; \
then \
	cp "Web/Before/Fanex.Alpha.Before/wwwroot/favicon2.ico" "Web/Before/Fanex.Alpha.Before/wwwroot/favicon.ico"; \
fi

WORKDIR /src/Web/Before/Fanex.Alpha.Before
RUN dotnet publish -r linux-x64 --self-contained false -o /app

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

ENTRYPOINT ["dotnet", "Fanex.Alpha.Before.dll"]