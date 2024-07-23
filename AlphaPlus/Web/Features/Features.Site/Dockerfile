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

COPY "Services/Features/Messages/Messages.API.csproj" "Services/Features/Messages/Messages.API.csproj"
COPY "Web/Features/Features.Site/Features.Site.csproj" "Web/Features/Features.Site/Features.Site.csproj"

COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Features/Features.Site/Features.Site.csproj" -v n

# copy src files
COPY "Shared" "Shared"
COPY "Modules/AlphaPlus.Logging" "Modules/AlphaPlus.Logging"
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"

COPY "Services/Features" "Services/Features"
COPY "Web/Features" "Web/Features"
COPY ./Web/Features/Features.Site/assets/styles/feedback/images ./Web/Features/Features.Site/wwwroot/assets/bundles/feedback/images

WORKDIR /src/Web/Features/Features.Site

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
