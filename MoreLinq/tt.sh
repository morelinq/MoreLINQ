#!/usr/bin/env bash
[[ -e tt.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
find . -name "*.tt" | xargs -t -n 1 dotnet tt
