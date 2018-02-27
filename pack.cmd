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
if not "%~1"=="" set VERSION_SUFFIX=/p:VersionSuffix=%1
call build /v:m ^
  && call msbuild.cmd /v:m /t:Pack                        ^
                           /p:Configuration=Release       ^
                           /p:IncludeSymbols=true         ^
                           /p:IncludeSource=true          ^
                           /p:PackageOutputPath=%cd%/dist ^
                           %VERSION_SUFFIX%               ^
                           MoreLinq\MoreLinq.csproj
goto :EOF
