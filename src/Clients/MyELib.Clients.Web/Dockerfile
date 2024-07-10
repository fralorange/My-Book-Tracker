FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json./
RUN npm install -g @angular/cli@17
RUN npm ci
COPY..
RUN ng build

FROM nginx:alpine

COPY --from=build /app/dist/angular-docker/browser /usr/share/nginx/html

EXPOSE 8080

# Run nginx
CMD ["nginx", "-g", "daemon off;"]
