FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Telematics.Simulator.ScaleOutConsole/Telematics.Simulator.ScaleOutConsole.csproj", "Telematics.Simulator.ScaleOutConsole/"]
COPY ["Telematics.Simulator.Models/Telematics.Simulator.Models.csproj", "Telematics.Simulator.Models/"]
COPY ["Telematics.Simulator.Contracts/Telematics.Simulator.Contracts.csproj", "Telematics.Simulator.Contracts/"]
COPY ["Telematics.Simulator.EndpointConfiguration/Telematics.Simulator.EndpointConfiguration.csproj", "Telematics.Simulator.EndpointConfiguration/"]
RUN dotnet restore "Telematics.Simulator.ScaleOutConsole/Telematics.Simulator.ScaleOutConsole.csproj"
COPY . .
WORKDIR "/src/Telematics.Simulator.ScaleOutConsole"
RUN dotnet build "Telematics.Simulator.ScaleOutConsole.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Telematics.Simulator.ScaleOutConsole.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Telematics.Simulator.ScaleOutConsole.dll"]