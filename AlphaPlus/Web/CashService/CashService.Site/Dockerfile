FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

# copy .csproj files
COPY "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj" "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj"

# restore NuGet packages
COPY "AlphaPlus/NuGet.config" "AlphaPlus/NuGet.config"
RUN dotnet restore "AlphaPlus/Web/CashService/CashService.Site/CashService.Site.csproj" --packages /root/.nuget/packages -v d

# copy all source code
COPY "AlphaPlus/Web/CashService/CashService.Site" "AlphaPlus/Web/CashService/CashService.Site"

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG


WORKDIR /src/AlphaPlus/Web/CashService/CashService.Site

# RUN yarn && gulp default
RUN dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

# don’t run as root
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

ENTRYPOINT ["dotnet", "AlphaPlus.CashService.Site.dll"]
