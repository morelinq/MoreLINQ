#!/usr/bin/env bash
set -eo pipefail
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
for f in net8.0 net9.0 net10.0; do
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
for f in net8.0 net9.0 net10.0; do
    dotnet publish -f $f MoreLinq.Test.Aot
    "$(find MoreLinq.Test.Aot -type d -name publish | grep -F $f)/MoreLinq.Test.Aot"
done
