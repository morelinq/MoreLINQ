#!/bin/sh

set -e

RID=`dotnet --info | grep RID | sed -E 's/ *RID: *(.+)/\1/'`
mono MoreLinq.Test/bin/Debug/net451/$RID/dotnet-test-nunit.exe \
	MoreLinq.Test/bin/Debug/net451/$RID/MoreLinq.Test.dll

