@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
call build ^
  && call :test net5.0 Debug ^
  && call :test net5.0 Debug ^
  && call :test netcoreapp2.1 Debug ^
  && call :test netcoreapp2.1 Release ^
  && call :test netcoreapp3.1 Debug ^
  && call :test netcoreapp3.1 Release ^
  && call :test net451 Debug ^
  && call :test net451 Release
goto :EOF

:test
setlocal
echo Testing %1 (%2)...
if %2==Debug set COVERAGE_ARGS=-p:CollectCoverage=true ^
                               -p:CoverletOutputFormat=opencover ^
                               -p:Exclude=\"[NUnit*]*,[MoreLinq]MoreLinq.Extensions.*,[MoreLinq]MoreLinq.Experimental.*\"
if %1==net451 (
    MoreLinq.Test\bin\%2\net451\MoreLinq.Test.exe
) else (
    dotnet test --no-build MoreLinq.Test -f %1 -c %2 %COVERAGE_ARGS%
)
goto :EOF
