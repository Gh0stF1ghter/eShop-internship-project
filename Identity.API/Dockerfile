FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Identity/
COPY ["Identity.API/Identity.API.csproj", "Identity.API/"]
COPY ["Identity.BusinessLogic/Identity.BusinessLogic.csproj", "Identity.BusinessLogic/"]
COPY ["Identity.DataAccess/Identity.DataAccess.csproj", "Identity.DataAccess/"]
RUN dotnet restore "./Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Identity.API"
RUN dotnet build "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]