FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /srv

COPY . /srv

RUN dotnet restore "./Source/SlithinMarketplace/SlithinMarketplace.csproj"
RUN dotnet build "./Source/SlithinMarketplace/SlithinMarketplace.csproj" -c Release -o /srv/build && \
  dotnet publish "./Source/SlithinMarketplace/SlithinMarketplace.csproj" -c Release -o /srv/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

COPY --from=base /srv/publish /app
EXPOSE 9696
ENTRYPOINT ["dotnet", "/app/SlithinMarketplace.dll"]
