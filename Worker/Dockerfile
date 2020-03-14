# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Worker/*.csproj ./Worker/
COPY API/*.csproj ./API/
COPY Shared/*.csproj ./Shared/
COPY OpenTracing.Contrib.NetCore/*.csproj ./OpenTracing.Contrib.NetCore/
RUN dotnet restore

# copy everything else and build app
COPY Worker/. ./Worker/
COPY OpenTracing.Contrib.NetCore/. ./OpenTracing.Contrib.NetCore/
COPY Shared/. ./Shared/
WORKDIR /source/Worker
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Worker.dll"]