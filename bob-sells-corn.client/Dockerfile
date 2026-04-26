# Build stage
FROM node:20-alpine AS builder
WORKDIR /app

ENV VITE_API_URL=https://corn-api.onrender.com

COPY package.json ./
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build