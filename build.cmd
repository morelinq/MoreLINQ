@echo off
setlocal
pushd "%~dp0"
set MSBUILDEXE=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
if not exist "%MSBUILDEXE%" (
    echo The .NET Framework 4.0 does not appear to be installed on this 
    echo machine, which is required to build the solution.
    exit /b 1
)
for %%i in (debug release) do "%MSBUILDEXE%" "MoreLinq.sln" /p:Configuration=%%i    
if "%1"=="all" for %%i in (debug release) do "%MSBUILDEXE%" MoreLinq.Silverlight.sln /p:Configuration=%%i
popd
