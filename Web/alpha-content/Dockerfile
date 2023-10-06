FROM hub.nexdev.net/base/node:18.16-alpine AS builder
# Installing libvips-dev for sharp Compatibility
RUN apk update && apk add --no-cache build-base gcc autoconf automake zlib-dev libpng-dev nasm bash vips-dev git
ARG NODE_ENV=development
ENV NODE_ENV=${NODE_ENV}

ARG TRANSFORM_CONFIG
RUN echo $TRANSFORM_CONFIG

WORKDIR /opt/
COPY Web/alpha-content/package.json ./
COPY Web/alpha-content/yarn.lock ./
COPY "Web/alpha-content/.env.$TRANSFORM_CONFIG" .env
RUN yarn global add node-gyp
RUN yarn config set network-timeout 600000 -g && yarn install
ENV PATH /opt/node_modules/.bin:$PATH

WORKDIR /opt/app
COPY Web/alpha-content .
RUN chown -R node:node /opt/app
USER node
RUN ["yarn", "build"]
EXPOSE 5000
CMD ["yarn", "develop"]