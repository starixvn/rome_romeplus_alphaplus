FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

#Installing npm & gulp global
RUN apk add --update nodejs npm
RUN npm install -g yarn gulp-cli

COPY . ./src
COPY ./Web/Features/Features.Site/assets/styles/feedback/images ./src/Web/Features/Features.Site/wwwroot/assets/bundles/feedback/images

WORKDIR /src/Web/Features/Features.Site
RUN yarn && yarn build
RUN dotnet restore
RUN dotnet publish --no-restore -o /app

FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS development
COPY . ./src
WORKDIR /src/Web/Features/Features.Site
CMD dotnet run --no-launch-profile

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

ENTRYPOINT ["dotnet", "AlphaPlus.Features.Site.dll"]
