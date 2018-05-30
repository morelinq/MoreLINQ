@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
for %%i in (dotnet.exe) do set dotnet=%%~dpnx$PATH:i
if "%dotnet%"=="" goto :nodotnet
if "%1"=="docs" shift & goto :docs
:build
dotnet --info ^
  && dotnet restore ^
  && for %%i in (debug release) do call msbuild.cmd "MoreLinq.sln" /v:m /p:Configuration=%%i %* || exit /b 1
goto :EOF

:docs
call :build && call msbuild.cmd MoreLinq.shfbproj %1 %2 %3 %4 %5 %6 %7 %8 %9
goto :EOF

:nodotnet
echo>&2 dotnet executable not found in PATH
echo>&2 For more on dotnet, see https://www.microsoft.com/net/core
exit /b 2
