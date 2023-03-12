@echo off
setlocal
for %%e in (Community Professional Enterprise) do (
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\%%e\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\%%e\MSBuild\Current\Bin\MSBuild.exe"
    )
)
if exist "%MSBUILD%" goto :build
set MSBUILD=
for %%i in (MSBuild.exe) do set MSBUILD=%%~dpnx$PATH:i
if not defined MSBUILD goto :nomsbuild
set MSBUILD_VERSION_MAJOR=
set MSBUILD_VERSION_MINOR=
for /f "delims=. tokens=1,2,3,4" %%m in ('msbuild /version /nologo') do (
    set MSBUILD_VERSION_MAJOR=%%m
)
if not defined MSBUILD_VERSION_MAJOR goto :nomsbuild
if %MSBUILD_VERSION_MAJOR% lss 17    goto :nomsbuild
:build
"%MSBUILD%" %*
exit /b %ERRORLEVEL%

:nomsbuild
echo>&2 Microsoft Build Engine 17.0 or a later version is required to build
echo>&2 the solution. For installation instructions, see:
echo>&2 https://docs.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio
echo>&2 At the very least, you will want to install the MSBuilt Tool workload
echo>&2 that has the identifier "Microsoft.VisualStudio.Workload.MSBuildTools":
echo>&2 https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-build-tools#msbuild-tools
exit /b 1
