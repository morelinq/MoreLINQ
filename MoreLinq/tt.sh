#!/usr/bin/env bash
[[ -e tt.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
find . -name "*.tt" -print0 | xargs -0 -t -L 1 sh -c '(echo dotnet tt "$0" || exit 255)'
