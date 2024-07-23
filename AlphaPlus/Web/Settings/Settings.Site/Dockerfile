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
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Modules/AlphaPlus.Products/AlphaPlus.Products.csproj" "Modules/AlphaPlus.Products/AlphaPlus.Products.csproj"

COPY "Services/Settings/Transfers/Transfer.API/Transfer.API.csproj" "Services/Settings/Transfers/Transfer.API/Transfer.API.csproj"
COPY "Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj" "Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj"
COPY "Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj" "Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj"
COPY "Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj" "Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj"

COPY "Web/Settings/Settings.Site/Settings.Site.csproj" "Web/Settings/Settings.Site/Settings.Site.csproj"

COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Settings/Settings.Site/Settings.Site.csproj" -v n

# copy src files
COPY "Shared" "Shared"
COPY "Modules/AlphaPlus.Products" "Modules/AlphaPlus.Products"
COPY "Modules/AlphaPlus.Logging" "Modules/AlphaPlus.Logging"
COPY "Modules/AlphaPlus.Utilities.IP" "Modules/AlphaPlus.Utilities.IP"
COPY "Modules/AlphaPlus.PartialUM" "Modules/AlphaPlus.PartialUM"
COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"
COPY "Services/Settings" "Services/Settings"

COPY "Web/Settings" "Web/Settings"

WORKDIR /src/Web/Settings/Settings.Site

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
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Settings.Site.dll"]
