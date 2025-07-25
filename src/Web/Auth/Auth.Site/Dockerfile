FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base
WORKDIR /app
EXPOSE 5000

FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS build
WORKDIR /build


# Copy project files
COPY ["src/Web/Auth/Auth.Site/Auth.Site.csproj", "src/Web/Auth/Auth.Site/"]
COPY ["src/Modules/AlphaPlus.Core/AlphaPlus.Core.csproj", "src/Modules/AlphaPlus.Core/"]
COPY ["src/Modules/AlphaPlus.Core.Authorization/AlphaPlus.Core.Authorization.csproj", "src/Modules/AlphaPlus.Core.Authorization/"]
COPY ["src/Modules/AlphaPlus.Logging/AlphaPlus.Logging.csproj", "src/Modules/AlphaPlus.Logging/"]
COPY ["src/Modules/AlphaPlus.PartialUM/AlphaPlus.PartialUM.csproj", "src/Modules/AlphaPlus.PartialUM/"]
COPY ["src/Shared/AlphaPlus.Shared.AppResources/AlphaPlus.Shared.AppResources.csproj", "src/Shared/AlphaPlus.Shared.AppResources/"]
COPY ["src/Modules/AlphaPlus.RequestInfo/AlphaPlus.RequestInfo.csproj", "src/Modules/AlphaPlus.RequestInfo/"]
COPY ["src/Modules/AlphaPlus.Utilities.IP/AlphaPlus.Utilities.IP.csproj", "src/Modules/AlphaPlus.Utilities.IP/"]

# Restore dependencies
COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/Auth/Auth.Site/Auth.Site.csproj" -v d

# copy all source code
COPY "src/Shared" "src/Shared"
COPY "src/Modules/AlphaPlus.Core" "src/Modules/AlphaPlus.Core"
COPY "src/Modules/AlphaPlus.Core.Authorization" "src/Modules/AlphaPlus.Core.Authorization"
COPY "src/Modules/AlphaPlus.Logging" "src/Modules/AlphaPlus.Logging"
COPY "src/Modules/AlphaPlus.PartialUM" "src/Modules/AlphaPlus.PartialUM"
COPY "src/Shared/AlphaPlus.Shared.AppResources" "src/Shared/AlphaPlus.Shared.AppResources"
COPY "src/Modules/AlphaPlus.RequestInfo" "src/Modules/AlphaPlus.RequestInfo"
COPY "src/Modules/AlphaPlus.Utilities.IP" "src/Modules/AlphaPlus.Utilities.IP"
COPY "src/Web/Auth/Auth.Site" "src/Web/Auth/Auth.Site"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "${TRANSFORM_CONFIG}" = "" ]; then cp -r src/Shared/Configuration/sharedSettings.json src/Web/Auth/Auth.Site/sharedSettings.json; fi

RUN if [ "${TRANSFORM_CONFIG}" != "" ]; then cp -r src/Shared/Configuration/sharedSettings.${TRANSFORM_CONFIG}.json src/Web/Auth/Auth.Site/sharedSettings.json; fi

# Build
WORKDIR "/build/src/Web/Auth/Auth.Site"
RUN dotnet build "Auth.Site.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Auth.Site.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Site.dll"] 