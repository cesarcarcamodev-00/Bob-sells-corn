# Build stage (keep as is)
FROM node:20-alpine AS builder
WORKDIR /app
ENV VITE_API_URL=https://corn-api.onrender.com
COPY package.json ./
RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build

# Nginx stage (this is what needs fixing)
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

# Add these fixes:
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost/ || exit 1

CMD ["nginx", "-g", "daemon off;"]