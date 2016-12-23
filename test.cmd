@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
set NUNIT_CONSOLE_PATH=tools\NUnit.Runners\tools\nunit-console.exe
if exist %NUNIT_CONSOLE_PATH% goto :test-all
for %%i in (NuGet.exe) do set nuget=%%~dpnx$PATH:i
if "%nuget%"=="" goto :nonuget
nuget.exe install NUnit.Runners -Version 2.6.4 -OutputDirectory tools || exit /b 1
move tools\NUnit.Runners.2.6.4 tools\NUnit.Runners
if not %errorlevel%==0 exit /b %errorlevel%
:test-all
call build /v:m /t:Rebuild ^
  && call :test net40 Debug ^
  && call :test net40 Release
goto :EOF

:test
setlocal
echo Testing %1 (%2)...
%NUNIT_CONSOLE_PATH% MoreLinq.Test\bin\%2\%1\MoreLinq.Test.dll
goto :EOF

:nonuget
echo>&2 NuGet executable not found in PATH
echo>&2 To download a version, see https://dist.nuget.org/
exit /b 2