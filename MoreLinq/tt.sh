#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
find . -name "*.tt" -print0 | xargs -0 -t -L 1 sh -c '(dotnet t4 "$0" || exit 255)'
