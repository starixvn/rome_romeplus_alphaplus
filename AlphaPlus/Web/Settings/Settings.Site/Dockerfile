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
COPY "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "AlphaPlus/Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "AlphaPlus/Modules/AlphaPlus.Products/AlphaPlus.Products.csproj" "AlphaPlus/Modules/AlphaPlus.Products/AlphaPlus.Products.csproj"

COPY "AlphaPlus/Services/Settings/Transfers/Transfer.API/Transfer.API.csproj" "AlphaPlus/Services/Settings/Transfers/Transfer.API/Transfer.API.csproj"
COPY "AlphaPlus/Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj" "AlphaPlus/Services/Settings/Viewlogs/ViewLog.API/ViewLog.API.csproj"
COPY "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj" "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.API/MemberInfo.API.csproj"
COPY "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj" "AlphaPlus/Services/Settings/MemberInfo/MemberInfo.Domain/MemberInfo.Domain.csproj"

COPY "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj" "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj"

COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/Settings/Settings.Site/Settings.Site.csproj" -v d

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

COPY "AlphaPlus/Web/Settings" "AlphaPlus/Web/Settings"

ARG TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.json AlphaPlus/Web/Settings/Settings.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r AlphaPlus/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json AlphaPlus/Web/Settings/Settings.Site/sharedSettings.json; fi

WORKDIR /src/AlphaPlus/Web/Settings/Settings.Site

RUN yarn && yarn build
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS final

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# cache nuget
WORKDIR /root
COPY --from=builder /root/.nuget ./.nuget

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "AlphaPlus.Settings.Site.dll"]
