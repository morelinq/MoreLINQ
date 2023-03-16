#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
if [[ "${SKIP_TEST_BUILD:=false}" == "false" ]]; then
    ./build.sh $c
fi
if [[ -d "MoreLinq.Test/TestResults" ]]; then
    rm -rf MoreLinq.Test/TestResults
fi
if [[ -z "$1" ]]; then
    configs="Debug Release"
else
    configs="$1"
fi
for f in net6.0 net7.0; do
    for c in $configs; do
        dotnet test --no-build -c $c -f $f --settings MoreLinq.Test/coverlet.runsettings MoreLinq.Test
        TEST_RESULTS_DIR="$(ls -dc MoreLinq.Test/TestResults/* | head -1)"
        cp "$TEST_RESULTS_DIR/coverage.opencover.xml" "MoreLinq.Test/TestResults/coverage-$f-$c.opencover.xml"
    done
done
dotnet reportgenerator -reports:MoreLinq.Test/TestResults/coverage-*.opencover.xml \
                       -reporttypes:Html\;TextSummary \
                       -targetdir:MoreLinq.Test/TestResults/reports
cat MoreLinq.Test/TestResults/reports/Summary.txt
if [[ -z `which mono 2>/dev/null` ]]; then
    echo>&2 NOTE! Mono does not appear to be installed so unit tests
    echo>&2 against the Mono runtime will be skipped.
else
    for c in $configs; do
        mono MoreLinq.Test/bin/$c/net471/MoreLinq.Test.exe
    done
fi
