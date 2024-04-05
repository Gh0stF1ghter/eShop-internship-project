#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Baskets.API/Baskets.API.csproj", "Baskets.API/"]
COPY ["Baskets.BusinessLogic/Baskets.BusinessLogic.csproj", "Baskets.BusinessLogic/"]
COPY ["Baskets.DataAccess/Baskets.DataAccess.csproj", "Baskets.DataAccess/"]
RUN dotnet restore "./Baskets.API/Baskets.API.csproj"
COPY . .
WORKDIR "/src/Baskets.API"
RUN dotnet build "./Baskets.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Baskets.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baskets.API.dll"]