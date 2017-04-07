#!/usr/bin/env bash
[[ -e test.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
./build.sh
dotnet exec MoreLinq.Test/bin/Debug/netcoreapp1.0/MoreLinq.Test.dll
dotnet exec MoreLinq.Test/bin/Release/netcoreapp1.0/MoreLinq.Test.dll
if [[ -z `which mono 2>/dev/null` ]]; then
    echo>&2 NOTE! Mono does not appear to be installed so unit tests
    echo>&2 against the Mono runtime will be skipped.
else
    mono MoreLinq.Test/bin/Debug/net452/MoreLinq.Test.exe 
    mono MoreLinq.Test/bin/Release/net452/MoreLinq.Test.exe 
fi
