@echo off
pushd "%~dp0"
call :main %*
popd & exit /b %ERRORLEVEL%

:main
setlocal
call build ^
  && call :clean ^
  && call :test net7.0 Debug ^
  && call :test net7.0 Release ^
  && call :test net6.0 Debug ^
  && call :test net6.0 Release ^
  && call :test net462 Debug ^
  && call :test net462 Release ^
  && call :report-cover
exit /b %ERRORLEVEL%

:clean
setlocal
cd MoreLinq.Test
if exist TestResults rd /s /q TestResults || exit /b 1
if exist TestResult.xml del TestResult.xml || exit /b 1
exit /b %ERRORLEVEL%

:test
setlocal
cd MoreLinq.Test
echo Testing %1 (%2)...
if %1==net462 (
    bin\%2\net462\MoreLinq.Test.exe
    exit /b %ERRORLEVEL%
)
dotnet test --no-build -f %1 -c %2 --settings coverlet.runsettings || exit /b 1
cd TestResults
set TEST_RESULTS_DIR=
for /f %%d in ('dir /b /od /ad') do if not defined TEST_RESULTS_DIR set TEST_RESULTS_DIR=%%~d
if not defined TEST_RESULTS_DIR (
    echo>&2 Test coverage XML not found!
    exit /b 1
)
copy "%TEST_RESULTS_DIR%\coverage.opencover.xml" coverage-%1-%2.opencover.xml > nul
exit /b %ERRORLEVEL%

:report-cover
setlocal
cd MoreLinq.Test\TestResults
dotnet reportgenerator -reports:coverage-*.opencover.xml ^
                       -reporttypes:Html;TextSummary ^
                       -targetdir:reports ^
  && type reports\Summary.txt
exit /b %ERRORLEVEL%
