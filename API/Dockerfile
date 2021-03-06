﻿# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY API/*.csproj ./API/
COPY Worker/*.csproj ./Worker/
COPY Shared/*.csproj ./Shared/
COPY OpenTracing.Contrib.NetCore/*.csproj ./OpenTracing.Contrib.NetCore/
RUN dotnet restore

# copy everything else and build app
COPY API/. ./API/
COPY Shared/. ./Shared/
COPY OpenTracing.Contrib.NetCore/. ./OpenTracing.Contrib.NetCore/
WORKDIR /source/API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "API.dll"]