#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
./build.sh
for v in 2.1 3.0; do
    for c in Debug Release; do
        if [[ "$c" == "Debug" ]]; then
            coverage_args="-p:CollectCoverage=true
                           -p:CoverletOutputFormat=opencover
                           -p:Exclude=[NUnit*]*"
        else
            unset coverage_args
        fi
        dotnet test --no-build -c $c -f netcoreapp$v MoreLinq.Test $coverage_args
    done
done
if [[ -z `which mono 2>/dev/null` ]]; then
    echo>&2 NOTE! Mono does not appear to be installed so unit tests
    echo>&2 against the Mono runtime will be skipped.
else
    mono MoreLinq.Test/bin/Debug/net451/MoreLinq.Test.exe
    mono MoreLinq.Test/bin/Release/net451/MoreLinq.Test.exe
fi
