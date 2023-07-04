@echo off
pushd "%~dp0"
call :main %*
popd & exit /b %ERRORLEVEL%

:main
setlocal
if not exist dist md dist
if not %ERRORLEVEL%==0 exit /b %ERRORLEVEL%
set VERSION_SUFFIX=
if not "%~1"=="" set VERSION_SUFFIX=/p:VersionSuffix=%1
call build ^
  && dotnet pack --no-build -c Release %VERSION_SUFFIX%
exit /b %ERRORLEVEL%
