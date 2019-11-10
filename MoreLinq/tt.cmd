@echo off
pushd "%~dp0"
for /f "tokens=*" %%f in ('dir /s /b *.tt') do (
    echo>&2 dotnet t4 "%%f"
    dotnet t4 "%%f" || goto :end
)
:end
popd
