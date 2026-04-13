FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5017

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Logging.API.csproj", "."]
RUN dotnet restore "./Logging.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Logging.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logging.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logging.API.dll"]