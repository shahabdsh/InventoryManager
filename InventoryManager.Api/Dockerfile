FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY InventoryManager.Api.csproj InventoryManager.Api.csproj
RUN dotnet restore
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
EXPOSE 80
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "InventoryManager.Api.dll"]
