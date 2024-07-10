# Stage 1: Base
FROM node:20-alpine AS base
WORKDIR /app
EXPOSE 4200

# Stage 2: Build
FROM node:20-alpine AS build
WORKDIR /src
COPY package*.json./
RUN npm install
COPY..
RUN ng build --prod

# Stage 3: Publish
FROM build AS publish
RUN ng build --prod --output-path /app/publish

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish.
CMD ["npm", "start"]