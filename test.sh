#!/usr/bin/env bash
[[ -e test.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
./build.sh
dotnet test -f netcoreapp1.0 MoreLinq.Test
RID=`dotnet --info | grep RID | sed -E 's/ *RID: *(.+)/\1/'`
if [[ -z `which mono 2>/dev/null` ]]; then
    echo>&2 NOTE! Mono does not appear to be installed so unit tests
    echo>&2 against the Mono runtime will be skipped.
else
    mono MoreLinq.Test/bin/Debug/net451/$RID/dotnet-test-nunit.exe \
         MoreLinq.Test/bin/Debug/net451/$RID/MoreLinq.Test.dll
fi
