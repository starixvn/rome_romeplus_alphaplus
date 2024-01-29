FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

WORKDIR /src

COPY "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj" "Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj"
COPY "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj" "Modules/AlphaPlus.Core/AlphaPlus.Core.csproj"
COPY "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj" "Modules/AlphaPlus.SecurityCode/AlphaPlus.SecurityCode.csproj"
COPY "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj" "Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj"
COPY "Web/Main/Alpha.Main/Alpha.Main.csproj" "Web/Main/Alpha.Main/Alpha.Main.csproj"
COPY "Web/Main/Alpha.Main.Site/Alpha.Main.Site.csproj" "Web/Main/Alpha.Main.Site/Alpha.Main.Site.csproj"
COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "Web/Main/Alpha.Main.Site/Alpha.Main.Site.csproj" -v n

COPY "Modules/AlphaPlus.RequestInfo" "Modules/AlphaPlus.RequestInfo"
COPY "Modules/AlphaPlus.Core" "Modules/AlphaPlus.Core"
COPY "Modules/AlphaPlus.SecurityCode" "Modules/AlphaPlus.SecurityCode"
COPY "Modules/AlphaPlus.Logging" "Modules/AlphaPlus.Logging"
COPY "Web/Main/Alpha.Main" "Web/Main/Alpha.Main"
COPY "Web/Main/Alpha.Main.Site" "Web/Main/Alpha.Main.Site"
COPY "Shared" "Shared"

WORKDIR /src/Web/Main/Alpha.Main.Site
RUN yarn && yarn build
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

ENTRYPOINT ["dotnet", "Alpha.Main.Site.dll"]
