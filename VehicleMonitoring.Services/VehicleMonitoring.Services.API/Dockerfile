FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY VehicleMonitoring.sln ./
COPY VehicleMonitoring.Services/VehicleMonitoring.Services.API/VehicleMonitoring.Services.API.csproj VehicleMonitoring.Services/VehicleMonitoring.Services.API/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/VehicleMonitoring.Services/VehicleMonitoring.Services.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.Services.API.dll"]
