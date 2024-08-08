FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

WORKDIR /src

COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj" "Services/Tickets/BetsForecast/Domain/BetsForecast.Domain.csproj"

COPY "Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj" "Modules/AlphaPlus.IPInfo/AlphaPlus.IPInfo.csproj"
COPY "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj" "Modules/AlphaPlus.SmartPunter/AlphaPlus.SmartPunter.csproj"
COPY "Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Services/Tickets/BetLists/AlphaPlus.BetLists.csproj" "Services/Tickets/BetLists/AlphaPlus.BetLists.csproj"
COPY "Services/Tickets/Results/AlphaPlus.Results.csproj" "Services/Tickets/Results/AlphaPlus.Results.csproj"

COPY "Web/Tickets/BetLists.Builders/BetLists.Builders.csproj" "Web/Tickets/BetLists.Builders/BetLists.Builders.csproj"
COPY "Web/Tickets/Tickets.Site/Tickets.Site.csproj" "Web/Tickets/Tickets.Site/Tickets.Site.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Tickets/Tickets.Site/Tickets.Site.csproj" -v n

COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Services/Tickets/BetsForecast/Domain" "Services/Tickets/BetsForecast/Domain"

COPY "Modules/AlphaPlus.IPInfo" "Modules/AlphaPlus.IPInfo"
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.SmartPunter" "Modules/AlphaPlus.SmartPunter"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"
COPY "Services/Tickets/BetLists" "Services/Tickets/BetLists"
COPY "Services/Tickets/Results" "Services/Tickets/Results"

COPY "Web/Tickets/BetLists.Builders" "Web/Tickets/BetLists.Builders"
COPY "Web/Tickets/Tickets.Site" "Web/Tickets/Tickets.Site/"
COPY "Shared" "Shared"

WORKDIR /src/Web/Tickets/Tickets.Site

RUN yarn
RUN gulp default
RUN dotnet publish --use-current-runtime --self-contained -o /app

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
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Tickets.Site.dll"]
