@echo off
setlocal
pushd "%~dp0"
set MSBUILD35EXE=%SystemRoot%\Microsoft.NET\Framework\v3.5\MSBuild.exe
if not exist "%MSBUILD35EXE%" (
    echo The .NET Framework 3.5 does not appear to be installed on this 
    echo machine, which is required to build the solution.
    exit /b 1
)
for %%i in (debug release) do "%MSBUILD35EXE%" MoreLinq.sln /p:Configuration=%%i    
if "%1"=="all" for %%i in (debug release) do "%MSBUILD35EXE%" MoreLinq.Silverlight.sln /p:Configuration=%%i
popd
