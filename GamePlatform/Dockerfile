# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GamePlatform.csproj", "./"]
RUN dotnet restore "GamePlatform.csproj"
COPY . .
RUN dotnet publish "GamePlatform.csproj" -c Release -o /app/publish

# Runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GamePlatform.dll"]