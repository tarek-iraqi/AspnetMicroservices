#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApiGateways/Shoppimg.Aggregator/Shoppimg.Aggregator.csproj", "ApiGateways/Shoppimg.Aggregator/"]
RUN dotnet restore "ApiGateways/Shoppimg.Aggregator/Shoppimg.Aggregator.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Shoppimg.Aggregator"
RUN dotnet build "Shoppimg.Aggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shoppimg.Aggregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shoppimg.Aggregator.dll"]