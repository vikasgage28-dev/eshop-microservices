# ================================
# STAGE 1 — BUILD
# Node builds the React app into static files
# ================================
FROM node:22-alpine AS build
WORKDIR /app

# Copy package files first — layer cache for npm install
COPY package*.json ./
RUN npm ci

# Copy source and build
# Note: TypeScript type checking runs as a separate CI step (not here)
# Docker only bundles — keeps build fast and focused
COPY . .
RUN npx vite build

# ================================
# STAGE 2 — RUNTIME
# Nginx serves the static dist/ folder
# ================================
FROM nginx:alpine AS runtime

LABEL maintainer="Vikas Gage"
LABEL org.opencontainers.image.title="eshop-frontend"
LABEL org.opencontainers.image.description="eShop Microservices — React Frontend"

# Copy built files from Stage 1
COPY --from=build /app/dist /usr/share/nginx/html

# Replace default Nginx config with our SPA config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget -qO- http://localhost/ || exit 1

CMD ["nginx", "-g", "daemon off;"]