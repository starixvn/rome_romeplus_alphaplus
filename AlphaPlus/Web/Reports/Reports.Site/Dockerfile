FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "AlphaPlus/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "AlphaPlus/Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj" "AlphaPlus/Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj"

COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "AlphaPlus/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "AlphaPlus/Web/Reports/Reports.Site/Reports.Site.csproj" "AlphaPlus/Web/Reports/Reports.Site/Reports.Site.csproj"

# restore NuGet packages
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Reports/Reports.Site/Reports.Site.csproj" --packages /root/.nuget/packages -v d

# copy all source code
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Services/Reports/AlphaPlus.Reports" "AlphaPlus/Services/Reports/AlphaPlus.Reports"

COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Modules/AlphaPlus.NetThrottling" "AlphaPlus/Modules/AlphaPlus.NetThrottling"
COPY "AlphaPlus/Web/Reports/Reports.Site" "AlphaPlus/Web/Reports/Reports.Site"
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/Reports/Reports.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Reports/Reports.Site/sharedSettings.json; fi

WORKDIR /src/AlphaPlus/Web/Reports/Reports.Site

RUN yarn && gulp default
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# don’t run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.Reports.Site.dll"]
