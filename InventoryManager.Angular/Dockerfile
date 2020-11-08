FROM node:alpine as build
RUN mkdir -p /app
WORKDIR /app
COPY inventory-manager-frontend/package.json package.json
COPY inventory-manager-frontend/package-lock.json package-lock.json
RUN npm install
COPY inventory-manager-frontend ./
RUN npm run build --prod

FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/inventory-manager-frontend /usr/share/nginx/html
