FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
# RUN apk add --update nodejs npm
# RUN npm install -g gulp

WORKDIR /src

COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Web/Settings/Settings.Site/Settings.Site.csproj" "Web/Settings/Settings.Site/Settings.Site.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Settings/Settings.Site/Settings.Site.csproj" -v n

COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Web/Settings/Settings.Site" "Web/Settings/Settings.Site"
COPY "Shared" "Shared"

WORKDIR /src/Web/Settings/Settings.Site

# RUN npm install
# RUN gulp default
RUN dotnet publish --no-restore -o /app

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

ENTRYPOINT ["dotnet", "AlphaPlus.Settings.Site.dll"]
