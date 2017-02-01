#!/bin/sh

set -e

for dir in MoreLinq.Test/bin/Debug/net451/*/ ; do
	mono $dir/dotnet-test-nunit.exe $dir/MoreLinq.Test.dll
done

