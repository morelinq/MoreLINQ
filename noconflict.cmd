@echo off
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
setlocal
if "%~1"=="" goto :gen-all
set ROSLYN_INSTALLED=0
for /d %%d in (tools\Microsoft.CodeAnalysis.CSharp.*) do set ROSLYN_INSTALLED=1
if %ROSLYN_INSTALLED%==0 nuget install Microsoft.CodeAnalysis.CSharp -o tools || exit /b 1
csi noconflict.csx %*
goto :EOF

:gen-all
     call :gen NoConflict.g.cs -x "^ToDataTable$" -u System.Linq ^
  && call :gen NoConflict.ToDataTable.g.cs -i "^ToDataTable$" -u System.Data -u System.Linq.Expressions
goto :EOF

:gen
call :main %2 %3 %4 %5 %6 %7 %8 %9 > %temp%\%1 && move %temp%\%1 MoreLinq > nul
goto :EOF
