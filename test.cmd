@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
call build ^
  && call :test netcoreapp1.0 Debug ^
  && call :test netcoreapp1.0 Release ^
  && call :test netcoreapp2.0 Debug ^
  && call :test netcoreapp2.0 Release ^
  && call :test netcoreapp2.1 Debug ^
  && call :test netcoreapp2.1 Release ^
  && call :test netcoreapp3.0 Debug ^
  && call :test netcoreapp3.0 Release ^
  && call :test net451 Debug ^
  && call :test net451 Release
goto :EOF

:test
setlocal
echo Testing %1 (%2)...
if %1==net451 (
    MoreLinq.Test\bin\%2\net451\MoreLinq.Test.exe
) else (
    dotnet exec MoreLinq.Test\bin\%2\%1\MoreLinq.Test.dll
)
goto :EOF
