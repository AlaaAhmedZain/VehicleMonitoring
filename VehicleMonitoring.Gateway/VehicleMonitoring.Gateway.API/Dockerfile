FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY VehicleMonitoring.sln ./
COPY VehicleMonitoring.Gateway/VehicleMonitoring.Gateway.API/VehicleMonitoring.Gateway.API.csproj VehicleMonitoring.Gateway/VehicleMonitoring.Gateway.API/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/VehicleMonitoring.Gateway/VehicleMonitoring.Gateway.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.Gateway.API.dll"]
