# Bundle and minify
FROM hub.nexdev.net/base/node:18.16-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app
EXPOSE 8080

# Install dependencies based on the preferred package manager
COPY src/Web/Assets/package.json src/Web/Assets/yarn.lock ./
RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/app/src/Web/Assets/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn install --frozen-lockfile --prefer-offline

FROM hub.nexdev.net/base/node:18.16-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY src/Web/Assets ./
# Register TRANSFORM_CONFIG argument for transforming by environment
ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
		|| [ "$TRANSFORM_CONFIG" = "SIT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "UAT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "STG2" ] \
		|| [ "$TRANSFORM_CONFIG" = "STGINT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "INT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "PRO2" ] \
		|| [ "$TRANSFORM_CONFIG" = "DR2" ] \
		|| [ "$TRANSFORM_CONFIG" = "ALPHAKLPROD2" ] \
		|| [ "$TRANSFORM_CONFIG" = "ALPHAKLPRODINT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLE2" ] \
		|| [ "$TRANSFORM_CONFIG" = "ALPHAKLSIMPLEINT2" ]; \
	then cp ./bundleconfig2.json ./bundleconfig.json; \
	fi

RUN --mount=type=cache,target=/root/.yarn \
    --mount=type=cache,target=/root/.cache/yarn \
    --mount=type=cache,target=/app/src/Web/Assets/node_modules \
    yarn config set cache-folder /root/.cache/yarn && \
    yarn gulp

# Use nginx-unprivileged image
FROM hub.nexdev.net/base/nginxinc/nginx-unprivileged:1.22-alpine-slim
# Set root user has full permissions of read/write files/folders to copy needed things
USER root

COPY --from=builder /app/nginx.conf /etc/nginx/conf.d/default.conf
WORKDIR /usr/share/nginx/html
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/common/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/common/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-betlists/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-reports/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-memberinfo/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-totalbetsforecast/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-transfer/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-viewlogs/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-messages/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-results/bundles/fonts
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/site-main/dashboard/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/common/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-betlists/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-reports/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-memberinfo/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-totalbetsforecast/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-transfer/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-viewlogs/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-messages/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-results/bundles/fonts
COPY --from=builder /app/styles/fonts ./bundles/site-main/dashboard/bundles/fonts
COPY --from=builder /app/styles-v2/fonts ./bundles-v2/common/fonts
COPY --from=builder /app/styles-v2/fonts ./bundles-v2/common/bundles/fonts
COPY --from=builder /app/styles-v2/fonts ./bundles-v2/fonts
COPY --from=builder /app/styles-v2/fonts/iconalpha/fonts ./bundles-v2/fonts
COPY --from=builder /app/styles-v2/fonts/iconalpha/fonts ./bundles-v2/common/fonts
COPY --from=builder /app/styles-v2/fonts/iconalpha/fonts ./bundles-v2/common/bundles/fonts
COPY --from=builder /app/styles-v3/base/fonts ./bundles3/fonts
COPY --from=builder /app/styles-v3/base/fonts ./bundles3/common/bundles/fonts
COPY --from=builder /app/styles/images ./bundles/images
COPY --from=builder /app/styles/vendors ./bundles/vendors
COPY --from=builder /app/styles-v3/images ./bundles3/images
COPY --from=builder /app/styles-v2/images ./bundles-v2/images
COPY --from=builder /app/styles/vendors/jqueryui/images ./bundles/site-viewlogs/images
COPY --from=builder /app ./

# Remove default page of nginx
RUN rm /usr/share/nginx/html/index.html

# Set non root user nginx
USER nginx