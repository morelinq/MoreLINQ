@echo off
setlocal
cd "%~dp0"
chcp 1252 > nul
for %%i in (NuGet.exe) do set nuget=%%~dpnx$PATH:i
if "%nuget%"=="" goto :nonuget
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
call build /v:m ^
    && nuget pack pkg\morelinq.nuspec -OutputDirectory dist -Symbols ^
    && nuget pack pkg\MoreLinq.Portable.nuspec -OutputDirectory dist -Symbols ^
    && for %%i in (pkg\MoreLinq.Source.*.nuspec) do nuget pack %%i -OutputDirectory dist
goto :EOF

:nonuget
echo NuGet executable not found in PATH
echo For more on NuGet, see http://nuget.codeplex.com
exit /b 2
