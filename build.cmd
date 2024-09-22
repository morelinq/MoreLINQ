@echo off
pushd "%~dp0"
call :main %*
popd & exit /b %ERRORLEVEL%

:main
setlocal
for %%i in (dotnet.exe) do set dotnet=%%~dpnx$PATH:i
if "%dotnet%"=="" goto :nodotnet
if "%1"=="docs" shift & goto :docs
:build
dotnet restore && dotnet tool restore ^
  && call :codegen MoreLinq\Extensions.g.cs -x "[/\\]ToDataTable\.cs$" -u System.Linq -u System.Collections MoreLinq ^
  && call :codegen MoreLinq\Extensions.ToDataTable.g.cs -i "[/\\]ToDataTable\.cs$" -u System.Data -u System.Linq.Expressions MoreLinq ^
  && call MoreLinq\tt ^
  && for %%i in (debug release) do dotnet build -c %%i --no-restore %* || exit /b 1
exit /b %ERRORLEVEL%

:docs
call :build && call msbuild.cmd MoreLinq.shfbproj %1 %2 %3 %4 %5 %6 %7 %8 %9
exit /b %ERRORLEVEL%

:nodotnet
echo>&2 dotnet executable not found in PATH
echo>&2 For more on dotnet, see https://www.microsoft.com/net/core
exit /b 2

:codegen
echo | set /p=Generating extensions wrappers (%1)...
dotnet run --project bld/ExtensionsGenerator/MoreLinq.ExtensionsGenerator.csproj -c Release -- %2 %3 %4 %5 %6 %7 %8 %9 > "%temp%\%~nx1" ^
  && move "%temp%\%~nx1" "%~dp1" > nul ^
  && echo Done.
exit /b %ERRORLEVEL%
