# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Redis/*.csproj ./Redis/
COPY TestRedis/*.csproj ./TestRedis/

# copy everything else and build app
COPY Redis/. ./Redis/
COPY TestRedis/. ./TestRedis/
WORKDIR /source/Redis
RUN dotnet publish -c debug -o /app --restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Redis.dll"]
