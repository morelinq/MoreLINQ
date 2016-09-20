@echo off
setlocal
cd "%~dp0"
chcp 1252 > nul
if exist tools\NUnit.Runners goto :test
for %%i in (NuGet.exe) do set nuget=%%~dpnx$PATH:i
if "%nuget%"=="" goto :nonuget
md tools
nuget.exe install NUnit.Runners -Version 2.6.4 -OutputDirectory tools || exit /b 1
move .\tools\NUnit.Runners.2.6.4 .\tools\NUnit.Runners
if not %errorlevel%==0 exit /b %errorlevel%

:test
call build /v:m ^
  && .\tools\NUnit.Runners\tools\nunit-console.exe .\MoreLinq.Test\bin\Debug\net40\MoreLinq.Test.dll ^
  && .\tools\NUnit.Runners\tools\nunit-console.exe .\MoreLinq.Test\bin\Release\net40\MoreLinq.Test.dll ^
  && .\tools\NUnit.Runners\tools\nunit-console.exe .\MoreLinq.Test\bin\Debug\net40-client\MoreLinq.Test.dll ^
  && .\tools\NUnit.Runners\tools\nunit-console.exe .\MoreLinq.Test\bin\Release\net40-client\MoreLinq.Test.dll
goto :EOF

:nonuget
echo NuGet executable not found in PATH
echo For more on NuGet, see http://nuget.codeplex.com
exit /b 2