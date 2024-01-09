# Bundle and minify
FROM hub.nexdev.net/base/node:18.16-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app
EXPOSE 8080

# Install dependencies based on the preferred package manager
COPY Web/Assets/package.json ./
COPY Web/Assets/yarn.lock ./
RUN --mount=type=cache,target=/root/.yarn yarn --frozen-lockfile

FROM hub.nexdev.net/base/node:18.16-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY Web/Assets ./
# Register TRANSFORM_CONFIG argument for transforming by environment
ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
		|| [ "$TRANSFORM_CONFIG" = "SIT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "UAT2" ] \
		|| [ "$TRANSFORM_CONFIG" = "PRO2" ] \
		|| [ "$TRANSFORM_CONFIG" = "AWS2" ] \
		|| [ "$TRANSFORM_CONFIG" = "NAPAWS2" ] \
		|| [ "$TRANSFORM_CONFIG" = "NAPINTERNAL2" ] \
		|| [ "$TRANSFORM_CONFIG" = "MNL2" ] \
		|| [ "$TRANSFORM_CONFIG" = "MNLNAP2" ]; \
	then COPY Web/Assets/bundleconfig2.json ./bundleconfig; \
	fi

RUN yarn gulp

# Use nginx-unprivileged image
FROM hub.nexdev.net/base/nginxinc/nginx-unprivileged:1.22-alpine-slim
# Set root user has full permissions of read/write files/folders to copy needed things
USER root

# Copy needed files/folders
# RUN cp -r styles/ /usr/share/nginx/html/styles

# RUN cp -r styles-v2/ /usr/share/nginx/html/styles-v2

COPY --from=builder /app/nginx.conf /etc/nginx/conf.d/default.conf
WORKDIR /usr/share/nginx/html
COPY --from=builder /app/bundles ./bundles
COPY --from=builder /app/bundles-v2 ./bundles-v2
COPY --from=builder /app/bundles3 ./bundles3
COPY --from=builder /app/styles/fonts/iconalpha/fonts ./bundles/common/fonts
COPY --from=builder /app/styles/fonts ./bundles/fonts
COPY --from=builder /app/styles-v2/fonts ./bundles-v2/common/fonts
COPY --from=builder /app/styles-v3/base/fonts ./bundles3/fonts
COPY --from=builder /app/styles/images ./images

# RUN if [ "$TRANSFORM_CONFIG" = "LOCAL2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "SIT2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "UAT2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "PRO2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "AWS2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "NAPAWS2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "NAPINTERNAL2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "MNL2" ] \
# 		|| [ "$TRANSFORM_CONFIG" = "MNLNAP2" ]; \
# 	then cp -r assets/bundles2/ /usr/share/nginx/html/bundles; \
# 		 cp -r assets/bundles2-v2/ /usr/share/nginx/html/bundles-v2; \
# 	else cp -r assets/bundles/ /usr/share/nginx/html/bundles; \
# 		 cp -r assets/bundles-v2/ /usr/share/nginx/html/bundles-v2; \
# 	fi

# Remove default page of nginx
RUN rm /usr/share/nginx/html/index.html

# Set non root user nginx
USER nginx