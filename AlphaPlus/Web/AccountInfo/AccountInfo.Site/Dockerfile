FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

RUN apk add --update nodejs npm

WORKDIR /src

# copy .csproj files
COPY "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" "Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj"

# restore NuGet packages
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/AccountInfo/AccountInfo.Site/AccountInfo.Site.csproj" -v n

# copy all source code
COPY "Modules/AlphaPlus.Logging" "Modules/AlphaPlus.Logging"
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"
COPY "Web/AccountInfo/AccountInfo.Site" "Web/AccountInfo/AccountInfo.Site"
COPY "Shared" "Shared"

WORKDIR /src/Web/AccountInfo/AccountInfo.Site

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

ENTRYPOINT ["dotnet", "AlphaPlus.AccountInfo.Site.dll"]