# Separate the npm install portion so builds take a lot less time if no npm packages were changed/added
FROM node:alpine as install
RUN mkdir -p /app
WORKDIR /app
COPY package.json package.json
RUN npm install

# Build the app using AOT compilation
FROM install as build
WORKDIR /app
COPY . .
RUN npm run build -- --configuration=staging

# Copy the results to nginx. Also copy a custom nginx config in order to let angular handle routing
FROM nginx:alpine as staging
WORKDIR /app
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/inventory-manager-frontend /usr/share/nginx/html
