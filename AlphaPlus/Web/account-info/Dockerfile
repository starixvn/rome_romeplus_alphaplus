FROM hub.nexdev.net/base/node:20.9.0-alpine AS builder

WORKDIR /app
COPY Web/account-info/package.json ./
COPY Web/account-info/yarn.lock ./
RUN --mount=type=cache,target=/root/.yarn yarn --frozen-lockfile
COPY Web/account-info ./
RUN yarn build

FROM hub.nexdev.net/base/nginxinc/nginx-unprivileged:1.22-alpine-slim
COPY --from=builder /app/dist /usr/share/nginx/html
COPY --from=builder /app/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 8080
USER nginx