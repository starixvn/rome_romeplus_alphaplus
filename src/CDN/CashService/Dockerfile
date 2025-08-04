FROM hub.nexdev.net/base/nginx:1.24-alpine AS builder

RUN addgroup -g 1001 -S nginx && \
    adduser -S -D -H -u 1001 -h /var/cache/nginx -s /sbin/nologin -G nginx -g nginx nginx

RUN mkdir -p /usr/share/nginx/images && \
    chown -R nginx:nginx /usr/share/nginx/images && \
    chown -R nginx:nginx /var/cache/nginx && \
    chown -R nginx:nginx /var/log/nginx && \
    chown -R nginx:nginx /etc/nginx/conf.d

FROM hub.nexdev.net/base/nginx:1.24-alpine

COPY --from=builder /etc/passwd /etc/passwd
COPY --from=builder /etc/group /etc/group

RUN mkdir -p /usr/share/nginx/images && \
    chown -R nginx:nginx /usr/share/nginx/images && \
    chown -R nginx:nginx /var/cache/nginx && \
    chown -R nginx:nginx /var/log/nginx && \
    chown -R nginx:nginx /etc/nginx/conf.d

COPY --chown=nginx:nginx default.conf /etc/nginx/conf.d/default.conf
COPY --chown=nginx:nginx nginx.conf /etc/nginx/nginx.conf

USER nginx




