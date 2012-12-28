@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
set MSBUILDEXE=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
if not exist "%MSBUILDEXE%" (
    echo The .NET Framework 4.0 does not appear to be installed on this 
    echo machine, which is required to build the solution.
    exit /b 1
)
if "%1"=="all"  call :sl   %2 %3 %4 %5 %6 %7 %8 %9 & goto :EOF
if "%1"=="docs" call :docs %2 %3 %4 %5 %6 %7 %8 %9 & goto :EOF
call :base %*
goto :EOF

:base
for %%i in (debug release) do "%MSBUILDEXE%" "MoreLinq.sln" /p:Configuration=%%i %*
goto :EOF

:sl
call :base %*
for %%i in (debug release) do "%MSBUILDEXE%" MoreLinq.Silverlight.sln /p:Configuration=%%i %*
goto :EOF

:docs
call :base %*
"%MSBUILDEXE%" MoreLinq.shfbproj %*
goto :EOF
