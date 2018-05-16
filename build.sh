#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
dotnet --info
dotnet restore
for c in Debug Release; do
    dotnet build --no-restore -c $c
done
