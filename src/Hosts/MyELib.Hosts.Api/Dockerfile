FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Application/MyELib.Application.AppData/MyELib.Application.AppData.csproj", "src/Application/MyELib.Application.AppData/"]
COPY ["src/Contracts/MyELib.Contracts/MyELib.Contracts.csproj", "src/Contracts/MyELib.Contracts/"]
COPY ["src/Domain/MyELib.Domain/MyELib.Domain.csproj", "src/Domain/MyELib.Domain/"]
COPY ["src/Infrastructure/MyELib.Infrastructure/MyELib.Infrastructure.csproj", "src/Infrastructure/MyELib.Infrastructure/"]
COPY ["src/Infrastructure/MyELib.Infrastructure.ComponentRegistrar/MyELib.Infrastructure.ComponentRegistrar.csproj", "src/Infrastructure/MyELib.Infrastructure.ComponentRegistrar/"]
COPY ["src/Infrastructure/MyELib.Infrastructure.DataAccess/MyELib.Infrastructure.DataAccess.csproj", "src/Infrastructure/MyELib.Infrastructure.DataAccess/"]
COPY ["src/Hosts/MyELib.Hosts.Api/MyELib.Hosts.Api.csproj", "src/Hosts/MyELib.Hosts.Api/"]
RUN dotnet restore "./src/Hosts/MyELib.Hosts.Api/MyELib.Hosts.Api.csproj"
COPY . .
WORKDIR "/src/src/Hosts/MyELib.Hosts.Api"
RUN dotnet build "./MyELib.Hosts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyELib.Hosts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY src/Hosts/MyELib.Hosts.Api/aspnetapp.pfx /app/certificates/

ENTRYPOINT ["dotnet", "MyELib.Hosts.Api.dll"]