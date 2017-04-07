#!/bin/sh

set -e
set -x

for dir in MoreLinq.Test/bin/$CONFIGURATION/net452/ ; do
    mono ${dir}MoreLinq.Test.exe
done

