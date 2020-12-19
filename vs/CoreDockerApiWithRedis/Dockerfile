FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src
COPY ["CoreDockerApi.csproj", "."]
RUN dotnet restore CoreDockerApi.csproj
COPY . .
RUN dotnet build "CoreDockerApi.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/build .
ENTRYPOINT ["dotnet", "CoreDockerApi.dll"]