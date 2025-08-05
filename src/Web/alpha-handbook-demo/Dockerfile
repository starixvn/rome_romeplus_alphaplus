FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-node22-25h1 AS builder

WORKDIR /build

# copy .csproj files
COPY "src/Web/alpha-handbook-demo/Alpha.Handbook.Demo.csproj" "src/Web/alpha-handbook-demo/Alpha.Handbook.Demo.csproj"

COPY "src/NuGet.config" "src/NuGet.config"
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Web/alpha-handbook-demo/Alpha.Handbook.Demo.csproj" -v d

# copy src files
COPY "src/Shared" "src/Shared"
COPY "src/Web/alpha-handbook-demo" "src/Web/alpha-handbook-demo"
COPY ./src/Web/alpha-handbook-demo/assets/styles/feedback/images ./src/Web/alpha-handbook-demo/wwwroot/assets/bundles/feedback/images
COPY ./src/Web/alpha-handbook-demo/assets/styles/handbook/images ./src/Web/alpha-handbook-demo/wwwroot/assets/bundles/handbook/images
COPY ./src/Web/alpha-handbook-demo/assets/styles/handbook/base/fonts ./src/Web/alpha-handbook-demo/wwwroot/assets/bundles/handbook/base/fonts

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

WORKDIR /build/src/Web/alpha-handbook-demo

# Install yarn dependencies and build
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/alpha-handbook-demo/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/build/src/Web/alpha-handbook-demo/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn build

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h1 AS final

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser

# final stage/image
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Alpha.Handbook.Demo.dll"]
