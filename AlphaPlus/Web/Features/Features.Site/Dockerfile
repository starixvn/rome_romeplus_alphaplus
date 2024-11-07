FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj" "AlphaPlus/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "AlphaPlus/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "AlphaPlus/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "AlphaPlus/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "AlphaPlus/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"

COPY "AlphaPlus/Services/Features/Messages/Messages.API.csproj" "AlphaPlus/Services/Features/Messages/Messages.API.csproj"
COPY "AlphaPlus/Web/Features/Features.Site/Features.Site.csproj" "AlphaPlus/Web/Features/Features.Site/Features.Site.csproj"

COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Features/Features.Site/Features.Site.csproj" -v n

# copy src files
COPY "AlphaPlus/Shared" "AlphaPlus/Shared"
COPY "AlphaPlus/Modules/AlphaPlus.Logging" "AlphaPlus/Modules/AlphaPlus.Logging"
COPY "AlphaPlus/Modules/AlphaPlus.Utilities.IP" "AlphaPlus/Modules/AlphaPlus.Utilities.IP"
COPY "AlphaPlus/Modules/AlphaPlus.PartialUM" "AlphaPlus/Modules/AlphaPlus.PartialUM"
COPY "AlphaPlus/Modules/AlphaPlus.RequestInfo" "AlphaPlus/Modules/AlphaPlus.RequestInfo"
COPY "AlphaPlus/Modules/AlphaPlus.Core" "AlphaPlus/Modules/AlphaPlus.Core"

COPY "AlphaPlus/Services/Features" "AlphaPlus/Services/Features"
COPY "AlphaPlus/Web/Features" "AlphaPlus/Web/Features"
COPY ./AlphaPlus/Web/Features/Features.Site/assets/styles/feedback/images ./AlphaPlus/Web/Features/Features.Site/wwwroot/assets/bundles/feedback/images

ARG TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Features/Features.Site/sharedSettings.json; fi

WORKDIR /src/AlphaPlus/Web/Features/Features.Site

RUN yarn && yarn build
RUN dotnet publish --use-current-runtime --self-contained -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS final

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

ENTRYPOINT ["dotnet", "AlphaPlus.Features.Site.dll"]
