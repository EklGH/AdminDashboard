# ======== BUILD

FROM node:20-alpine AS build
WORKDIR /app

# Variables d'environnement :
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

# Copier uniquement les fichiers nécessaires pour installer les dépendances :
COPY frontend/package.json frontend/package-lock.json* frontend/tsconfig*.json frontend/vite.config.ts ./

# Installer les dépendances :
RUN npm ci

# Copie le code frontend :
COPY frontend/ .

# Build Vite
RUN npm run build



# ======= Serve avec NGINX

FROM nginx:stable-alpine

# Supprimer la config NGINX par défaut :
RUN rm -rf /usr/share/nginx/html/*

# Copier la config NGINX et les fichiers build :
COPY nginx/default.conf /etc/nginx/conf.d/
COPY --from=build /app/dist /usr/share/nginx/html

# Port de l'application :
EXPOSE 80

# Lancer NGINX en foreground
CMD ["nginx", "-g", "daemon off;"]