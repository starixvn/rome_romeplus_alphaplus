FROM hub.nexdev.net/base/node:18.16-alpine AS builder

WORKDIR /app
COPY Web/account-info/package.json ./
COPY Web/account-info/yarn.lock ./
RUN --mount=type=cache,target=/root/.yarn yarn --frozen-lockfile