@echo off
setlocal
set CSIPATH=
for %%i in (csi.exe) do set CSIPATH=%%~$PATH:i
if defined CSIPATH goto :main
if defined ProgramFiles(x86) set CSIPATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe && goto :main
set CSIPATH=%ProgramFiles%\MSBuild\14.0\Bin\csi.exe

:main
if not exist "%CSIPATH%" goto :nocsi
"%CSIPATH%" "%~dpn0.csx" > "%~dpn0.cs"
goto :EOF

:nocsi
>&2 echo Microsoft (R) Visual C# Interactive Compiler does not appear to be
>&2 echo installed. You can download it as part of Microsoft Build Tools 2015
>&2 echo using the URL below, install and try again:
>&2 echo https://www.microsoft.com/en-us/download/details.aspx?id=48159
exit /b 1