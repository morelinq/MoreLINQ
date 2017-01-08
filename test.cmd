@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
set NUNIT_CONSOLE_PATH=tools\NUnit.ConsoleRunner.3.5.0\tools\nunit3-console.exe
if exist %NUNIT_CONSOLE_PATH% goto :test-all
for %%i in (NuGet.exe) do set nuget=%%~dpnx$PATH:i
if "%nuget%"=="" goto :nonuget
nuget.exe install NUnit.Console -Version 3.5.0 -OutputDirectory tools || exit /b 1
:test-all
call build ^
  && call :test netcore10 Debug ^
  && call :test netcore10 Release ^
  && call :test net451 Debug ^
  && call :test net451 Release
goto :EOF

:test
setlocal
echo Testing %1 (%2)...
if %1==netcore10 (
    dotnet test -f netcoreapp1.0 -c %2 MoreLinq.Test
) else (
    for /d %%d in (MoreLinq.Test\bin\%2\%1\win*) do (
        %NUNIT_CONSOLE_PATH% %%d\MoreLinq.Test.dll
    )
)
goto :EOF

:nonuget
echo>&2 NuGet executable not found in PATH
echo>&2 To download a version, see https://dist.nuget.org/
exit /b 2