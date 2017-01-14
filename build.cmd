@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
if "%PROCESSOR_ARCHITECTURE%"=="x86" set MSBUILD=%ProgramFiles%
if defined ProgramFiles(x86) set MSBUILD=%ProgramFiles(x86)%
set MSBUILD=%MSBUILD%\MSBuild\14.0\bin\msbuild
if not exist "%MSBUILD%" goto :nomsbuild
for %%i in (dotnet.exe) do set dotnet=%%~dpnx$PATH:i
if "%dotnet%"=="" goto :nodotnet
if "%1"=="docs" shift & goto :docs

:base
dotnet --info ^
  && dotnet restore ^
  && for %%i in (debug release) do "%MSBUILD%" "MoreLinq.sln" /v:m /p:Configuration=%%i %* || exit /b 1
goto :EOF

:docs
call :base && "%MSBUILD%" MoreLinq.shfbproj %1 %2 %3 %4 %5 %6 %7 %8 %9
goto :EOF

:nomsbuild
echo>&2 Microsoft Build Tools 2015 does not appear to be installed on this
echo>&2 machine, which is required to build the solution. You can install
echo>&2 it from the URL below and then try building again:
echo>&2 https://www.microsoft.com/en-us/download/details.aspx?id=48159
exit /b s

:nodotnet
echo>&2 dotnet executable not found in PATH
echo>&2 For more on dotnet, see https://www.microsoft.com/net/core
exit /b 2
