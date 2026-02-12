ARG DOTNET_VERSION=10.0

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY shared/ ./shared/
COPY client/ ./client/

WORKDIR /src/client/ReLoop.Client
RUN dotnet publish ReLoop.Client.csproj -c Release -o /app/publish

# ---- Serve with nginx ----
FROM nginx:alpine AS runtime
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
