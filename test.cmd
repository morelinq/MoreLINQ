@echo off
pushd "%~dp0"
call :main %*
popd & exit /b %ERRORLEVEL%

:main
setlocal
if not defined SKIP_TEST_BUILD set SKIP_TEST_BUILD=false
if %SKIP_TEST_BUILD%==false call build || exit /b 1
if not "%~1"=="aot" goto :test-all
call :test-aot net8.0 && call :test-aot net9.0
exit /b %ERRORLEVEL%
:test-all
call :clean ^
  && call :test net9.0 Debug ^
  && call :test net9.0 Release ^
  && call :test net8.0 Debug ^
  && call :test net8.0 Release ^
  && call :test net6.0 Debug ^
  && call :test net6.0 Release ^
  && call :test net471 Debug ^
  && call :test net471 Release ^
  && call :report-cover ^
  && call :test-aot net8.0 ^
  && call :test-aot net9.0
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
if %1==net471 (
    bin\%2\net471\MoreLinq.Test.exe
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

:test-aot
setlocal
cd MoreLinq.Test.Aot
dotnet publish -f %1
if not ERRORLEVEL==0 exit /b %ERRORLEVEL%
set AOT_TEST_PUBLISH_DIR=
for /f %%d in ('cmd /c "dir /ad /s /b publish | findstr /c:%1"') do if not defined AOT_TEST_PUBLISH_DIR set AOT_TEST_PUBLISH_DIR=%%~d
if not defined AOT_TEST_PUBLISH_DIR (
    echo>&2 Published binary directory not found!
    exit /b 1
)
"%AOT_TEST_PUBLISH_DIR%\MoreLinq.Test.Aot.exe"
exit /b %ERRORLEVEL%
