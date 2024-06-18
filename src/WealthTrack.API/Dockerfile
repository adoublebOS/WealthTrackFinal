# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore projects
COPY ["src/WealthTrack.Domain/WealthTrack.Domain.csproj", "WealthTrack.Domain/"]
COPY ["src/WealthTrack.Infrastructure/WealthTrack.Infrastructure.csproj", "WealthTrack.Infrastructure/"]
COPY ["src/WealthTrack.Application/WealthTrack.Application.csproj", "WealthTrack.Application/"]
COPY ["src/WealthTrack.API/WealthTrack.API.csproj", "WealthTrack.API/"]
RUN dotnet restore "./WealthTrack.API/./WealthTrack.API.csproj"

WORKDIR "/src/WealthTrack.API"
# Copy the rest of the code
COPY . .

# Build the application
RUN dotnet build "./WealthTrack.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WealthTrack.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WealthTrack.API.dll"]
