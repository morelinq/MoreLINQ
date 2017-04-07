@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
call build ^
  && call :test netcore10 Debug ^
  && call :test netcore10 Release ^
  && call :test net452 Debug ^
  && call :test net452 Release
goto :EOF

:test
setlocal
echo Testing %1 (%2)...
if %1==netcore10 (
    dotnet exec MoreLinq.Test\bin\%2\netcoreapp1.0\MoreLinq.Test.dll
) else (
    MoreLinq.Test\bin\%2\net452\MoreLinq.Test.exe
)
goto :EOF
