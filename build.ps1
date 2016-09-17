dotnet --info
dotnet restore
dotnet build -c Debug .\*\project.json
dotnet build -c Release .\*\project.json