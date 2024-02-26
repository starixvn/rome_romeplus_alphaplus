FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

RUN apk add --update nodejs npm

COPY . ./src

WORKDIR /src/Web/Account/Account.Site
RUN dotnet publish -o /app

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

ENTRYPOINT ["dotnet", "AlphaPlus.Account.Site.dll"]
