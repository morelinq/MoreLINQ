@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
call build /v:m ^
    && dotnet pack -c Release MoreLinq\project.json -o dist
goto :EOF
