@echo off
dotnet build src/Skybrud.Umbraco.Time --configuration Release /t:rebuild /t:pack -p:BuildTools=1 -p:PackageOutputPath=../../releases/nuget