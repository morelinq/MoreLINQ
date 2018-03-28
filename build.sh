#!/usr/bin/env bash
[[ -e build.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
dotnet --info
dotnet restore
dotnet restore MoreLinq.NoConflictGenerator/MoreLinq.NoConflictGenerator.csproj
codegen() {
    dest="$1"
    printf "Generating no-conflict wrappers (%s)..." "$1"
    shift
    dotnet run -p MoreLinq.NoConflictGenerator/MoreLinq.NoConflictGenerator.csproj -c Release -- "$@" > "$dest"
    printf "Done.\n"
}
codegen MoreLinq/NoConflict.g.cs -x "[/\\\\]ToDataTable\.cs$" -u System.Linq -u System.Collections MoreLinq
codegen MoreLinq/NoConflict.ToDataTable.g.cs -i "[/\\\\]ToDataTable\.cs$" -u System.Data -u System.Linq.Expressions MoreLinq
for c in Debug Release; do
    dotnet build --no-restore -c $c
done
