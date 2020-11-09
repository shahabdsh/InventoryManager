FROM node:alpine as install
RUN mkdir -p /app
WORKDIR /app
COPY inventory-manager-frontend/package.json package.json
RUN npm install

FROM install as dev
WORKDIR /app
RUN npm install -g @angular/cli
COPY inventory-manager-frontend ./
ENTRYPOINT ["npm", "run", "start"]

FROM install as prod-build
WORKDIR /app
COPY inventory-manager-frontend .
RUN npm run build --prod

FROM nginx:alpine as prod
WORKDIR /app
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=prod-build /app/dist/inventory-manager-frontend /usr/share/nginx/html
