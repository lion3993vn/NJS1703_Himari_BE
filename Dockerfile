# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 12345
EXPOSE 12346


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HimariServer.API/HimariServer.API.csproj", "HimariServer.API/"]
COPY ["HimariServer.Services/HimariServer.Services.csproj", "HimariServer.Services/"]
COPY ["HimariServer.Repositories/HimariServer.Repositories.csproj", "HimariServer.Repositories/"]
RUN dotnet restore "./HimariServer.API/HimariServer.API.csproj"
COPY . .
WORKDIR "/src/HimariServer.API"
RUN dotnet build "./HimariServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HimariServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HimariServer.API.dll"]