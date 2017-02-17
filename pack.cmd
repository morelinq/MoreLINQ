@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
set VERSION_SUFFIX=
if not "%~1"=="" set VERSION_SUFFIX=--version-suffix %1
call build /v:m ^
    && dotnet pack -c Release %VERSION_SUFFIX% MoreLinq -o dist
goto :EOF
