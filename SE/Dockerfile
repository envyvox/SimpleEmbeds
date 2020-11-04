FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

# Copy everything else and build
COPY SE ./SE
COPY SE.Framework ./SE.Framework
COPY SE.Data ./SE.Data
COPY SE.Services ./SE.Services

COPY nuget.config ./
RUN dotnet restore SE/SE.csproj --configfile ./nuget.config
RUN dotnet publish SE/*.csproj -c Release -o out

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env ./out .
ENTRYPOINT ["dotnet", "SE.dll"]