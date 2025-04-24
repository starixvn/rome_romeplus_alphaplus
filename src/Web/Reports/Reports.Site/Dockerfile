FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /build

# copy .csproj files
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "src/Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj" "src/Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj"

COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "src/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj" "src/Modules/AlphaPlus.NetThrottling/AlphaPlus.NetThrottling.csproj"
COPY "src/Web/Reports/Reports.Site/Reports.Site.csproj" "src/Web/Reports/Reports.Site/Reports.Site.csproj"

# restore NuGet packages
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Reports/Reports.Site/Reports.Site.csproj" -v d

# copy all source code
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Services/Reports/AlphaPlus.Reports" "src/Services/Reports/AlphaPlus.Reports"

COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.SecurityCode" "src/Modules/AlphaPlus.SecurityCode"
COPY "src/Modules/AlphaPlus.NetThrottling" "src/Modules/AlphaPlus.NetThrottling"
COPY "src/Web/Reports/Reports.Site" "src/Web/Reports/Reports.Site"
COPY "src/Shared" "src/Shared"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Reports/Reports.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Reports/Reports.Site/sharedSettings.json; fi

WORKDIR /build/src/Web/Reports/Reports.Site

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Reports/Reports.Site/node_modules \  
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Reports/Reports.Site/node_modules \  
    yarn config set cache-folder /root/.cache/yarn && \
    yarn gulp

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS base

# don’t run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.Reports.Site.dll"]
