#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Hosts/MyELib.Hosts.Migrator/MyELib.Hosts.Migrator.csproj", "src/Hosts/MyELib.Hosts.Migrator/"]
COPY ["src/Infrastructure/MyELib.Infrastructure.DataAccess/MyELib.Infrastructure.DataAccess.csproj", "src/Infrastructure/MyELib.Infrastructure.DataAccess/"]
COPY ["src/Application/MyELib.Application.AppData/MyELib.Application.AppData.csproj", "src/Application/MyELib.Application.AppData/"]
COPY ["src/Contracts/MyELib.Contracts/MyELib.Contracts.csproj", "src/Contracts/MyELib.Contracts/"]
COPY ["src/Domain/MyELib.Domain/MyELib.Domain.csproj", "src/Domain/MyELib.Domain/"]
COPY ["src/Infrastructure/MyELib.Infrastructure.ComponentRegistrar/MyELib.Infrastructure.ComponentRegistrar.csproj", "src/Infrastructure/MyELib.Infrastructure.ComponentRegistrar/"]
COPY ["src/Infrastructure/MyELib.Infrastructure/MyELib.Infrastructure.csproj", "src/Infrastructure/MyELib.Infrastructure/"]
RUN dotnet restore "./src/Hosts/MyELib.Hosts.Migrator/MyELib.Hosts.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Hosts/MyELib.Hosts.Migrator"
RUN dotnet build "./MyELib.Hosts.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyELib.Hosts.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyELib.Hosts.Migrator.dll"]