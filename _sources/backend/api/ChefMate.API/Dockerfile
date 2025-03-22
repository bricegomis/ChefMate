# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Build API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/api

# Copy all projects to the container
COPY ["ChefMate.API/ChefMate.API.csproj", "ChefMate.API/"]
COPY ["ChefMate.Models/ChefMate.Models.csproj", "ChefMate.Models/"]

# Restore dependencies
RUN dotnet restore "ChefMate.API/ChefMate.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the project
WORKDIR "/src/api/ChefMate.API"
RUN dotnet build "ChefMate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChefMate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChefMate.API.dll"]
