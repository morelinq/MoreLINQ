#!/usr/bin/env bash

which msbuild 2>/dev/null || {
    echo>&2 MSBuild does not appear to be installed on this system. MSBuild 15.1
    echo>&2 is the minimum required version to build the solution. You can
    echo>&2 install it as part of Mono 4.8 from the following URL and then try
    echo>&2 building again:
    echo>&2 http://www.mono-project.com/download/
    exit 1
}

SDK_DIR=$(dotnet --info | grep -E "^ *[Bb]ase [Pp]ath:" | sed -E "s/^.+: *//")
if [[ -z $SDK_DIR ]]; then
    echo>&2 Unable to determine .NET Core SDK path
    exit 1
fi

export MSBuildExtensionsPath=$SDK_DIR
export CscToolExe=$SDK_DIR/Roslyn/bincore/RunCsc
export MSBuildSDKsPath=$SDK_DIR/Sdks

msbuild "$@"
