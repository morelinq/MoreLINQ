@echo off
setlocal
cd "%~dp0"
chcp 1252 > nul
for %%i in (dotnet.exe) do set dotnet=%%~dpnx$PATH:i
if "%dotnet%"=="" goto :nodotnet
for %%i in (NuGet.exe) do set nuget=%%~dpnx$PATH:i
if "%nuget%"=="" goto :nonuget
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
call build /v:m ^
    && dotnet pack -c Release MoreLinq\project.json -o dist ^
    && for %%i in (pkg\MoreLinq.Source.*.nuspec) do nuget pack %%i -OutputDirectory dist
goto :EOF

:nonuget
echo NuGet executable not found in PATH
echo For more on NuGet, see http://nuget.codeplex.com
exit /b 2

:nodotnet
echo dotnet executable not found in PATH
echo For more on dotnet, see https://www.microsoft.com/net/core
exit /b 2
