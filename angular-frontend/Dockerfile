# Separate the npm install portion so builds take a lot less time if no npm packages were changed/added
FROM node:alpine as install
RUN mkdir -p /app
WORKDIR /app
COPY package.json package.json
RUN npm install

# Build the app using AOT compilation
FROM install as prod-build
WORKDIR /app
COPY . .
RUN npm run build --prod

# Copy the results to nginx
FROM nginx:alpine as prod
WORKDIR /app
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=prod-build /app/dist/inventory-manager-frontend /usr/share/nginx/html
