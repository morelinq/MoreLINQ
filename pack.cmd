@echo off
setlocal
cd "%~dp0"
chcp 1252 > nul
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

