#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
dotnet restore
codegen() {
    dest="$1"
    printf "Generating extensions wrappers (%s)..." "$1"
    shift
    dotnet run -p bld/ExtensionsGenerator/MoreLinq.ExtensionsGenerator.csproj -c Release -- "$@" > "$dest"
    printf "Done.\n"
}
codegen MoreLinq/Extensions.g.cs -x "[/\\\\]ToDataTable\.cs$" -u System.Linq -u System.Collections MoreLinq
codegen MoreLinq/Extensions.ToDataTable.g.cs -i "[/\\\\]ToDataTable\.cs$" -u System.Data -u System.Linq.Expressions MoreLinq
for c in Debug Release; do
    dotnet build --no-restore -c $c
done
