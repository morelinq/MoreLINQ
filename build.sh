#!/usr/bin/env bash
[[ -e build.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
which dotnet 2>/dev/null || {
    echo>&2 .NET Core does not appear to be installed on this machine, which is
    echo>&2 required to build the solution. You can install it from the URL below
    echo>&2 and then try building again:
    echo>&2 https://dot.net
    exit 1
}
set -e
dotnet --info
dotnet restore
dotnet restore MoreLinq.NoConflictGenerator/MoreLinq.NoConflictGenerator.csproj
codegen() {
    dest="$1"
    shift
    dotnet run -p MoreLinq.NoConflictGenerator/MoreLinq.NoConflictGenerator.csproj -c Release -- "$@" > "$dest"
}
printf "Generating no-conflict wrappers..."
codegen MoreLinq/NoConflict.g.cs -x "^ToDataTable$" -u System.Linq MoreLinq
codegen MoreLinq/NoConflict.ToDataTable.g.cs -i "^ToDataTable$" -u System.Data MoreLinq -u System.Linq.Expressions
printf "Done.\n"
for c in Debug Release; do
    ./msbuild.sh /v:m /p:Configuration=$c
done
