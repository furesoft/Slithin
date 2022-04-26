# Nutze SDK Image als base
FROM mcr.microsoft.com/dotnet/sdk:6.0 as base
# Erstele arbeitsbereich im Image
WORKDIR /srv 

# Kopiere Project in Arbeitsbereich
COPY . /srv

# Restore & Cache
RUN dotnet restore SlithinMarketplace.csproj

# Build & Publish
RUN dotnet build "SlithinMarketplace.csproj" -c Release -o /srv/build && \
    dotnet publish "SlithinMarketplace.csproj" -c Release -o /srv/publish

# Nutze runtime Image nö sehe das hier
FROM mcr.microsoft.com/dotnet/runtime:6.0
# Erstele arbeitsbereich im Image.
WORKDIR /app

# Kopiere aus base
COPY --from=base /srv/publish /app

# Starte Andwendung
CMD ["dotnet", "run", "/app/SlithinMarketplace.dll"] #The referenced project '../Slithin.Marketplace.Models/Slithin.Marketplace.Models.csproj' does not exist. [/srv/SlithinMarketplace.csproj]