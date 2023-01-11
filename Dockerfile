FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /srv

COPY . /srv

RUN dotnet restore "./Source/New/MarketplaceService/MarketplaceService.csproj"
RUN dotnet build "./Source/New/MarketplaceService/MarketplaceService.csproj" -c Release -o /srv/build && \
  dotnet publish "./Source/New/MarketplaceService/MarketplaceService.csproj" -c Release -o /srv/publish

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app

COPY --from=base /srv/publish /app
EXPOSE 9696
ENTRYPOINT ["dotnet", "/app/MarketplaceService.dll"]
