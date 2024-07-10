# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:latest as build

# Set the working directory
WORKDIR /src

# Add the source code to app
COPY ["src/Clients/MyELib.Clients.Web/", "src/Clients/MyELib.Clients.Web/"]

# Install all the dependencies
RUN npm ci --prefix "src/Clients/MyELib.Clients.Web/"

# Generate the build of the application
RUN npm run build --prefix "src/Clients/MyELib.Clients.Web/"

# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:stable

COPY src/Clients/MyELib.Clients.Web/nginx.conf /etc/nginx/nginx.conf
RUN rm -rf /usr/share/nginx/html
# Copy the build output to replace the default nginx contents.
COPY --from=build /src/src/Clients/MyELib.Clients.Web/dist/my-elib.clients.web/browser /usr/share/nginx/html

# Expose port 80
EXPOSE 80
