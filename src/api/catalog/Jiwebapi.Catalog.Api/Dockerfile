#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jiwebapi.Catalog.Api/Jiwebapi.Catalog.Api.csproj", "Jiwebapi.Catalog.Api/"]
COPY ["Jiwebapi.Catalog.Application/Jiwebapi.Catalog.Application.csproj", "Jiwebapi.Catalog.Application/"]
COPY ["Jiwebapi.Catalog.Domain/Jiwebapi.Catalog.Domain.csproj", "Jiwebapi.Catalog.Domain/"]
COPY ["Jiwebapi.Catalog.Cache/Jiwebapi.Catalog.Cache.csproj", "Jiwebapi.Catalog.Cache/"]
COPY ["Jiwebapi.Catalog.History/Jiwebapi.Catalog.History.csproj", "Jiwebapi.Catalog.History/"]
COPY ["Jiwebapi.Catalog.Identity/Jiwebapi.Catalog.Identity.csproj", "Jiwebapi.Catalog.Identity/"]
COPY ["Jiwebapi.Catalog.Message/Jiwebapi.Catalog.Message.csproj", "Jiwebapi.Catalog.Message/"]
COPY ["Jiwebapi.Catalog.Persistence/Jiwebapi.Catalog.Persistence.csproj", "Jiwebapi.Catalog.Persistence/"]
RUN dotnet restore "./Jiwebapi.Catalog.Api/./Jiwebapi.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Jiwebapi.Catalog.Api"
RUN dotnet build "./Jiwebapi.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Jiwebapi.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jiwebapi.Catalog.Api.dll"]