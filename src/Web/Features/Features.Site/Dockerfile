FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /build

# copy .csproj files
COPY "src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"

COPY "src/Services/Features/Messages/Messages.API.csproj" "src/Services/Features/Messages/Messages.API.csproj"
COPY "src/Services/Features/Handbook/Handbook.API.csproj" "src/Services/Features/Handbook/Handbook.API.csproj"
COPY "src/Web/Features/Features.Site/Features.Site.csproj" "src/Web/Features/Features.Site/Features.Site.csproj"

COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Features/Features.Site/Features.Site.csproj" -v d

# copy src files
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"

COPY "src/Services/Features" "src/Services/Features"
COPY "src/Web/Features" "src/Web/Features"
COPY ./src/Web/Features/Features.Site/assets/styles/feedback/images ./src/Web/Features/Features.Site/wwwroot/assets/bundles/feedback/images
COPY ./src/Web/Features/Features.Site/assets/styles/handbook/images ./src/Web/Features/Features.Site/wwwroot/assets/bundles/handbook/images
COPY ./src/Web/Features/Features.Site/assets/styles/handbook/base/fonts ./src/Web/Features/Features.Site/wwwroot/assets/bundles/handbook/base/fonts

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Features/Features.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Features/Features.Site/sharedSettings.json; fi

WORKDIR /build/src/Web/Features/Features.Site

# Install yarn dependencies and build
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/Features/Features.Site/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile && \
    yarn build

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS final

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

ENTRYPOINT ["dotnet", "AlphaPlus.Features.Site.dll"]
