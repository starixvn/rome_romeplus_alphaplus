# Creating multi-stage build for production
FROM hub.nexdev.net/base/node:18-alpine-25h1 AS builder
RUN apk update && apk add --no-cache build-base gcc autoconf automake zlib-dev libpng-dev vips-dev git > /dev/null 2>&1
ENV NODE_ENV=production

WORKDIR /opt/
COPY src/Web/alpha-handbook-admin/package.json ./
COPY src/Web/alpha-handbook-admin/yarn.lock ./
RUN yarn global add node-gyp
RUN --mount=type=cache,target=/opt/app/node_modules \
    yarn config set network-timeout 600000 -g && yarn install --production
ENV PATH /opt/node_modules/.bin:$PATH
WORKDIR /opt/app
COPY src/Web/alpha-handbook-admin .

# Register TRANSFORM_CONFIG argument for transforming by environment
ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG
COPY "src/Web/alpha-handbook-admin/.env.$TRANSFORM_CONFIG" .env

RUN  yarn build

# Creating final production image
FROM hub.nexdev.net/base/node:18-alpine-25h1

RUN apk add --no-cache vips-dev
ENV NODE_ENV=production

WORKDIR /opt/
COPY --from=builder /opt/node_modules ./node_modules
WORKDIR /opt/app
COPY --from=builder /opt/app ./
ENV PATH /opt/node_modules/.bin:$PATH

RUN chown -R node:node /opt/app
USER node
EXPOSE 1337
CMD ["yarn", "start"]
