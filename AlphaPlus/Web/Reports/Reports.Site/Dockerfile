FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

# installing yarn & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

WORKDIR /src

# copy .csproj files
COPY "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj" "Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj"
COPY "Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj" "Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj"
COPY "Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj" "Services/Reports/AlphaPlus.Reports/AlphaPlus.Reports.csproj"

COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Web/Reports/Reports.Site/Reports.Site.csproj" "Web/Reports/Reports.Site/Reports.Site.csproj"

# restore NuGet packages
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Reports/Reports.Site/Reports.Site.csproj" -v n

# copy all source code
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Services/Reports/AlphaPlus.Reports" "Services/Reports/AlphaPlus.Reports"

COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"

COPY "Web/Reports/Reports.Site" "Web/Reports/Reports.Site"
COPY "Shared" "Shared"

WORKDIR /src/Web/Reports/Reports.Site

RUN yarn
RUN gulp default
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
  dotnet publish --use-current-runtime --self-contained -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

# don’t run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.Reports.Site.dll"]
