# Separate stages so if nuget packages were not changed, then no restore is needed
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS restore
WORKDIR /app
COPY InventoryManager.Api.csproj InventoryManager.Api.csproj
RUN dotnet restore

# Build the application
FROM restore as build
WORKDIR /app
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

# Run on a slimmer runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
EXPOSE 80
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "InventoryManager.Api.dll"]
