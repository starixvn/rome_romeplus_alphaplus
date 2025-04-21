FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Products/AlphaPlus.Products.csproj" "AlphaPlus/Modules/AlphaPlus.Products/AlphaPlus.Products.csproj"

COPY "AlphaPlus/Services/Settings/Transfers/Transfer.API/Transfer.API.csproj" "AlphaPlus/Services/Settings/Transfers/Transfer.API/Transfer.API.csproj"
COPY "AlphaPlus/Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj" "AlphaPlus/Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj"
COPY "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj" "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj"
COPY "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj" "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj"
COPY "AlphaPlus/Services/Features/Messages/Messages.API.csproj" "AlphaPlus/Services/Features/Messages/Messages.API.csproj"

COPY "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj" "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj"

COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"

# Restore NuGet packages with cache
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj" -v d

# copy src files
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"
COPY "AlphaPlus/Modules/AlphaPlus.Products" "AlphaPlus/Modules/AlphaPlus.Products"
COPY "AlphaPlus/Modules/AlphaPlus.Logging" "AlphaPlus/Modules/AlphaPlus.Logging"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode" "AlphaPlus/Modules/AlphaPlus.SecurityCode"
COPY "AlphaPlus/Services/Settings" "AlphaPlus/Services/Settings"
COPY "AlphaPlus/Services/Features/Messages" "AlphaPlus/Services/Features/Messages"

COPY "AlphaPlus/Web/Settings" "AlphaPlus/Web/Settings"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/Settings/Settings.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Settings/Settings.Site/sharedSettings.json; fi

ARG CI_JOB_TOKEN
RUN echo $TRANSFORM_CONFIG
RUN mkdir /src/tmp
RUN if [ "${TRANSFORM_CONFIG}" = "PRO" ] \
  || [ "${TRANSFORM_CONFIG}" = "PRO2" ] \
  || [ "${TRANSFORM_CONFIG}" = "PRO3" ] \
  || [ "${TRANSFORM_CONFIG}" = "PRO4" ] \
  || [ "${TRANSFORM_CONFIG}" = "PRO5" ]; \
  then cd /src/tmp && curl --fail -o ${TRANSFORM_CONFIG}.domainmappings.json --header "PRIVATE-TOKEN:${CI_JOB_TOKEN}" https://gitlab.nexdev.net/api/v4/projects/alpha-config%2Flogin-domains/repository/files/src%2F${TRANSFORM_CONFIG}.domainmappings.json/raw?ref=main \
  && cd - && cp -r tmp/${TRANSFORM_CONFIG}.domainmappings.json AlphaPlus/Web/Settings/Settings.Site/Domains/domainmappings.json; fi

WORKDIR /src/AlphaPlus/Web/Settings/Settings.Site

# Copy package files for better caching
COPY "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/package.json" "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/package.json"
COPY "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/package-lock.json" "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/package-lock.json"
COPY "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/yarn.lock" "AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/yarn.lock"

# Install ClientApp dependencies with yarn cache and build
WORKDIR /src/AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/src/AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    yarn build && cp -r dist/* /src/AlphaPlus/Web/Settings/Settings.Site/wwwroot

WORKDIR /src/AlphaPlus/Web/Settings/Settings.Site

# Install main dependencies with yarn cache and build
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/src/AlphaPlus/Web/Settings/Settings.Site/node_modules \
    --mount=type=cache,target=/src/AlphaPlus/Web/Settings/Settings.Site/Features/CreateAccount/ClientApp/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    yarn build

RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS final

# Don't run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.Settings.Site.dll"]
