# Stage 1: Build
FROM node:20 AS build
WORKDIR /app
COPY . .
RUN npm install && npm run build

# Stage 2: Serve
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/build .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
