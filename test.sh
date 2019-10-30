#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
./build.sh
for v in 1.0 2.0 2.1 3.0; do
    for c in Debug Release; do
        dotnet exec MoreLinq.Test/bin/$c/netcoreapp$v/MoreLinq.Test.dll
    done
done
if [[ -z `which mono 2>/dev/null` ]]; then
    echo>&2 NOTE! Mono does not appear to be installed so unit tests
    echo>&2 against the Mono runtime will be skipped.
else
    mono MoreLinq.Test/bin/Debug/net451/MoreLinq.Test.exe
    mono MoreLinq.Test/bin/Release/net451/MoreLinq.Test.exe
fi
