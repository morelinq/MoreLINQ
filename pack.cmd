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
    && dotnet pack -c Release MoreLinq\project.json -o dist
goto :EOF

:nonuget
echo>&2 NuGet executable not found in PATH
echo>&2 To download a version, see https://dist.nuget.org/
exit /b 2

:nodotnet
echo>&2 dotnet executable not found in PATH
echo>&2 For more on dotnet, see https://www.microsoft.com/net/core
exit /b 2
