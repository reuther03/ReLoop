ARG DOTNET_VERSION=10.0

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY shared/ ./shared/
COPY server/ ./server/

WORKDIR /src/server/ReLoop.Api
RUN dotnet publish ReLoop.Api.csproj -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
COPY seed/ ./seed/
ENTRYPOINT ["dotnet", "ReLoop.Api.dll"]
