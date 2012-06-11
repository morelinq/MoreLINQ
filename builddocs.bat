REM TODO: Don't do this in a batch file!

REM Really force a clean build...
rmdir /s /q src\MoreLinq\bin\Release

msbuild "MoreLinq.sln" /property:Configuration=Release
msbuild MoreLinq.shfbproj
