#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
./build.sh $c
if [[ -z "$1" ]]; then
    configs="Debug Release"
else
    configs="$1"
fi
for v in 2.1 3.0; do
    for c in $configs; do
        if [[ "$c" == "Debug" ]]; then
            coverage_args="-p:CollectCoverage=true
                           -p:CoverletOutputFormat=opencover
                           -p:Exclude=\"[NUnit*]*,[MoreLinq]MoreLinq.Extensions.*,[MoreLinq]MoreLinq.Experimental.*\""
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
    for c in $configs; do
        mono MoreLinq.Test/bin/$c/net451/MoreLinq.Test.exe
    done
fi
