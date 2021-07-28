dotnet publish -c Release -r win-x64 --output ./Dist/Win-x64 ./Source/Slithin.csproj
dotnet publish -c Release -r win-x86 --output ./Dist/Win-x86 ./Source/Slithin.csproj
dotnet publish -c Release -r linux-x64 --output ./Dist/Linux-x64 ./Source/Slithin.csproj
dotnet publish -c Release -r osx-x64 --output ./Dist/MacOS-x64 ./Source/Slithin.csproj

PAUSE